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
        private readonly IPlayerServiceAsync _playerServiceAsync;
        private readonly ILogger<GameRoomController> _logger;

        public GameRoomController(IGameRoomServiceAsync gameRoomServiceAsync, 
            IPlayerServiceAsync playerServiceAsync,
            ILogger<GameRoomController> logger)
        {
            _gameRoomServiceAsync = gameRoomServiceAsync;
            _playerServiceAsync = playerServiceAsync;
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
            return CreatedAtAction(nameof(GetById), new {id = gameRoom.Id}, gameRoom);
        }

        [HttpPost]
        [Route("{id}/join")]
        public async Task<ActionResult<GameRoom>> JoinRoom(string id, JoinRoomData joinRoomData)
        {
            var playerName = joinRoomData.PlayerName;
            _logger.LogInformation("Player {playerName:l} wants to join the room {id:l}", playerName, id);
            
            var isValidGuid = Guid.TryParse(id, out var validGameRoomId);
            if (!isValidGuid) return BadRequest();

            _logger.LogInformation("Room id is valid. Checking if it exists...");
            var gameRoom = await _gameRoomServiceAsync.GetById(validGameRoomId);
            if (gameRoom == null) return BadRequest();

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest();
            
            var gameRoomPlayer = await _gameRoomServiceAsync.JoinRoom(gameRoom, player);
            
            // TODO: create REST route to get the created resource -- that's why CreatedAtAction was not used here!
            return StatusCode(201, new {id = gameRoomPlayer.Id, playerId = player.Id, gameRoomId = gameRoom.Id});
        }
    }
}