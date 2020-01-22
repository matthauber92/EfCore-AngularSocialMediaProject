using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MovieApp.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Friends> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Posts>()
                .HasOne(u => u.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(f => f.UserId)
                .HasConstraintName("ForeignKey_UserId");

            modelBuilder.Entity<Comments>()
                .HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(f => f.PostId)
                .HasConstraintName("ForeignKey_PostsId");

            modelBuilder.Entity<Movies>()
                .HasOne(u => u.User)
                .WithMany(m => m.Movies)
                .HasForeignKey(f => f.UserId)
                .HasConstraintName("ForeignKey_UserId");

            modelBuilder.Entity<Friends>()
                .HasOne(c => c.User)
                .WithMany(b => b.Friends)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("ForeignKey_UserId");

        }
    }
}
