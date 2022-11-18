using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[PrimaryKey(nameof(user_id), nameof(shared_post_id))]
	public class Users_SharedPosts {
		public int user_id { get; set; }
		public int shared_post_id { get; set; }
	}
}
