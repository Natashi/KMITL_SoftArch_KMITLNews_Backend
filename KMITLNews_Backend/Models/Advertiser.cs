using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	public class Advertiser {
		[Key]
		public int advertiser_id { get; set; }
		public string advertiser_name { get; set; } = string.Empty;

		public string ad_image_url { get; set; } = string.Empty;
	}
}
