using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }


        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WishlistItemTag> WishlistItemTags { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
/*             builder.Ignore<BaseEntity>();

            builder.Entity<BaseEntity>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd(); */
            builder.Entity<WishlistItem>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishlistItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Tag>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WishlistItemTag>()
                .HasKey(x => new {x.WishlistItemId, x.TagId});

            builder.Entity<WishlistItemTag>()
                .HasOne(x => x.WishlistItem)
                .WithMany(w => w.WishlistItemTags)
                .HasForeignKey(x => x.WishlistItemId);
            
            builder.Entity<WishlistItemTag>()
                .HasOne(x => x.Tag)
                .WithMany(t => t.WishlistItemTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tag>()
                .HasIndex(t => new { t.UserId, t.Name})
                .IsUnique();

            

            
        }
        
    }
}