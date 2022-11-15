using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models
{
    public class Post_Delete
    {
        [Required]
        public int post_id { get; set; }

    }
	
}