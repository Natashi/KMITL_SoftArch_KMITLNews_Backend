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

		private bool CheckAuthorization(int userID) {
			var users = _context.Users.Where(i => i.user_id == userID).ToArray();
			if (users.Length == 0)
				return false;
			return CheckAuthorization(users[0]);
		}
		private static bool CheckAuthorization(User user) {
			return user.user_type == (int)UserType.Admin;
		}

		[HttpGet("GetAllUnverifiedPosts")]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllUnverifiedPosts([FromQuery] Request_Admin_Basic request) {
			if (!CheckAuthorization(request.RequesterUserID))
				return Unauthorized("No authorization.");

			var res = await _context.Posts.Where(i => !i.verified).ToListAsync();
			return Ok(res);
		}
		[HttpGet("GetAllReportedPosts")]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllReportedPosts([FromQuery] Request_Admin_Basic request) {
			if (!CheckAuthorization(request.RequesterUserID))
				return Unauthorized("No authorization.");

			var res = await _context.Users.Where(i => i.report_count > 0).ToListAsync();
			return Ok(res);
		}

		[HttpGet("GetVerificationPendingUsers")]
		public async Task<ActionResult<IEnumerable<User>>> GetVerificationPendingUsers([FromQuery] Request_Admin_Basic request) {
			if (!CheckAuthorization(request.RequesterUserID))
				return Unauthorized("No authorization.");

			var res = await _context.Users.Select(i => i.verified == (int)UserVerificationStatus.Pending).ToListAsync();
			return Ok(res);
		}

		[HttpPut("SetPostVerification/{postID}")]
		public async Task<ActionResult> SetPostVerification(int postID, Request_Admin_SetPostVerification request) {
			if (!CheckAuthorization(request.RequesterUserID))
				return Unauthorized("No authorization.");

			if (postID != request.PostID)
				return BadRequest("Post IDs do not match.");

			Post? post = await _context.Posts.FindAsync(postID);
			if (post == null)
				return BadRequest(string.Format("Post not found with id={0}", postID));

			post.verified = request.Verification;

			await _context.SaveChangesAsync();
			return Ok(post);
		}

		[HttpPut("SetUserVerification/{userID}")]
		public async Task<ActionResult> SetUserVerification(int userID, Request_Admin_SetUserVerification request) {
			if (!CheckAuthorization(request.RequesterUserID))
				return Unauthorized("No authorization.");

			if (userID != request.UserID)
				return BadRequest("User IDs do not match.");

			User? user = await _context.Users.FindAsync(userID);
			if (user == null)
				return BadRequest(string.Format("User not found with id={0}", userID));

			user.verified = request.Verification;

			await _context.SaveChangesAsync();
			return Ok(user);
		}

		[HttpDelete("RemovePost/{postID}")]
		public async Task<ActionResult> RemovePost(int postID, Request_Admin_Basic request) {
			Post? post = await _context.Posts.FindAsync(postID);
			if (post == null)
				return BadRequest(string.Format("Post not found with id={0}", postID));

			var _posterID = await _context.Posts_Users.Where(u => u.post_id == postID).ToArrayAsync();
			int posterID = _posterID.Length > 0 ? _posterID[0].user_id : -1;

			if (request.RequesterUserID != posterID && !CheckAuthorization(request.RequesterUserID))
				return Unauthorized("No authorization.");

			//Remove post from each tables
			_context.Posts.Remove(post);
			DataContext.RemoveFrom(_context.Posts_Users, i => i.post_id == postID);
			DataContext.RemoveFrom(_context.Tags_Posts, i => i.post_id == postID);
			DataContext.RemoveFrom(_context.Users_SharedPosts, i => i.shared_post_id == postID);

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpDelete("RemoveUser/{userID}")]
		public async Task<ActionResult> RemoveUser(int userID, Request_Admin_Basic request) {
			if (!CheckAuthorization(request.RequesterUserID))
				return Unauthorized("No authorization.");

			User? user = await _context.Users.FindAsync(userID);
			if (user == null)
				return BadRequest(string.Format("User not found with id={0}", userID));

			//Remove user from Users
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

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}
	}

	public class Request_Admin_Basic {
		[Required]
		public int RequesterUserID { get; set; }	//ID of the user making the request
	}
	public class Request_Admin_SetPostVerification {
		[Required]
		public int RequesterUserID { get; set; }	//ID of the user making the request
		[Required]
		public int PostID { get; set; }             //ID of the target post
		[Required]
		public bool Verification { get; set; }		//New verification status
	}
	public class Request_Admin_SetUserVerification {
		[Required]
		public int RequesterUserID { get; set; }	//ID of the user making the request
		[Required]
		public int UserID { get; set; }				//ID of the target user
		[Required]
		public int Verification { get; set; }		//New verification status
	}
}
