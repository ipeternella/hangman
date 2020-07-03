using System.Linq;
using Hangman.Models;
using Microsoft.EntityFrameworkCore;

namespace Hangman.Repository
{
    /**
     * The Database context of this application. Contains all DbSets used for operations.
     * Adds CreatedAt and UpdatedAt fields.
     */
    public class HangmanDbContext: DbContext
    {
        public HangmanDbContext(DbContextOptions<HangmanDbContext> options) : base(options)
        {
        }
        
        public DbSet<GameRoom> GameRooms { get; set; }
        
        /**
         * Saves the context to the database. Adds CreatedAt and UpdatedAt.
         */
        public override int SaveChanges()
        {
            AutomaticallyAddCreatedAndUpdatedAt();
            return base.SaveChanges();
        }
        private void AutomaticallyAddCreatedAndUpdatedAt()
        {
            var trackables = ChangeTracker.Entries<BaseEntity>();

            if (trackables != null)
            {
                // createdAt
                foreach (var item in trackables.Where(t => t.State == EntityState.Added))
                {
                    item.Entity.CreatedAt = System.DateTime.Now;
                    item.Entity.UpdatedAt = System.DateTime.Now;
                }
                
                // updatedAt
                foreach (var item in trackables.Where(t => t.State == EntityState.Modified))
                {
                    item.Entity.UpdatedAt = System.DateTime.Now;
                }
            }
        }
    }
}