using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeAgenda.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public ProjectContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEvent>().HasKey(table => new {
                table.UserId,
                table.EventId
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set;}
        public IEnumerable<object> User { get; internal set; }
        public object UserEvent { get; internal set; }

        public static implicit operator ProjectContext(User v)
        {
            throw new NotImplementedException();
        }

        internal object Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
