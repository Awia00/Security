using Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Database
{
    public class InstarootContext : DbContext
    {
        public InstarootContext(DbContextOptions<InstarootContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ImageUser> ImageUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<User>().HasMany(user => user.OwnedImages).WithOne(image => image.Owner);
            modelBuilder.Entity<User>().HasMany(user => user.Comments).WithOne(comment => comment.User);
            modelBuilder.Entity<User>().HasMany(user => user.AccessibleImages).WithOne(imageUser => imageUser.User);
            modelBuilder.Entity<ImageUser>().HasKey(imageUser => new { imageUser.UserId, imageUser.ImageId });
            modelBuilder.Entity<Image>().HasMany(image => image.Users).WithOne(imageUser => imageUser.Image);
            modelBuilder.Entity<Image>().HasMany(image => image.Comments).WithOne(comment => comment.Image);
        }
    }
}
