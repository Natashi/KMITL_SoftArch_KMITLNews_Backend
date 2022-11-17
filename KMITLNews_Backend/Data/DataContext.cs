using Microsoft.EntityFrameworkCore;

using KMITLNews_Backend.Models;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;

#pragma warning disable CS8618

namespace KMITLNews_Backend.Data
{
    public class DataContext : DbContext {
		public DataContext(DbContextOptions<DataContext> options) : base(options) {
		}

		//Entities

		public DbSet<User> Users { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Advertiser> Advertisers { get; set; }

		//Relations

		public DbSet<Posts_Users> Posts_Users { get; set; }
		public DbSet<Users_SharedPosts> Users_SharedPosts { get; set; }
		public DbSet<Users_Follows> Users_Follows { get; set; }
		public DbSet<Tags_Posts> Tags_Posts { get; set; }
		public DbSet<Tags_Follows> Tags_Follows { get; set; }

		//Extra methods

		public static IQueryable<T> RemoveFrom<T>(DbSet<T> dest, Expression<Func<T, bool>> predicate) where T : class {
			if (dest == null)
				throw new ArgumentNullException(nameof(dest));
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			var arr = dest.Where(predicate);
			foreach (var i in arr)
				dest.Remove(i);

			return arr;
		}
	}
}
