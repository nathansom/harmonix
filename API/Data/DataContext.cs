using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserLike>? Likes { get; set; }
        public DbSet<Message>? Messages { get; set; }
        public DbSet<Group>? Groups { get; set; }
        public DbSet<Connection>? Connections { get; set; }
        public DbSet<Job>? Jobs { get; set; }
        public DbSet<JobSave>? SavedJobs {get;set;}
        public DbSet<Organization>? Organizations { get; set; }
        public DbSet<OrgLike>? OrgLikes { get; set; }
        public DbSet<OrgPhoto>? OrgPhotos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<JobSave>()
              .HasKey(k => new { k.JobId, k.SavedUserId });

            builder.Entity<JobSave>()
              .HasOne<AppUser>(u=>u.SavedUser)
              .WithMany(u=>u.SavedJobs)
              .HasForeignKey(k=> k.SavedUserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobSave>()
              .HasOne<Job>(u=>u.SavedJob)
              .WithMany(u=>u.SavedByUsers)
              .HasForeignKey(k=> k.JobId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Group>()
                .HasMany(x => x.Connections)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
              .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<UserLike>()
              .HasOne(s => s.SourceUser)
              .WithMany(l => l.LikedUsers)
              .HasForeignKey(s => s.SourceUserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
              .HasOne(s => s.LikedUser)
              .WithMany(l => l.LikedByUsers)
              .HasForeignKey(s => s.LikedUserId)
              .OnDelete(DeleteBehavior.Cascade);

              builder.Entity<OrgLike>()
              .HasKey(k => new { k.OrgId, k.LikedUserId });

            // builder.Entity<OrgLike>()
            //   .HasOne(s => s.LikedUser)
            //   .WithMany(l => l.LikedByOrganizations)
            //   .HasForeignKey(s => s.LikedUserId)
            //   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
              .HasOne(u => u.Recipient)
              .WithMany(m => m.MessagesReceived)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
              .HasOne(u => u.Sender)
              .WithMany(m => m.MessagesSent)
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
