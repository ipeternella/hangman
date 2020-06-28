using Microsoft.EntityFrameworkCore;

namespace hangman.Models
{
    public class GameRoomContext : DbContext
    {
        public GameRoomContext(DbContextOptions<GameRoomContext> options) : base(options)
        {
        }
        public DbSet<GameRoom> GameRooms { get; set; }
    }
}