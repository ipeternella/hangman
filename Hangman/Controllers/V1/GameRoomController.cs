﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangman.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangman.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
        public ActionResult<IEnumerable<GameRoom>> Get()
        {
            return Ok(_context.GameRooms.ToList());
        }
        
        [HttpGet]
        [Route("async")]
        public async Task<ActionResult<IEnumerable<GameRoom>>> GetAsync()
        {
            return Ok(_context.GameRooms.ToList());
        }
    }
}