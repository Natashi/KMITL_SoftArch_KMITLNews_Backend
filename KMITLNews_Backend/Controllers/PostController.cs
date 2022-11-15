using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KMITLNews_Backend.Models;
using System.Linq;

namespace PostAPI.Controllers {
	[Route("api/[controller]")] // ตรวจค่าว่าเอาคำที่อยู่หน้า Controller มาเป็นชื่อ Route
	[ApiController]
	public class PostController : ControllerBase {
		private readonly DataContext _context;

		public PostController(DataContext context) {
			_context = context;
		}

		public async Task<ActionResult<IEnumerable<Post>>> GetAllPost() {
			return await _context.Posts.ToListAsync();
		}

		[HttpPost("CreatePost")]
		public async Task<ActionResult> PostCreate(Post_Create request) {

			var post = new Post {
				post_date = DateTime.Now,

				post_text = request.PostText,
				attached_image_url = request.AttachedImgUrl,

				verified = false,
				report_count = 0,
			};

			_context.Posts.Add(post);
			await _context.SaveChangesAsync();

			var Post_U = await _context.Posts.FirstOrDefaultAsync(u => u.post_text == request.PostText);
			if (Post_U.post_id == null) return BadRequest("not found Post_id");
			var P_User = new Posts_Users {
				user_id = request.UserID,
				post_id = Post_U.post_id,

			};
			_context.Posts_Users.Add(P_User);
			await _context.SaveChangesAsync();

			return Ok("success");


		}


		[HttpPut("UpdatePost/{id}")]
		public async Task<ActionResult> Postupdate(int id, Post_Update request) {
			var Post_id = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == request.post_id);
			var User_id = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
			if (Post_id == null || User_id == null) return BadRequest("not found");

			Post_id.post_text = request.post_text;
			Post_id.post_date = DateTime.Now;

			return Ok("Post Edit success");


		}

		[HttpPut("DeletePost/{id}")]
		public async Task<ActionResult> Postdelete(int id, Post_Delete request) {
			var Post_id_Post = await _context.Posts.FindAsync(request.post_id);
			var Post_id_PU = await _context.Posts_Users.FindAsync(request.post_id);
			var Post_id_TP = await _context.Tags_Posts.FindAsync(request.post_id);
			var Post_id_Share = await _context.Users_SharedPosts.FindAsync(request.post_id);
			if (Post_id_Post == null && Post_id_PU.post_id == id && Post_id_PU.post_id != null) return BadRequest("not found");

			_context.Posts.Remove(Post_id_Post);
			_context.Posts_Users.Remove(Post_id_PU);
			_context.Tags_Posts.Remove(Post_id_TP);
			_context.Users_SharedPosts.Remove(Post_id_Share);

			await _context.SaveChangesAsync();
			return Ok("Post Delete success");
		}


		[HttpGet("GetPostReportCount")]
		public async Task<ActionResult> GetPostReportCount() {
			return Ok(await _context.Posts.Where(u => u.report_count > 0).ToListAsync());
		}

		[HttpGet("GetPostTags/{tag}")]
		public async Task<ActionResult> GetPostTags(string tag) {
			//   
			return Ok(await _context.Tags_Follows.Where(u => u.tag_name == tag).ToListAsync());

		}

		[HttpGet("GetAllPost")]
		public async Task<ActionResult> GetAllPostc() {
			//   
			return Ok(await _context.Posts.Where(u => u.verified == true).ToListAsync());

		}

		[HttpGet("GetAllPostbyUser/{id}")]
		public async Task<ActionResult> GetAllPostbyUser(int id) {
			//   
			return Ok(await _context.Posts_Users.Where(u => u.user_id == id).ToListAsync());

		}
	}
}