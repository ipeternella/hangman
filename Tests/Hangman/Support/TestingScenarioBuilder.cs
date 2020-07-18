using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Models;
using Hangman.Repository;

namespace Tests.Hangman.Support
{
    public class TestingScenarioBuilder
    {
        private readonly HangmanDbContext _context;
        
        public TestingScenarioBuilder(HangmanDbContext context)
        {
            _context = context;
        }

        public async Task BuildScenarioWithThreeRooms(string name1 = "Room 1", string name2 = "Room 2", string name3 = "Room 3")
        {
            var gameRooms = new List<GameRoom>()
            {
                new GameRoom {Name = "Game Room 1"},
                new GameRoom {Name = "Game Room 2"},
                new GameRoom {Name = "Game Room 3"}
            };
            
            await _context.AddRangeAsync(gameRooms);
            await _context.SaveChangesAsync();
        }
    }
}