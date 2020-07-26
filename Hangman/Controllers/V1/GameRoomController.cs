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
            _logger.LogInformation("Player {playerName:l} wants to join room {id:l}", playerName, id);
            
            var gameRoom = await _gameRoomServiceAsync.GetById(id);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new {message = "Player was not found!"});
            
            var gameRoomPlayer = await _gameRoomServiceAsync.JoinRoom(gameRoom, player);
            return StatusCode(200, new {playerId = player.Id, gameRoomId = gameRoom.Id, isInRoom = gameRoomPlayer.IsInRoom});
        }

        [HttpPost]
        [Route("{id}/leave")]
        public async Task<ActionResult<GameRoom>> LeaveRoom(Guid id, JoinRoomData joinRoomData)
        {
            var playerName = joinRoomData.PlayerName;
            _logger.LogInformation("Player {playerName:l} wants to leave room {id:l}", playerName, id);
            
            var gameRoom = await _gameRoomServiceAsync.GetById(id);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});
            
            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new {message = "Player was not found!"});
            
            // leave the room, if he was previously in it
            var gameRoomPlayer = await _gameRoomServiceAsync.GetPlayerRoomData(gameRoom, player);
            if (gameRoomPlayer == null) return BadRequest(new {message = "Player not found in this room!"});

            var gameRoomPlayerUpdated = await _gameRoomServiceAsync.LeaveRoom(gameRoomPlayer);
            return StatusCode(200, new {playerId = player.Id, gameRoomId = gameRoom.Id, isInRoom = gameRoomPlayerUpdated.IsInRoom});
        }
        
        // [HttpGet]
        // [Route("{roomId}/guessword")]
        // public async Task<ActionResult<IEnumerable<GuessLetter>>> CreateGuessWordInRoom(Guid roomId, NewGuessWordData newGuessWordData)
        // {
        //     // TODO: gets all guess words of a given room
        // }
        
        // [HttpPost]
        // [Route("{roomId}/guessword")]
        // public async Task<ActionResult<GameRoom>> CreateGuessLetter(Guid roomId, Guid guessWordId)
        // {
        //     //  TODO: creates a new guess word in a given room
        // }

        // [HttpGet]
        // [Route("{roomId}/guessword/{guessWordId}/guessletter")]
        // public async Task<ActionResult<IEnumerable<GuessLetter>>> CreateGuessWord(Guid roomId, NewGuessWordData newGuessWordData)
        // {
        //     //  TODO: gets all guess letters of a given guess word
        // }

        // [HttpPost]
        // [Route("{roomId}/guessword/{guessWordId}/guessletter")]
        // public async Task<ActionResult<GameRoom>> CreateGuessWord(Guid roomId, NewGuessWordData newGuessWordData)
        // {
        //     //  TODO: creates a new guess letter for a given guess word (in a given game room)
        // }
    }
}