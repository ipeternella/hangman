using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("{gameRoomId}")]
        public async Task<ActionResult<GameRoom>> GetById(Guid gameRoomId)
        {
            _logger.LogInformation("Calling gameRoomService to get room with id: {id:l}", gameRoomId);
            var gameRoom = await _gameRoomServiceAsync.GetById(gameRoomId);
            if (gameRoom != null) return Ok(gameRoom);

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameRoom>>> All()
        {
            _logger.LogInformation("Calling gameRoomService to get all rooms...");
            var gameRooms = await _gameRoomServiceAsync.GetAll();

            _logger.LogInformation("Returning all gameRooms...");
            return Ok(gameRooms);
        }

        [HttpPost]
        public async Task<ActionResult<GameRoom>> Create(NewGameRoomData newGameRoomData)
        {
            var gameRoom = await _gameRoomServiceAsync.Create(newGameRoomData);

            _logger.LogInformation("New room has been created: {gameRoom}", gameRoom);
            return CreatedAtAction(nameof(GetById), new {gameRoomId = gameRoom.Id}, gameRoom);
        }

        [HttpPost]
        [Route("{gameRoomId}/join")]
        public async Task<ActionResult<GameRoom>> JoinRoom(Guid gameRoomId, JoinRoomData joinRoomData)
        {
            var playerName = joinRoomData.PlayerName;
            _logger.LogInformation("Player {playerName:l} wants to join room {id:l}", playerName, gameRoomId);

            var gameRoom = await _gameRoomServiceAsync.GetById(gameRoomId);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new {message = "Player was not found!"});

            var gameRoomPlayer = await _gameRoomServiceAsync.JoinRoom(gameRoom, player);
            return StatusCode(200,
                new {playerId = player.Id, gameRoomId = gameRoom.Id, isInRoom = gameRoomPlayer.IsInRoom});
        }

        [HttpPost]
        [Route("{gameRoomId}/leave")]
        public async Task<ActionResult<GameRoom>> LeaveRoom(Guid gameRoomId, JoinRoomData joinRoomData)
        {
            var playerName = joinRoomData.PlayerName;
            _logger.LogInformation("Player {playerName:l} wants to leave room {id:l}", playerName, gameRoomId);

            var gameRoom = await _gameRoomServiceAsync.GetById(gameRoomId);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new {message = "Player was not found!"});

            // leave the room, if he was previously in it
            var gameRoomPlayer = await _gameRoomServiceAsync.GetPlayerRoomData(gameRoom, player);
            if (gameRoomPlayer == null) return BadRequest(new {message = "Player not found in this room!"});

            var gameRoomPlayerUpdated = await _gameRoomServiceAsync.LeaveRoom(gameRoomPlayer);
            return StatusCode(200,
                new {playerId = player.Id, gameRoomId = gameRoom.Id, isInRoom = gameRoomPlayerUpdated.IsInRoom});
        }

        [HttpGet]
        [Route("{gameRoomId}/guessword")]
        public async Task<ActionResult<IEnumerable<GuessWord>>> GetGuessWordsInRoom(Guid gameRoomId)
        {
            _logger.LogInformation("Getting all guessed words for room {:l}", gameRoomId);
            var guessedWords = await _gameRoomServiceAsync.GetAllGuessedWords(gameRoomId);

            return Ok(guessedWords);
        }

        [HttpPost]
        [Route("{gameRoomId}/guessword")]
        public async Task<ActionResult<GuessWord>> CreateGuessWordInRoom(Guid gameRoomId, NewGuessWordData newGuessWordData)
        {
            var newGuessWord = newGuessWordData.GuessWord;
            var playerName = newGuessWordData.PlayerName;
            _logger.LogInformation("Player {playerName:l} wants to leave room {id:l}", playerName, newGuessWord);

            var gameRoom = await _gameRoomServiceAsync.GetById(gameRoomId);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new {message = "Host was not found!"});

            // TODO: only host should be able to create new guess words for the players
            // _logger.LogInformation("Checking if the player is the host of the room...");
            // var gameRoomPlayer = await _gameRoomServiceAsync.GetPlayerRoomData(gameRoom, player);
            // if (gameRoomPlayer == null || !gameRoomPlayer.IsHost)

            var createdGuessWord = await _gameRoomServiceAsync.CreateGuessWord(gameRoom, newGuessWord);
            return StatusCode(201, new {createdGuessWord.Id, GuessWord = createdGuessWord.Word});
        }

        [HttpGet]
        [Route("{gameRoomId}/guessword/{guessWordId}")]
        public async Task<ActionResult<GameStateData>> GetGuessWord(Guid gameRoomId, Guid guessWordId)
        {
            _logger.LogInformation("Getting game state for {guessWordId}:l} in {gameRoomId:l}", guessWordId,
                gameRoomId);

            var gameRoom = await _gameRoomServiceAsync.GetById(gameRoomId);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});

            var guessWord = await _gameRoomServiceAsync.GetGuessedWord(guessWordId);
            if (guessWord == null) return BadRequest(new {message = "Guess Word was not found in such room!"});

            var gameRound = guessWord.Round;
            var guessWordIfRoundIsOver = gameRound.IsOver ? guessWord.Word : null;

            return Ok(new GameStateData()
            {
                GuessWord = guessWordIfRoundIsOver,
                IsOver = gameRound.IsOver,
                PlayerHealth = guessWord.Round.Health,
                GuessWordSoFar = _gameRoomServiceAsync.GetGuessWordStateSoFar(guessWord),
                GuessedLetters = guessWord.GuessLetters.Select(letter => letter.Letter),
            });
        }

        [HttpPost]
        [Route("{gameRoomId}/guessword/{guessWordId}/guessletter")]
        public async Task<ActionResult<GameStateData>> CreateGuessWord(Guid gameRoomId, Guid guessWordId,
            NewGuessLetterData newGuessLetterData)
        {
            var guessLetterString = newGuessLetterData.GuessLetter;
            var playerName = newGuessLetterData.PlayerName;
            
            _logger.LogInformation(
                "Player {playerName:l} is guessing the letter {guessLetterString:l} for the word {guessWordId:l} in room {gameRoomId:l}",
                playerName,
                guessLetterString, guessWordId, gameRoomId);

            var gameRoom = await _gameRoomServiceAsync.GetById(gameRoomId);
            if (gameRoom == null) return BadRequest(new {message = "Game Room was not found!"});

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new {message = "Player was not found!"});

            _logger.LogInformation("Checking if the player is in the room...");
            var gameRoomPlayer = await _gameRoomServiceAsync.GetPlayerRoomData(gameRoom, player);
            if (gameRoomPlayer == null || !gameRoomPlayer.IsInRoom)
                return BadRequest(new {message = "Player is not in the room!"}); // TODO: HOST cannot make guesses!

            var guessWord = await _gameRoomServiceAsync.GetGuessedWord(guessWordId);
            if (guessWord == null) return BadRequest(new {message = "Guess Word was not found in such room!"});

            var gameRound = guessWord.Round;
            if (gameRound.IsOver) return BadRequest(new {message = "The round is over for this guess word!"});

            var alreadyGuessedLetter =
                guessWord.GuessLetters.FirstOrDefault(letter => letter.Letter == guessLetterString);
            
            if (alreadyGuessedLetter != null)
                return BadRequest(new {message = "This letter has already been guessed!"});

            var updatedGameRoundState = await _gameRoomServiceAsync.UpdateGameRoundState(guessWord, guessLetterString);
            return StatusCode(201, updatedGameRoundState);
        }
    }
}