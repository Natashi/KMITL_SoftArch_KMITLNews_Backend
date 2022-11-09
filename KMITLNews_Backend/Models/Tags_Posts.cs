using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[Keyless]
	public class Tags_Posts {
		public int post_id { get; set; }
		public int shared_post_id { get; set; }
	}
}
