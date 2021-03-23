using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Common;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

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
                        entry.Entity.CreatedTimestamp = _dateTime.NowUtc;
                        entry.Entity.CreatedByUserId = _authenticatedUser.UserId.Value;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedTimestamp = _dateTime.NowUtc;
                        entry.Entity.LastModifiedByUserId = _authenticatedUser.UserId.Value;
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
                        entry.Entity.CreatedTimestamp = _dateTime.NowUtc;
                        entry.Entity.CreatedByUserId = _authenticatedUser.UserId.Value;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedTimestamp = _dateTime.NowUtc;
                        entry.Entity.LastModifiedByUserId = _authenticatedUser.UserId.Value;
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
            });

            builder.Entity<UserImage>(entity =>
            {
                entity.Property(x => x.Image).HasColumnType("bytea");
            });

            builder.Entity<CarImage>(entity =>
            {
                entity.Property(x => x.Image).HasColumnType("bytea");
            });

            builder.Entity<UserPhone>(entity =>
            {
                entity.HasIndex(x => x.Number).IsUnique();
            });

            builder.Entity<UserRoleComposition>().HasKey(x => new {x.UserId, x.RoleId});
            
            builder.Entity<RestrictionRideComposition>().HasKey(x => new {x.RestrictionId, x.RideId});
            
            builder.Entity<RideLocationPointComposition>().HasKey(x => new {x.RideId,x.LocationPointId});

            builder.Entity<CarSeatComposition>().HasIndex(x => new {x.SeatId, x.CarId}).IsUnique();

            builder.Entity<Restriction>().HasIndex(x => x.Title);

            builder.Entity<PotentialClientNumber>().HasIndex(x => x.Phone);

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
        public DbSet<RideLocationPointComposition> RideLocationPointComposition { get; set; }
        public DbSet<RideCarSeatComposition> RideCarSeatCompositions { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<BanType> BanTypes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserFcmToken> UserFcmTokens { get; set; }
        public DbSet<PassengerToRideRequest> PassengerToRideRequests { get; set; }
        public DbSet<PotentialClientNumber> PotentialClientNumbers { get; set; }
        public DbSet<SiteVisitor> SiteVisitors { get; set; }

        #endregion
    }
}