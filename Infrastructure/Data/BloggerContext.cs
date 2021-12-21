﻿using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BloggerContext : IdentityDbContext<ApplicationUser>
    {        
        public BloggerContext(DbContextOptions<BloggerContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            //wyszukujemy wszystkie encje, które są typu AuditableEntitiy i zostały dodane lub zaktualizowane
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntitiy && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach(var entityEntry in entries)
            {
                ((AuditableEntitiy)entityEntry.Entity).LastModified = DateTime.UtcNow;

                if(entityEntry.State == EntityState.Added)
                    ((AuditableEntitiy)entityEntry.Entity).Created = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync();
        }
    }
}
