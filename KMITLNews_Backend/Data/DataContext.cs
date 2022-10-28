using KMITLNews_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace KMITLNews_Backend.Data {
	public class DataContext : DbContext {
		public DataContext(DbContextOptions<DataContext> options) : base(options) {
		}

		public DbSet<User> Users { get; set; }
	}
}
