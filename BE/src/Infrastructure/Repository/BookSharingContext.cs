using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public partial class BookSharingContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public BookSharingContext(DbContextOptions<BookSharingContext> options) : base(options) { }
        private static readonly string _ignorePrefix = "AspNet";
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public new DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<WishBook> WishBooks { get; set; }
        public DbSet<Extend> Extends { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public new DbSet<Role> Roles { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Author>()
                        .HasIndex(i => i.FullName)
                        .IsUnique();
            modelBuilder.Entity<Genre>()
                        .HasIndex(i => i.Name)
                        .IsUnique();
            modelBuilder.Entity<Language>()
                        .HasIndex(i => i.Name)
                        .IsUnique();
            modelBuilder.Entity<Book>()
                        .HasMany(b => b.Assignments)
                        .WithOne(b => b.Book)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Assignment>()
                        .HasOne(b => b.Extend)
                        .WithOne(b => b.Assignment)
                        .HasForeignKey<Assignment>(a => a.ExtendId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Book>()
                        .HasMany(b => b.Reviews)
                        .WithOne(b => b.Book)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Notification>()
                        .HasOne(b => b.PendingExtend)
                        .WithOne(b => b.Notification)
                        .HasForeignKey<Notification>(a => a.ExtendId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Notification>()
                        .HasOne(b => b.PendingAuthor)
                        .WithOne(b => b.Notification)
                        .HasForeignKey<Notification>(a => a.AuthorId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Notification>()
                        .HasOne(b => b.Review)
                        .WithOne(b => b.Notification)
                        .HasForeignKey<Notification>(a => a.ReviewId)
                        .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith(_ignorePrefix))
                {
                    entityType.SetTableName(tableName[_ignorePrefix.Length..]);
                }
            }
        }
    }
}
