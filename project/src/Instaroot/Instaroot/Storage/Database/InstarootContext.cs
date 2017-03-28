using Instaroot.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Instaroot.Storage.Database
{
    public class InstarootContext : IdentityDbContext<User>
    {
        public InstarootContext(DbContextOptions options) : base(options) { }
        public InstarootContext() {}

        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ImageUser> ImageUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<User>().HasMany(user => user.OwnedImages).WithOne(image => image.Owner);
            modelBuilder.Entity<User>().HasMany(user => user.Comments).WithOne(comment => comment.User);
            modelBuilder.Entity<User>().HasMany(user => user.AccessibleImages).WithOne(imageUser => imageUser.User);
            modelBuilder.Entity<ImageUser>().HasKey(imageUser => new { imageUser.UserId, imageUser.ImageId });
            modelBuilder.Entity<Image>().HasMany(image => image.Users).WithOne(imageUser => imageUser.Image).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Image>().HasMany(image => image.Comments).WithOne(comment => comment.Image).OnDelete(DeleteBehavior.Cascade); ;

            //modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new {l.LoginProvider, l.ProviderKey, l.UserId});
        }
    }
}
