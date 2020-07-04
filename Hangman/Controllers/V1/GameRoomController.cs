using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly ILogger<GameRoomController> _logger;

        public GameRoomController(IHangmanRepositoryAsync<GameRoom> repository, ILogger<GameRoomController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GameRoom>> Get(string id)
        {
            var roomId = new Guid(id);
            var gameRoom = await _repository.GetById(roomId);

            if (gameRoom == null)
            {
                return NotFound();
            }

            return Ok(gameRoom);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameRoom>>> Get()
        {
            var gameRooms = await _repository.All();
            return Ok(gameRooms);
        }
    }
}