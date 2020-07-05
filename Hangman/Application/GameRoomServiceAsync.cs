using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Models;
using Hangman.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangman.Application
{
    public class NewGameRoomData
    {
        public string Name { get; set; }
    }

    /**
     * Application service that is used to perform CRUD operations over the entity
     * GameRoom.
     */
    public class GameRoomServiceAsync : IGameRoomServiceAsync
    {
        private readonly IHangmanRepositoryAsync<GameRoom> _repository;
        private readonly ILogger<GameRoomServiceAsync> _logger;
        
        public GameRoomServiceAsync(IHangmanRepositoryAsync<GameRoom> repository, ILogger<GameRoomServiceAsync> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        public async Task<GameRoom> GetById(Guid id)
        {
            var gameRoom = await _repository.GetById(id);

            return gameRoom;
        }

        public async Task<IEnumerable<GameRoom>> GetAll()
        {
            var gameRooms = await _repository.All();
            
            return gameRooms;
        }
        
        public async Task<GameRoom> Create(NewGameRoomData newGameRoomData)
        {
            var newGameRoom = new GameRoom {Name = newGameRoomData.Name};
            await _repository.Save(newGameRoom);

            return newGameRoom;
        }
    }
}