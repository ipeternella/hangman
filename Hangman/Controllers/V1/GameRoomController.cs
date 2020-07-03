using System;
using System.Collections.Generic;
using System.Linq;
using Hangman.Repository;
using Hangman.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangman.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameRoomController : ControllerBase
    {
        
        private readonly HangmanRepository<GameRoom> _repository;
        private readonly ILogger<GameRoomController> _logger;

        public GameRoomController(HangmanRepository<GameRoom> repository, ILogger<GameRoomController> logger)
        {
            _repository = repository;
            _logger = logger;

            if (_repository.All().Any()) return;

            _repository.Save(new GameRoom {Name = "Game Room Cadinho"});
            _repository.Save(new GameRoom {Name = "Game Room Cadinho"});
        }

        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<GameRoom> Get(string id)
        {
            var roomId = new Guid(id);
            var gameRoom = _repository.FindById(roomId);

            if (gameRoom == null)
            {
                return NotFound();
            }

            return Ok(gameRoom);
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<GameRoom>> Get()
        {
            return Ok(_repository.All());
        }
    }
}