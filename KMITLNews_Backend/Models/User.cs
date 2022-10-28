using System.ComponentModel.DataAnnotations;

namespace KMITLNews_Backend.Models {
	public class User {
		[Key]
		public int UserID { get; set; }

		public string Email { get; set; } = string.Empty;
		public string PassHash { get; set; } = string.Empty;
		public string MobileNo { get; set; } = string.Empty;

		public string FName { get; set; } = string.Empty;
		public string LName { get; set; } = string.Empty;
		public string? ProfilePicUrl { get; set; }
		public string DisplayName { get; set; } = string.Empty;

		public int UserType { get; set; }
		public int VerifiedStatus { get; set; }
		public int ReportCount { get; set; }
	}
}
