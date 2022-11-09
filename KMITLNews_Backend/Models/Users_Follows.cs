using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[Keyless]
	public class Users_Follows {
		public int user_id { get; set; }
		public int follower_id { get; set; }
	}
}
