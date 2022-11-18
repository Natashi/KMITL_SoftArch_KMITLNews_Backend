using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[PrimaryKey(nameof(post_id), nameof(user_id))]
	public class Posts_Users {
		public int post_id { get; set; }
		public int user_id { get; set; }
	}
}
