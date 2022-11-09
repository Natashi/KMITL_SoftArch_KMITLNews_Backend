using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[Keyless]
	public class Posts_Users {
		public int post_id { get; set; }
		public int user_id { get; set; }
	}
}
