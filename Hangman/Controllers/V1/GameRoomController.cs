using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangman.Application;
using Hangman.DTOs;
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
        public async Task<ActionResult<GameRoom>> Create(GameRoomDTO gameRoomDTO)
        {
            var gameRoom = await _gameRoomServiceAsync.Create(gameRoomDTO);
            _logger.LogInformation("New room has been created: {@gameRoom}", gameRoom);

            return CreatedAtAction(nameof(GetById), new { gameRoomId = gameRoom.Id }, gameRoom);
        }

        [HttpPost]
        [Route("{gameRoomId}/join")]
        public async Task<ActionResult<PlayerInRoomDTO>> JoinRoom(Guid gameRoomId, PlayerDTO playerDTO)
        {
            var joinRoomDTO = new JoinRoomDTO { GameRoomId = gameRoomId, PlayerId = playerDTO.PlayerId, IsHost = false };
            _logger.LogInformation("Start join room validation: {@joinRoomDTO}", joinRoomDTO);

            var validator = new GameRoomPlayerValidator(_gameRoomServiceAsync, _playerServiceAsync);
            var validationResult = validator.Validate(joinRoomDTO);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            _logger.LogInformation("Validations were successfull, adding player to the room...");
            var playerInRoomDTO = await _gameRoomServiceAsync.JoinRoom(joinRoomDTO);

            return StatusCode(200, playerInRoomDTO);
        }

        [HttpPost]
        [Route("{gameRoomId}/leave")]
        public async Task<ActionResult<PlayerInRoomDTO>> LeaveRoom(Guid gameRoomId, PlayerDTO playerDTO)
        {
            var leaveRoomDTO = new LeaveRoomDTO { GameRoomId = gameRoomId, PlayerId = playerDTO.PlayerId };
            _logger.LogInformation("Leave room data received: {@leaveRoomDTO}", leaveRoomDTO);

            var validator = new PlayerPreviouslyInRoomValidator(_gameRoomServiceAsync, _playerServiceAsync);
            var validationResult = validator.Validate(leaveRoomDTO);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            _logger.LogInformation("Validations were successfull, removing player from the room...");
            var playerInRoomDTO = await _gameRoomServiceAsync.LeaveRoom(leaveRoomDTO);

            return StatusCode(200, playerInRoomDTO);
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
        public async Task<ActionResult<GuessWord>> CreateGuessWordInRoom(Guid gameRoomId, GuessWordRequestDTO guessWordRequestDTO)
        {
            _logger.LogInformation("New guess word creation: {@guessWordRequestDTO}", guessWordRequestDTO);
            var guessWordDTO = new GuessWordDTO
            {
                GameRoomId = gameRoomId,
                PlayerId = guessWordRequestDTO.PlayerId,
                GuessWord = guessWordRequestDTO.GuessWord
            };

            var validator = new GuessWordCreationHostValidation(_gameRoomServiceAsync, _playerServiceAsync);
            var validationResult = validator.Validate(guessWordDTO);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            _logger.LogInformation("Validations were successfull, removing player from the room...");
            var guessWordResponseDTO = await _gameRoomServiceAsync.CreateGuessWord(guessWordDTO);

            return StatusCode(201, guessWordResponseDTO);
        }

        [HttpGet]
        [Route("{gameRoomId}/guessword/{guessWordId}")]
        public async Task<ActionResult<GameStateData>> GetGuessWord(Guid gameRoomId, Guid guessWordId)
        {
            _logger.LogInformation("Getting game state for {guessWordId}:l} in {gameRoomId:l}", guessWordId,
                gameRoomId);

            var guessWord = await _gameRoomServiceAsync.GetGuessedWord(guessWordId);
            if (guessWord == null || guessWord.GameRoom.Id != gameRoomId)
                return BadRequest(new { message = "Guess Word was not found in such room!" });

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

            var guessWord = await _gameRoomServiceAsync.GetGuessedWord(guessWordId);
            if (guessWord == null || guessWord.GameRoom.Id != gameRoomId)
                return BadRequest(new { message = "Guess Word was not found in such room!" });

            var gameRoom = await _gameRoomServiceAsync.GetById(gameRoomId);  // gameRoomId obligatory matches guess word rooms' Id
            if (gameRoom == null) return BadRequest(new { message = "Game Room was not found!" });

            _logger.LogInformation("Room was found. Checking if player is valid...");
            var player = await _playerServiceAsync.GetByPlayerName(playerName);
            if (player == null) return BadRequest(new { message = "Player was not found!" });

            _logger.LogInformation("Checking if the player is in the room...");
            var gameRoomPlayer = await _gameRoomServiceAsync.GetPlayerRoomData(gameRoom.Id, player.Id);
            if (gameRoomPlayer == null || !gameRoomPlayer.IsInRoom)
                return BadRequest(new { message = "Player is not in the room!" }); // TODO: HOST cannot make guesses!

            var gameRound = guessWord.Round;
            if (gameRound.IsOver) return BadRequest(new { message = "The round is over for this guess word!" });

            var alreadyGuessedLetter =
                guessWord.GuessLetters.FirstOrDefault(letter => letter.Letter == guessLetterString);

            if (alreadyGuessedLetter != null)
                return BadRequest(new { message = "This letter has already been guessed!" });

            var updatedGameRoundState = await _gameRoomServiceAsync.UpdateGameRoundState(guessWord, guessLetterString);
            return StatusCode(201, updatedGameRoundState);
        }
    }
}