using KMITLNews_Backend.Models;
using Microsoft.EntityFrameworkCore;

using KMITLNews_Backend.Models.User;

namespace KMITLNews_Backend.Data {
	public class DataContext : DbContext {
		public DataContext(DbContextOptions<DataContext> options) : base(options) {
		}

		public DbSet<User> Users { get; set; }
	}
}
