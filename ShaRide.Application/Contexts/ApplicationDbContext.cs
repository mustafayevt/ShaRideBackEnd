using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Common;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime,
            IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        #region Method overriding

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId.Value;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId.Value;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId.Value;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId.Value;
                        break;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }

            base.OnModelCreating(builder);

            #region Mapping

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
                entity.Property(e => e.Img)
                    .HasColumnType("bytea");
            });

            builder.Entity<User>()
                .Property(e => e.Img)
                .HasColumnType("bytea");

            builder.Entity<UserRoleComposition>().HasKey(x => new {x.UserId, x.RoleId});
            
            builder.Entity<RestrictionRideComposition>().HasKey(x => new {x.RestrictionId, x.RideId});

            builder.Entity<Restriction>().HasIndex(x => x.Title).IsUnique();

            #endregion
        }

        #endregion

        #region DbSets

        public DbSet<User> Users { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleComposition> UserRoleComposition { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationPoint> LocationPoints { get; set; }
        public DbSet<Restriction> Restrictions { get; set; }
        public DbSet<RestrictionRideComposition> RestrictionRideComposition { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<BanType> BanTypes { get; set; }
        public DbSet<Seat> Seats { get; set; }

        #endregion
    }
}