using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models
{
    public class Advertiser_Create
    {
        [Required]
        public string advertiser_name { get; set; } = string.Empty;
        [Required]
        public string ad_image_url { get; set; } = string.Empty;
    }
}
