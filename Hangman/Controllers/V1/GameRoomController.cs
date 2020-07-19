using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Application;
using Hangman.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangman.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameRoomController : ControllerBase
    {
        private readonly IGameRoomServiceAsync _gameRoomServiceAsync;
        private readonly ILogger<GameRoomController> _logger;

        public GameRoomController(ILogger<GameRoomController> logger, IGameRoomServiceAsync gameRoomServiceAsync)
        {
            _gameRoomServiceAsync = gameRoomServiceAsync;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GameRoom>> GetById(string id)
        {
            _logger.LogInformation("Calling gameRoomService to get room with id: {id:l}", id);
            var isValidGuid = Guid.TryParse(id, out var validGuid);
            if (!isValidGuid) return BadRequest();
            
            var gameRoom = await _gameRoomServiceAsync.GetById(validGuid);
            if (gameRoom != null) return Ok(gameRoom);
            
            return NotFound();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameRoom>>> All()
        {
            _logger.LogInformation("Calling gameRoomService to get all rooms...");
            var gameRooms = await _gameRoomServiceAsync.GetAll();
            
            _logger.LogInformation($"Returning all gameRooms: {@gameRooms}", gameRooms);
            return Ok(gameRooms);
        }

        [HttpPost]
        public async Task<ActionResult<GameRoom>> Create(NewGameRoomData newGameRoomData)
        {
            var gameRoom = await _gameRoomServiceAsync.Create(newGameRoomData);
            
            _logger.LogInformation("New room has been created: {@gameRoom}", gameRoom);
            return CreatedAtAction(nameof(GetById), new { id = gameRoom.Id }, gameRoom);
        }
    }
}