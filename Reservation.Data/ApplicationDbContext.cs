using Microsoft.EntityFrameworkCore;
using Reservation.Data.Entities;
using Reservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is ITablesAudit && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = (ITablesAudit)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
            }
        }

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FavoriteReservation> FavoriteReservations { get; set; }
        public DbSet<Entities.Reservation> Reservations { get; set; }
    }
}
