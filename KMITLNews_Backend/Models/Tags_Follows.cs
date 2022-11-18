using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	[PrimaryKey(nameof(tag_name), nameof(follower_id))]
	public class Tags_Follows {
		public string tag_name { get; set; } = string.Empty;
		public int follower_id { get; set; }
	}
}
