using KMITLNews_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase {
		private readonly DataContext _context;

		public AdminController(DataContext context) {
			_context = context;
		}

		[HttpGet("GetAllUnverifiedPosts")]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllUnverifiedPosts() {
			var res = await _context.Posts.Select(i => !i.verified).ToListAsync();
			return Ok(res);
		}
		[HttpGet("GetAllReportedPosts")]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllReportedPosts() {
			var res = await _context.Users.Select(i => i.report_count > 0).ToListAsync();
			return Ok(res);
		}

		[HttpGet("GetVerificationPendingUsers")]
		public async Task<ActionResult<IEnumerable<User>>> GetVerificationPendingUsers() {
			var res = await _context.Users.Select(i => i.verified == (int)UserVerificationStatus.Pending).ToListAsync();
			return Ok(res);
		}

		[HttpPut("SetPostVerification/{id}")]
		public async Task<ActionResult> SetPostVerification(int postID, Request_Admin_SetPostVerification request) {
			if (postID != request.PostID)
				return BadRequest("Post IDs do not match.");

			var posts = await _context.Posts.Where(i => i.post_id == postID).ToArrayAsync();
			if (posts.Length == 0)
				return BadRequest(string.Format("Post not found with id={0}", postID));

			Post post = posts[0];
			post.verified = request.Verification;

			return Ok(post);
		}

		[HttpPut("SetUserVerification/{id}")]
		public async Task<ActionResult> SetUserVerification(int userID, Request_Admin_SetUserVerification request) {
			if (userID != request.UserID)
				return BadRequest("User IDs do not match.");

			var users = await _context.Users.Where(i => i.user_id == userID).ToArrayAsync();
			if (users.Length == 0)
				return BadRequest(string.Format("User not found with id={0}", userID));

			User user = users[0];
			user.verified = request.Verification;

			return Ok(user);
		}

		[HttpDelete("RemovePost/{id}")]
		public async Task<ActionResult> RemovePost(int userID) {
			var users = await _context.Users.Where(i => i.user_id == userID).ToArrayAsync();
			if (users.Length == 0)
				return BadRequest(string.Format("User not found with id={0}", userID));

			//Remove user from Users
			User user = users[0];
			_context.Users.Remove(user);

			//Remove user from Users_Follows
			DataContext.RemoveFrom(_context.Users_Follows, i => i.user_id == userID);
			DataContext.RemoveFrom(_context.Users_Follows, i => i.follower_id == userID);

			//Remove user from Tags_Follows
			DataContext.RemoveFrom(_context.Tags_Follows, i => i.follower_id == userID);

			//Remove user's posts
			int[] userPostIDs;
			{
				var userPosts = DataContext.RemoveFrom(_context.Posts_Users, i => i.user_id == userID);
				userPostIDs = userPosts.Select(i => i.post_id).ToArray();
			}

			//Remove user from Users_SharedPosts
			DataContext.RemoveFrom(_context.Users_SharedPosts, i => i.user_id == userID);

			//Remove orphaned posts from Users_SharedPosts
			DataContext.RemoveFrom(_context.Users_SharedPosts, i => userPostIDs.Contains(i.shared_post_id));

			//Remove posts from Tags_Posts
			DataContext.RemoveFrom(_context.Tags_Posts, i => userPostIDs.Contains(i.post_id));

			//Remove "dead" tags from Tags_Follows
			//Not doing that for now.

			return Ok("Success.");
		}
	}

	public class Request_Admin_SetPostVerification {
		[Required]
		public int PostID { get; set; }
		[Required]
		public bool Verification { get; set; }
	}
	public class Request_Admin_SetUserVerification {
		[Required]
		public int UserID { get; set; }
		[Required]
		public int Verification { get; set; }
	}
}
