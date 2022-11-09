using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models.User
{
    public class UserEdit
    {
        //public string first_name { get; set; } = string.Empty;
        //public string last_name { get; set; } = string.Empty;
        public string profile_pic_url { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
    }
}
