using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KMITLNews_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace PostAPI.Controllers
{
    [Route("api/[controller]")] // ตรวจค่าว่าเอาคำที่อยู่หน้า Controller มาเป็นชื่อ Route
	[ApiController]
	public class PostController : ControllerBase {
		private readonly DataContext _context;

    public class PostController : ControllerBase{
        private readonly DataContext _context;

        
       
       public PostController(DataContext context)
		{
			_context = context;
		}

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPost()
		{
			return await _context.Posts.ToListAsync();
		}




        [HttpPost("CreateShare/{id}")]
        public async Task<ActionResult> PostCreate(int id, Post_Share request)
        {  
            var post_check = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == request.post_id);
            var user_check = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
            if (post_check == null || user_check == null) return BadRequest("Don't found DATA");

            var chare_post = new Post
            {
                post_date = DateTime.Now,
                post_text = post_check.post_text,
                attached_image_url = post_check.attached_image_url,
                verified = false,
                report_count = 0,
            };

            _context.Posts.Add(chare_post);
            await _context.SaveChangesAsync();

            var Share_Post = await _context.Posts.MaxAsync(u => u.post_id);
            //var Share_Post = await _context.Posts.LastAsync();
            var state_share = new Users_SharedPosts
            {
                user_id = id,
                shared_post_id = Share_Post

            };
            _context.Users_SharedPosts.Add(state_share);
            await _context.SaveChangesAsync();
            return Ok("success");

        }

		}


		[HttpPut("UpdatePost/{id}")]
		public async Task<ActionResult> Postupdate(int id, Post_Update request) {
			var Post_id = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == request.post_id);
			var User_id = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
			if (Post_id == null || User_id == null) return BadRequest("not found");


        [HttpGet("GetAllPostsSharedByUser/{id}")]
        public async Task<ActionResult> GetAllPostsSharedByUser(int id)
        {
            return Ok(await _context.Users_SharedPosts.Where(u => u.user_id == id).ToListAsync());
        }


        [HttpGet("GetAllTags")]
        public async Task<ActionResult<IEnumerable<Tags_Follows>>> GetAllTags()
        {
            return Ok(await _context.Tags_Follows.ToListAsync());
        }

		}

        [HttpGet("GetFollowingByUser/{id}")]
        public async Task<ActionResult> GetFollowingByUser(int id)
        {
            return Ok(await _context.Users_Follows.Where(u => u.user_id == id).ToListAsync());
        }

        [HttpGet("GetAllPostsByTags/{tags}")]
        public async Task<ActionResult> GetAllPostsByTags(string tags)
        {
            return Ok(await _context.Tags_Posts.Where(u => u.tag_name == tags).ToListAsync());

        }


    }
}
