using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookstoreContext : IdentityDbContext<ApiUser>
{
    public BookstoreContext()
    {
    }

    public BookstoreContext(DbContextOptions<BookstoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Identity
        base.OnModelCreating(modelBuilder);

        // Author
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(60);
            entity.Property(e => e.LastName).HasMaxLength(60);
        });

        // Book
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.Isbn).HasMaxLength(20);
            entity.Property(e => e.Summary).HasMaxLength(60);
            entity.Property(e => e.Title).HasMaxLength(30);

            entity.HasOne(d => d.Author).WithMany(p => p.Books).HasForeignKey(d => d.AuthorId);
        });

        // Roles
        modelBuilder.Entity<IdentityRole>().HasData(
                       new IdentityRole
                       {
                           Name = "User",
                           NormalizedName = "USER",
                           Id = "5b769c88-641f-40e3-a7dd-b915954d7c45"
                       },
                       new IdentityRole
                       {
                           Name = "Administrator",
                           NormalizedName = "ADMINISTRATOR",
                           Id = "ea3e0a66-e19b-4e74-9cfe-08ac95b45464"
                       }
       );

        // Users
        var hasher = new PasswordHasher<ApiUser>();

        modelBuilder.Entity<ApiUser>().HasData(
                       new ApiUser
                       {
                           Id = "6795541e-56df-413c-9806-75bbc76180b4",
                           Email = "admin@bookstore.com",
                           NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                           Username = "admin@bookstore.com",
                           NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                           FirstName = "System",
                           LastName = "Administrator",
                           PasswordHash = hasher.HashPassword(null, "P@ssw0rd1"),
                       },
                       new ApiUser
                       {
                           Id = "4d154ce2-c7e8-43eb-9abf-e35319e9d9f6",
                           Email = "admin@bookstore.com",
                           NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                           Username = "admin@bookstore.com",
                           NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                           FirstName = "System",
                           LastName = "Administrator",
                           PasswordHash = hasher.HashPassword(null, "P@ssw0rd1"),
                       });


        // Assign users to roles
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            // User role
            {
                RoleId = "5b769c88-641f-40e3-a7dd-b915954d7c45",
                UserId = "4d154ce2-c7e8-43eb-9abf-e35319e9d9f6"
            },
            // Admin role
            new IdentityUserRole<string>
            {
                RoleId = "ea3e0a66-e19b-4e74-9cfe-08ac95b45464",
                UserId = "6795541e-56df-413c-9806-75bbc76180b4"
            }
            );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
