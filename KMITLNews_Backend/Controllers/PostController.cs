using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KMITLNews_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Azure;
using System.ComponentModel.DataAnnotations;
using Azure.Core;



#pragma warning disable CS1998

namespace KMITLNews_Backend.Controllers {
	[Route("api/[controller]")] // ตรวจค่าว่าเอาคำที่อยู่หน้า Controller มาเป็นชื่อ Route
	[ApiController]

	public class PostController : ControllerBase {
		private readonly DataContext _context;
		public PostController(DataContext context) {
			_context = context;
		}

		public static async Task<Post[]> GetAllPostStatic(DataContext ctx) {
			return await ctx.Posts.ToArrayAsync();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllPost() {
			return Ok(GetAllPostStatic(_context));
		}

		[HttpGet("GetPost/{postID}")]
		public async Task<ActionResult<Post>> GetPost(int postID) {
			Post? post = await _context.Posts.FindAsync(postID);
			if (post == null)
				return BadRequest("Post not found.");
			
			return Ok(post);
		}

		[HttpGet("GetPostOwner/{postID}")]
		public async Task<ActionResult<Post>> GetPostOwner(int postID) {
			Post? post = await _context.Posts.FindAsync(postID);
			if (post == null)
				return BadRequest("Post not found.");

			var postUser = await _context.Posts_Users.FirstOrDefaultAsync(u => u.post_id == postID);
			if (postUser == null)
				return BadRequest();

			return Ok(postUser.user_id);
		}

		private void AddTags(int postID, string[]? tags) {
			if (tags != null) {
				foreach (string iTag in tags) {
					Tags_Posts tagsPosts = new Tags_Posts {
						tag_name = iTag,
						post_id = postID,
					};
					_context.Tags_Posts.Add(tagsPosts);
				}
			}
		}

		[HttpPost("CreatePost/{user_id}")]
		public async Task<ActionResult> CreatePost(int user_id, Request_Post_Create request) {
			var post = new Post {
				post_date = DateTime.Now,
				post_text = request.PostText,
				attached_image_url = request.AttachedImgUrl,
				verified = false,
				report_count = 0,
			};

			var entityEntry = _context.Posts.Add(post);
			await _context.SaveChangesAsync();

			Post addedPost = entityEntry.Entity;

			var P_User = new Posts_Users {
				user_id = user_id,
				post_id = addedPost.post_id,
			};
			_context.Posts_Users.Add(P_User);

			AddTags(addedPost.post_id, request.Tags);

			await _context.SaveChangesAsync();
			return Ok(entityEntry.Entity.post_id);
		}

		[HttpPut("AddTagsToPost/{postID}")]
		public async Task<ActionResult> AddTagsToPost(int postID, Request_Post_AddTagsToPost request) {
			Post? post = await _context.Posts.FindAsync(postID);
			if (post == null)
				return BadRequest("Post not found.");

			AddTags(postID, request.Tags);

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpPut("UpdatePost/{id}")]
		public async Task<ActionResult> UpdatePost(int id, Request_Post_Update request) {
			var Post_id = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == request.PostID);
			var User_id = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
			if (Post_id == null || User_id == null)
				return BadRequest("Post not found.");

			Post_id.post_text = request.PostText;
			Post_id.attached_image_url = request.AttachedImageUrl;
			Post_id.post_date = DateTime.Now;

			return Ok("Success.");
		}

		[HttpPut("ResetReportCount/{id}")]
		public async Task<ActionResult> ResetReportCount(int id) {
			var Post_id = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == id);
			if (Post_id == null)
				return BadRequest("Post not found.");

			Post_id.report_count = 0;

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpPut("AddReportCount/{id}")]
		public async Task<ActionResult> AddReportCount(int id) {
			var Post_id = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == id);
			if (Post_id == null)
				return BadRequest("Post not found.");

			++(Post_id.report_count);

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpGet("GetPostReportCount/{postID}")]
		public async Task<ActionResult> GetPostReportCount(int postID) {
			var post = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == postID);
			if (post == null)
				return BadRequest("Post not found.");

			return Ok(post.report_count);
		}

		[HttpGet("GetPostTags/{postID}")]
		public async Task<ActionResult> GetPostTags(int postID) {
			var post = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == postID);
			if (post == null)
				return BadRequest("Post not found.");

			string[] tags = await _context.Tags_Posts.Where(i => i.post_id == postID).Select(i => i.tag_name).ToArrayAsync();

			return Ok(tags);
		}

		[HttpGet("GetAllVerifiedPost")]
		public async Task<ActionResult> GetAllVerifiedPost() {
			return Ok(await _context.Posts.Where(u => u.verified == true).ToListAsync());
		}

		[HttpGet("GetAllPostbyUser/{userID}")]
		public async Task<ActionResult> GetAllPostbyUser(int userID) {
			int[] postIDs = await _context.Posts_Users.Where(i => i.user_id == userID).Select(i => i.post_id).ToArrayAsync();
			Post[] posts = await _context.Posts.Where(i => postIDs.Contains(i.post_id)).ToArrayAsync();
			return Ok(posts);
		}

		[HttpPost("CreateShare/{id}")]
		public async Task<ActionResult> CreateShare(int id, Request_Post_Share request) {
			var post_check = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == request.PostID);
			var user_check = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
			if (post_check == null || user_check == null)
				return BadRequest("User or post not found.");

			var state_share = new Users_SharedPosts {
				user_id = id,
				shared_post_id = post_check.post_id,
			};
			_context.Users_SharedPosts.Add(state_share);

			await _context.SaveChangesAsync();
			return Ok("Success.");

		}

		[HttpGet("GetAllPostsSharedByUser/{id}")]
		public async Task<ActionResult> GetAllPostsSharedByUser(int id) {
			return Ok(await _context.Users_SharedPosts.Where(u => u.user_id == id).Select(i => i.shared_post_id).ToArrayAsync());
		}

		[HttpGet("GetAllTags")]
		public async Task<ActionResult<IEnumerable<Tags_Follows>>> GetAllTags() {
			var tags = _context.Tags_Posts.Select(i => i.tag_name).ToHashSet();

			return Ok(tags.ToArray());
		}

		[HttpGet("GetAllPostsByTags/{tag}")]
		public async Task<ActionResult> GetAllPostsByTag(string tag) {
			int[] postIDs = await _context.Tags_Posts.Where(i => i.tag_name == tag).Select(i => i.post_id).ToArrayAsync();
			Post[] posts = await _context.Posts.Where(i => postIDs.Contains(i.post_id)).ToArrayAsync();

			return Ok(posts);
		}
	}

	public class Request_Post_Create {
		[Required]
		public string PostText { get; set; } = string.Empty;
		[Required]
		public string AttachedImgUrl { get; set; } = string.Empty;
		public string[]? Tags { get; set; }     //List of tags to add
	}
	public class Request_Post_Update {
		[Required]
		public int PostID { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string PostText { get; set; } = string.Empty;
		public string AttachedImageUrl { get; set; } = string.Empty;
	}
	public class Request_Post_Share {
		[Required]
		public int PostID { get; set; }
	}
	public class Request_Post_AddTagsToPost {
		[Required]
		public string[]? Tags { get; set; }		//List of tags to add
	}
}
