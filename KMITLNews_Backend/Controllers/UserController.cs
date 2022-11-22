using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

using KMITLNews_Backend.Models;
using System.ComponentModel.DataAnnotations;
using KMITLNews_Backend.Controllers;

namespace KMITLNews_Backend.Controllers {
	[Route("api/[controller]")] // ตรวจค่าว่าเอาคำที่อยู่หน้า Controller มาเป็นชื่อ Route
	[ApiController]
	
	public class UserController : ControllerBase {
		// ----------------------------------------- Connect DB

		private readonly DataContext _context;
		
		public UserController(DataContext context)
		{
			_context = context;
		}

		// ----------------------------------------- Get AllUser / id

		[HttpGet]                      // ส่งค่าชื่อ user ทั้งหมดกลับ
		public async Task<ActionResult<IEnumerable<User>>> GetAllUser()
		{
			return await _context.Users.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			// หา id นั้น
			var users = await _context.Users.FindAsync(id);
			if (users == null) return NotFound();

			return Ok(users.email);
		}

		// ----------------------------------------- Register User

		[HttpPost("register")]
		public async Task<ActionResult> RegisterUser(Request_User_Register request)
		{

			// check exist email
			if (_context.Users.Any(u => u.email == request.email))
			{
				return BadRequest("User already exists.");
			}

			// สร้าง hast salt รหัส
			CreatePassHash(request.password, out byte[] pass_hash, out byte[] pass_salt);

			// data User
			var user = new User
			{
				email = request.email,
				pass_hash = pass_hash,
				pass_salt = pass_salt,
				mobile_no = request.mobile_no,
				first_name = request.first_name,
				last_name = request.last_name,
				display_name = request.first_name + " " + request.last_name,
				verificationToken = CreateRandomToken()
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();  // รอให้ save การเปลี่ยนแปลงข้อมูลลง DB

			return Ok("User created successfully.");
		}

		// method passHash
		private static void CreatePassHash(string password, out byte[] pass_hash, out byte[] pass_salt)
		{

			using (var hmac = new HMACSHA512())
			{
				pass_salt = hmac.Key;
				pass_hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private static string CreateRandomToken()  // randome token ไปใช้ในการ reset รหัส
		{
			return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
		}

		// ----------------------------------------- Login User

		[HttpPost("Login")]
		public async Task<ActionResult> LoginUser(Request_User_Login request)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.email == request.email);

			if (user == null) return BadRequest("User not found.");

			if(!VerifypassHash(request.password, user.pass_hash, user.pass_salt))
				return BadRequest("Incorrect password.");

			Post[] posts = await PostController.GetAllPostbyUserStatic(_context, user.user_id);

			return Ok(new Dictionary<string, object>() {
				["user_id"] = user.user_id,
				["email"] = user.email,
				["posts"] = posts,
			});
		}

		// confirm password
		private static bool VerifypassHash(string password, byte[] pass_hash, byte[] pass_salt)
		{
			using (var hmac = new HMACSHA512(pass_salt)) // ใส่ salt pass เพื่อยืนยัน
			{ 
				// hash pass ที่ส่งมา เพื่อนำมาเปรียบเทียบ
				var compute_hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return compute_hash.SequenceEqual(pass_hash);   // เปรียบเทียบเหมือนกันไหม ถ้าเหมือนส่ง true
			}
		}

		// ----------------------------------------- Edit Profile

		[HttpPut("Edit/{id}")]
		public async Task<ActionResult> EditUser(int id, Request_User_Edit request)
		{
			var user_find = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);

			// check exist id
			if (user_find == null )
				return BadRequest("User not found.");

			// update data User
			//user_find.first_name = request.first_name;
			//user_find.last_name = request.last_name;
			user_find.profile_pic_url = request.profile_pic_url;
			user_find.display_name = request.display_name;

			await _context.SaveChangesAsync();  // รอให้ save การเปลี่ยนแปลงข้อมูลลง DB
			return Ok(user_find);
		}

		// ----------------------------------------- reset password


		[HttpGet("VerifyEmail/{email}")]
		public async Task<ActionResult<User>> GetEmailUser(string email)
		{
			// หา email นั้น
			var user = await _context.Users.FirstOrDefaultAsync(x => x.email == email);
			if (user == null)
				return BadRequest("Email not found.");

			return Ok(user.verificationToken);
		}


		[HttpPut("ResetPass/{token}")]
		public async Task<ActionResult> ResetPassUser(string token, Request_User_ResetPass request)
		{
			var verify_token_User = await _context.Users.FirstOrDefaultAsync(u => u.verificationToken == token);

			// check exist id
			if (verify_token_User == null)
				return BadRequest("Invalid token.");

			// สร้าง hast salt รหัส
			CreatePassHash(request.password, out byte[] pass_hash, out byte[] pass_salt);
		   
			verify_token_User.pass_hash = pass_hash ;
			verify_token_User.pass_salt = pass_salt;
			verify_token_User.verificationToken = CreateRandomToken();

			await _context.SaveChangesAsync();  // รอให้ save การเปลี่ยนแปลงข้อมูลลง DB
			return Ok("Success.");
		}

		[HttpPut("ResetReportCount/{id}")]
		public async Task<ActionResult> ResetReportCount(int id) {
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
			if (user == null)
				return BadRequest("User not found.");

			user.report_count = 0;

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpPut("AddReportCount/{id}")]
		public async Task<ActionResult> AddReportCount(int id) {
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
			if (user == null)
				return BadRequest("User not found.");

			++(user.report_count);

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpPut("RequestVerification/{userID}")]
		public async Task<ActionResult> RequestVerification(int userID) {
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.user_id == userID);
			if (user == null)
				return BadRequest("User not found.");

			user.verified = (int)UserVerificationStatus.Pending;

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpPut("FollowUser/{id}")]
		public async Task<ActionResult> FollowUser(int id, Request_User_FollowUser request) {
			User? user = await _context.Users.FindAsync(id);
			if (user == null)
				return BadRequest("User not found.");

			User? userToFollow = await _context.Users.FindAsync(request.UserToFollow);
			if (userToFollow == null)
				return BadRequest("UserToFollow not found.");

			var follow = new Users_Follows {
				user_id = userToFollow.user_id,
				follower_id = user.user_id,
			};
			_context.Users_Follows.Add(follow);

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}
		[HttpDelete("UnFollowUser/{id}")]
		public async Task<ActionResult> UnFollowUser(int id, Request_User_FollowUser request) {
			User? user = await _context.Users.FindAsync(id);
			if (user == null)
				return BadRequest("User not found.");

			User? userToUnfollow = await _context.Users.FindAsync(request.UserToFollow);
			if (userToUnfollow == null)
				return BadRequest("UserToUnfollow not found.");

			_context.Users_Follows.Remove(new Users_Follows {
				user_id = userToUnfollow.user_id,
				follower_id = user.user_id,
			});

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		[HttpPut("FollowTag/{tag}")]
		public async Task<ActionResult> FollowTag(int id, Request_User_FollowTag request) {
			User? user = await _context.Users.FindAsync(id);
			if (user == null)
				return BadRequest("User not found.");

			var follow = new Tags_Follows {
				tag_name = request.tag,
				follower_id = user.user_id,
			};
			_context.Tags_Follows.Add(follow);

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}
		[HttpDelete("UnFollowTag/{tag}")]
		public async Task<ActionResult> UnFollowTag(int id, Request_User_FollowTag request) {
			User? user = await _context.Users.FindAsync(id);
			if (user == null)
				return BadRequest("User not found.");

			_context.Tags_Follows.Remove(new Tags_Follows {
				tag_name = request.tag,
				follower_id = user.user_id,
			});

			await _context.SaveChangesAsync();
			return Ok("Success.");
		}

		//Get all the users following this user
		[HttpGet("GetFollowersByUser/{id}")]
		public async Task<ActionResult> GetFollowersByUser(int id) {
			return Ok(await _context.Users_Follows.Where(u => u.user_id == id)
				.Select(i => i.follower_id).ToArrayAsync());
		}

		//Gets all the users followed by this user
		[HttpGet("GetFollowingUsers/{userID}")]
		public async Task<ActionResult> GetFollowingUsers(int userID) {
			var user = await _context.Users.FirstOrDefaultAsync(x => x.user_id == userID);
			if (user == null)
				return BadRequest("User not found.");

			var following = await _context.Users_Follows.Where(x => x.follower_id == userID)
				.Select(x => x.user_id).ToListAsync();

			return Ok(following);
		}

		[HttpGet("GetFollowingTags/{userID}")]
		public async Task<ActionResult<User>> GetFollowingTags(int userID) {
			var user = await _context.Users.FirstOrDefaultAsync(x => x.user_id == userID);
			if (user == null)
				return BadRequest("User not found.");

			var following = await _context.Tags_Follows.Where(x => x.follower_id == userID)
				.Select(x => x.tag_name).ToListAsync();

			return Ok(following);
		}
	}

	public class Request_User_Register {
		[Required, EmailAddress]
		public string email { get; set; } = string.Empty;
		[Required, MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
		public string password { get; set; } = string.Empty;
		[Required, Compare("password")]
		public string confirmPassword { get; set; } = string.Empty;
		[Required, Phone]
		public string mobile_no { get; set; } = string.Empty;
		[Required]
		public string first_name { get; set; } = string.Empty;
		[Required]
		public string last_name { get; set; } = string.Empty;
		//public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
	}
	public class Request_User_Login {
		[Required, EmailAddress]
		public string email { get; set; } = string.Empty;
		[Required]
		public string password { get; set; } = string.Empty;
	}
	public class Request_User_Edit {
		//public string first_name { get; set; } = string.Empty;
		//public string last_name { get; set; } = string.Empty;
		public string profile_pic_url { get; set; } = string.Empty;
		public string display_name { get; set; } = string.Empty;
	}
	public class Request_User_ResetPass {
		//public string verificationToken { get; set; } = string.Empty;
		public string password { get; set; } = string.Empty;

	}
	public class Request_User_FollowUser {
		[Required]
		public int UserToFollow { get; set; }
	}
	public class Request_User_FollowTag {
		[Required]
		public string tag { get; set; } = string.Empty;
	}
}
