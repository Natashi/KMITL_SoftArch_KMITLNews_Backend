using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[Keyless]
	public class Users_SharedPosts {
		public int user_id { get; set; }
		public int shared_post_id { get; set; }
	}
}
