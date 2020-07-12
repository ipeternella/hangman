using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Application;
using Hangman.Models;
using Hangman.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangman.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameRoomController : ControllerBase
    {
        
        private readonly IHangmanRepositoryAsync<GameRoom> _repository;
        private readonly IGameRoomServiceAsync _gameRoomServiceAsync;
        private readonly ILogger<GameRoomController> _logger;

        public GameRoomController(IHangmanRepositoryAsync<GameRoom> repository, ILogger<GameRoomController> logger, IGameRoomServiceAsync gameRoomServiceAsync)
        {
            _gameRoomServiceAsync = gameRoomServiceAsync;
            _repository = repository;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GameRoom>> GetById(string id)
        {
            var isValidGuid = Guid.TryParse(id, out var validGuid);
            if (!isValidGuid) return BadRequest();

            var gameRoom = await _gameRoomServiceAsync.GetById(validGuid);
            return Ok(gameRoom);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameRoom>>> All()
        {
            _logger.LogInformation("Calling gameRoomService...");
            var gameRooms = await _gameRoomServiceAsync.GetAll();
            
            _logger.LogInformation($"Returning all gameRooms: {@gameRooms}", gameRooms);
            return Ok(gameRooms);
        }

        [HttpPost]
        public async Task<ActionResult<GameRoom>> Create(NewGameRoomData newGameRoomData)
        {
            var gameRoom = await _gameRoomServiceAsync.Create(newGameRoomData);
            return CreatedAtAction(nameof(GetById), new { id = gameRoom.Id }, gameRoom);
        }
    }
}