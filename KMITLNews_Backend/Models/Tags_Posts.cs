using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[PrimaryKey(nameof(tag_name), nameof(post_id))]
	public class Tags_Posts {
		public string tag_name { get; set; } = string.Empty;
		public int post_id { get; set; }
	}
}
