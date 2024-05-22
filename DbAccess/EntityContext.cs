using Microsoft.EntityFrameworkCore;
using Models.EntityModels;

namespace DbAccess
{
    public class EntityContext : DbContext
    {
        public DbSet<ChatLobby> ChatLobbies { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wish> Wishes { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<History> Histories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            //optionsBuilder.UseSqlServer("Server=DESKTOP-G01L98C;Database=H5Diagram;Integrated Security=SSPI;TrustServerCertificate=True;").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.UseSqlServer("Server=localhost;Database=H5Diagram;Integrated Security=SSPI;TrustServerCertificate=True;").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
      .HasMany(u => u.WishLists)
      .WithOne(wl => wl.Owner)
      .HasForeignKey(wl => wl.OwnerId)
      .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<History>()
                .HasOne(h => h.WishList)
                .WithMany()
                .HasForeignKey(h => h.WishListId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WishList>().HasOne(x => ChatLobbies).WithOne().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
