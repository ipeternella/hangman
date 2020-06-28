using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hangman.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hangman.Controllers
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

            if (_context.GameRooms.Count() == 0)
            {
                _context.GameRooms.Add(new GameRoom { Name = "Game Room Cadinho" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        [Route("~/gamerooms")]
        public IEnumerable<GameRoom> Get()
        {
            return _context.GameRooms.ToList();
        }
    }
}