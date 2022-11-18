using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[PrimaryKey(nameof(user_id), nameof(follower_id))]
	public class Users_Follows {
		public int user_id { get; set; }
		public int follower_id { get; set; }
	}
}
