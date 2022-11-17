using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KMITLNews_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace PostAPI.Controllers {
	[Route("api/[controller]")] // ตรวจค่าว่าเอาคำที่อยู่หน้า Controller มาเป็นชื่อ Route
	[ApiController]

	public class PostController : ControllerBase {
		private readonly DataContext _context;
		public PostController(DataContext context) {
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllPost() {
			return await _context.Posts.ToListAsync();
		}

[HttpPost("CreatePost/{user_id}")]
    public async Task<ActionResult> PostCreate(int user_id,Post_Create request){
        
        	var post = new Post
			{
				post_date = DateTime.Now,
				post_text = request.post_text,
				attached_image_url = request.attached_image_url,
                verified = false,
                report_count =0,
		

			};

			_context.Posts.Add(post);
			await _context.SaveChangesAsync(); 
      var Post_U =await _context.Posts.FirstOrDefaultAsync(u=>u.post_text == request.post_text);
      if (Post_U.post_id==null) return BadRequest("not found Post_id");
      var P_User= new Posts_Users
     {

         user_id = user_id,
         post_id = Post_U.post_id,
        
      };
      _context.Posts_Users.Add(P_User);
			await _context.SaveChangesAsync(); 
			
			return Ok("success");
   

    }


      [HttpPut("UpdatePost/{id}")]
       public async Task<ActionResult> Postupdate(int id,Post_Update request){
        var Post_id =await _context.Posts.FirstOrDefaultAsync(u=>u.post_id == request.post_id);
        var User_id =await _context.Users.FirstOrDefaultAsync(u=>u.user_id == id);
        if(Post_id==null || User_id==null) return BadRequest("not found");
        
        Post_id.post_text = request.post_text;
        Post_id.post_date = DateTime.Now;

			return Ok("Post Edit success");
   

    }

      [HttpPut("UpdateReportCountToZero/{id}")]
       public async Task<ActionResult> UpdateReportCountToZero(int id){
        var Post_id =await _context.Posts.FirstOrDefaultAsync(u=>u.post_id == id);
        
        if(Post_id==null) return BadRequest("not found");
        
        Post_id.report_count= 0;
        

			return Ok("Post Edit success");
   

    }

      [HttpPut("UpdateReportCount/{id}")]
       public async Task<ActionResult> UpdateReportCount(int id){
        var Post_id =await _context.Posts.FirstOrDefaultAsync(u=>u.post_id == id);
        
        if(Post_id==null) return BadRequest("not found");
        
        Post_id.report_count= Post_id.report_count + 1;
        

			return Ok("Post Edit success");
    }


    [HttpPut("DeletePost/{id}")]
       public async Task<ActionResult> Postdelete(int id,Post_Delete request){
        var Post_id_Post = await _context.Posts.FindAsync(request.post_id);
        var Post_id_PU = await _context.Posts_Users.FindAsync(request.post_id);
        var Post_id_TP = await _context.Tags_Posts.FindAsync(request.post_id);
        var Post_id_Share = await _context.Users_SharedPosts.FindAsync(request.post_id);
        if(Post_id_Post==null && Post_id_PU.post_id == id && Post_id_PU.post_id != null ) return BadRequest("not found");
    
        _context.Posts.Remove(Post_id_Post);
        _context.Posts_Users.Remove(Post_id_PU);
        _context.Tags_Posts.Remove(Post_id_TP);
        _context.Users_SharedPosts.Remove(Post_id_Share);
        

        await _context.SaveChangesAsync();
        return Ok("Post Delete success");
    }


     [HttpGet("GetPostReportCount")]
       public async Task<ActionResult> GetPostReportCount(){
        
        return Ok(await _context.Posts.Where(u=>u.report_count > 0).ToListAsync());

      
       
    }

    [HttpGet("GetPostTags/{tag}")]
       public async Task<ActionResult> GetPostTags(string tag){
       var targetIDs = await _context.Tags_Follows.Where(u=>u.tag_name == tag).Select(u=>u.post_id).ToListAsync();

    
       var PostID_tag = _context.Posts.AsEnumerable().Where(
        u=>{
          foreach(int Post_id in targetIDs){
            if(targetIDs.Contains(u.post_id)){
              return true;
            }
          }
          return false;
        }
       ).ToList();

     //   
      return Ok(PostID_tag);

    }

    [HttpGet("GetAllPost")]
       public async Task<ActionResult> GetAllPostc(){
     //   
      return Ok(await _context.Posts.Where(u=>u.verified== true).ToListAsync());

    }

        [HttpGet("GetAllPostbyUser/{id}")]
       public async Task<ActionResult> GetAllPostbyUser(int id){
     //   
      var targetIDs = await _context.Posts_Users.Where(u=>u.user_id == id).Select(u=>u.post_id).ToListAsync();

    
       var PostID_ByUsers = _context.Posts.AsEnumerable().Where(
        u=>{
          foreach(int id in targetIDs){
            if(targetIDs.Contains(u.post_id)){
              return true;
            }
          }
          return false;
        }
       ).ToList();

         return Ok(PostID_ByUsers);
      

    }
    
    [HttpPost("CreateShare/{id}")]
		public async Task<ActionResult> CreateShare(int id, Post_Share request) {
			var post_check = await _context.Posts.FirstOrDefaultAsync(u => u.post_id == request.post_id);
			var user_check = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);
			if (post_check == null || user_check == null) return BadRequest("Don't found DATA");

			var chare_post = new Post {
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
			var state_share = new Users_SharedPosts {
				user_id = id,
				shared_post_id = Share_Post

			};
			_context.Users_SharedPosts.Add(state_share);
			await _context.SaveChangesAsync();
			return Ok("success");

		}

		[HttpGet("GetAllPostsSharedByUser/{id}")]
		public async Task<ActionResult> GetAllPostsSharedByUser(int id) {
			return Ok(await _context.Users_SharedPosts.AnyAsync(u => u.user_id == id));
		}

		[HttpGet("GetAllTags")]
		public async Task<ActionResult<IEnumerable<Tags_Follows>>> GetAllTags() {
			return await _context.Tags_Follows.ToListAsync();
		}


		[HttpGet("GetFollowingByUser/{id}")]
		public async Task<ActionResult> GetFollowingByUser(int id) {
			return Ok(await _context.Users_Follows.AnyAsync(u => u.user_id == id));
		}

		[HttpGet("GetAllPostsByTags/{tags}")]
		public async Task<ActionResult> GetAllPostsByTags(string tags) {
			return Ok(await _context.Tags_Posts.AnyAsync(u => u.tag_name == tags));
		}
	}
}
