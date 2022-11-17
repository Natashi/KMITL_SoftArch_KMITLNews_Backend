using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models
{
    // Real schema 
    public class User
    {
        [Key]  // บอกถึงว่าเป็น primary key
        public int user_id { get; set; }
        public string email { get; set; } = string.Empty;
        public byte[] pass_hash { get; set; } = new byte[32];
        public byte[] pass_salt { get; set; } = new byte[32];
        public string mobile_no { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string verificationToken { get; set; } = string.Empty;
        public string profile_pic_url { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int user_type { get; set; }
        public int verified { get; set; }
        public int report_count { get; set; }
    }

	public enum UserType : int {
		Normal = 0,
		Admin,
	}
	public enum UserVerificationStatus : int {
		Unverified = 0,
		Pending,
		Verified,
	}
}
