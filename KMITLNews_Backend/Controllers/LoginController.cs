using KMITLNews_Backend.Models.User;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace UserAPI.Controllers
{
	[Route("api/[controller]")] // ตรวจค่าว่าเอาคำที่อยู่หน้า Controller มาเป็นชื่อ Route
	[ApiController]
	
	public class UserController : ControllerBase {
		// ----------------------------------------- Connect DB

		private readonly DataContext _context;
		//Constructure
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
		public async Task<ActionResult> RegisterUser(UserRegister request)
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
		public async Task<ActionResult> LoginUser(UserLogin request)
		{

			var user = await _context.Users.FirstOrDefaultAsync(x => x.email == request.email);

			if (user == null) return BadRequest("User not found!");

			if(!VerifypassHash(request.password,user.pass_hash,user.pass_salt)) return BadRequest("Incorrect password.");

			return Ok($"user_id : {user.user_id}  ,  Email : {user.email}"); 
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
		public async Task<ActionResult> EditUser(int id,UserEdit request)
		{
			var user_find = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);

			// check exist id
			if (user_find == null )
			{
				return BadRequest("Id user dont found");
			}

			// update data User
			//user_find.first_name = request.first_name;
			//user_find.last_name = request.last_name;
			user_find.profile_pic_url = request.profile_pic_url;
			user_find.display_name = request.display_name;

			await _context.SaveChangesAsync();  // รอให้ save การเปลี่ยนแปลงข้อมูลลง DB

			return Ok("User info edited.");
		}

		// ----------------------------------------- reset password


		[HttpGet("VerifyEmail/{email}")]
		public async Task<ActionResult<User>> GetEmailUser(string email)
		{
			// หา email นั้น
			var user = await _context.Users.FirstOrDefaultAsync(x => x.email == email);
			if (user == null) return BadRequest("User not found.");

			return Ok(user.verificationToken);
		}


		[HttpPut("ResetPass/{token}")]
		public async Task<ActionResult> ResetPassUser(string token, UserResetPass request)
		{
			var verify_token_User = await _context.Users.FirstOrDefaultAsync(u => u.verificationToken == token);

			// check exist id
			if (verify_token_User == null)
			{
				return BadRequest("Invalid token.");
			}

			// สร้าง hast salt รหัส
			CreatePassHash(request.password, out byte[] pass_hash, out byte[] pass_salt);
		   
			verify_token_User.pass_hash = pass_hash ;
			verify_token_User.pass_salt = pass_salt;
			verify_token_User.verificationToken = CreateRandomToken();

			await _context.SaveChangesAsync();  // รอให้ save การเปลี่ยนแปลงข้อมูลลง DB


			return Ok("Password reset successfully.");
		}
	}
}
