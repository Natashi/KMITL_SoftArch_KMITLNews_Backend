using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models
{
    public class Post_Create
    {
        [Required]
        public int user_id { get; set; }
        [Required(AllowEmptyStrings=false)]
        public string post_text { get; set; } = string.Empty;
        [Required]
        public string attached_image_url { get; set; } = string.Empty;
    }
	
}