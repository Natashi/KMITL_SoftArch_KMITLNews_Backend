using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	public class Post {
		[Key]
		public int post_id { get; set; }
		public DateTime post_date { get; set; }

		public string post_text { get; set; } = string.Empty;
		public string attached_image_url { get; set; } = string.Empty;

		public bool verified { get; set; }
		public int report_count { get; set; }
	}
}
