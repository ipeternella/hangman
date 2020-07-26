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
        public async Task<ActionResult<GameRoom>> GetById(Guid id)
        {
            _logger.LogInformation("Calling gameRoomService to get room with id: {id:l}", id);
            var gameRoom = await _gameRoomServiceAsync.GetById(id);
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
        public async Task<ActionResult<GameRoom>> JoinRoom(Guid id, JoinRoomData joinRoomData)
        {
            var playerName = joinRoomData.PlayerName;
            _logger.LogInformation("Player {playerName:l} wants to join the room {id:l}", playerName, id);
            
            var gameRoom = await _gameRoomServiceAsync.GetById(id);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new {message = "Player was not found!"});
            
            await _gameRoomServiceAsync.JoinRoom(gameRoom, player);
            return StatusCode(200, new {playerId = player.Id, gameRoomId = gameRoom.Id});
        }
    }
}