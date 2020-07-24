using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangman.Models;
using Microsoft.EntityFrameworkCore;

namespace Hangman.Repository
{
    /**
     * The Database context of this application. Contains all DbSets used for operations.
     * Adds CreatedAt and UpdatedAt fields.
     */
    public class HangmanDbContext : DbContext
    {
        public HangmanDbContext(DbContextOptions<HangmanDbContext> options) : base(options)
        {
        }

        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GameRoomPlayer> GameRoomPlayers { get; set; }

        /**
         * Saves the context to the database. Adds CreatedAt and UpdatedAt.
         */
        public override int SaveChanges()
        {
            AutomaticallyAddCreatedAndUpdatedAt();
            return base.SaveChanges();
        }

        /**
         * Saves the context to the database (Async version).
         */
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AutomaticallyAddCreatedAndUpdatedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameRoomPlayer>()
                .HasKey(gameRoomPlayer => new {gameRoomPlayer.GameRoomId,  gameRoomPlayer.PlayerId});

            modelBuilder.Entity<GameRoomPlayer>()
                .HasOne(gameRoomPlayer => gameRoomPlayer.Player)
                .WithMany(user => user.GameRoomPlayers)
                .HasForeignKey(gameRoomPlayer => gameRoomPlayer.PlayerId);

            modelBuilder.Entity<GameRoomPlayer>()
                .HasOne(gameRoomPlayer => gameRoomPlayer.GameRoom)
                .WithMany(gameRoom => gameRoom.GameRoomPlayers)
                .HasForeignKey(gameRoomPlayer => gameRoomPlayer.GameRoomId);
        }

        private void AutomaticallyAddCreatedAndUpdatedAt()
        {
            var entitiesOnDbContext = ChangeTracker.Entries<BaseEntity>();

            if (entitiesOnDbContext == null) return; // nothing was changed on DB context

            // createdAt addition
            foreach (var item in entitiesOnDbContext.Where(t => t.State == EntityState.Added))
            {
                item.Entity.CreatedAt = System.DateTime.Now;
                item.Entity.UpdatedAt = System.DateTime.Now;
                
                if (item.Entity.GetType().Name == "GameRoomPlayer")
                {
                    item.Entity.Id = Guid.NewGuid();
                }
            }

            // updatedAt addition
            foreach (var item in entitiesOnDbContext.Where(t => t.State == EntityState.Modified))
            {
                item.Entity.UpdatedAt = System.DateTime.Now;
            }
        }
    }
}