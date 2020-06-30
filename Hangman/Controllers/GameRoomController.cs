using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangman.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangman.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameRoomController : ControllerBase
    {

        // for DI
        private readonly GameRoomContext _context;
        private readonly ILogger<GameRoomController> _logger;

        public GameRoomController(GameRoomContext context, ILogger<GameRoomController> logger)
        {
            _context = context;
            _logger = logger;

            if (_context.GameRooms.Any()) return;
            
            _context.GameRooms.Add(new GameRoom { Name = "Game Room Cadinho" });
            _context.SaveChanges();
        }

        [HttpGet]
        [Route("~/gamerooms")]
        public IEnumerable<GameRoom> Get()
        {
            return _context.GameRooms.ToList();
        }
    }
}