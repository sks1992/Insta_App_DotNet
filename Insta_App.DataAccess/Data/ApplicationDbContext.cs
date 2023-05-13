using Insta_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Insta_App.DataAccess.Data
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }
        public DbSet<User> User { get; set; }
    }
}
