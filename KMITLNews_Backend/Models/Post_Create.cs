using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models
{
    public class Post_Create
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public string PostText { get; set; } = string.Empty;

        [Required]
        public string AttachedImgUrl { get; set; } = string.Empty;
    }
	
}