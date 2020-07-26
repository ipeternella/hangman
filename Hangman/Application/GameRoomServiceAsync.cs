using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hangman.Models;
using Hangman.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hangman.Application
{
    public class JoinRoomData
    {
        [Required] public string PlayerName { get; set; }
    }

    public class NewGameRoomData
    {
        [Required] public string Name { get; set; }
    }

    /**
     * Application service that is used to perform CRUD operations over the entity
     * GameRoom.
     */
    public class GameRoomServiceAsync : IGameRoomServiceAsync
    {
        private readonly IHangmanRepositoryAsync<GameRoomPlayer> _repositoryGameRoomPlayer;
        private readonly IHangmanRepositoryAsync<GameRoom> _repository;
        private readonly ILogger<GameRoomServiceAsync> _logger;

        public GameRoomServiceAsync(IHangmanRepositoryAsync<GameRoom> repository,
            IHangmanRepositoryAsync<GameRoomPlayer> repositoryGameRoomPlayer, ILogger<GameRoomServiceAsync> logger)
        {
            _repositoryGameRoomPlayer = repositoryGameRoomPlayer;
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

        // join room (without joining, can't make moves!)
        public async Task<GameRoomPlayer> JoinRoom(GameRoom gameRoom, Player player, bool isHost = false)
        {
            var previousGameRoomPlayer = await _repositoryGameRoomPlayer.Get(
                grp => (grp.PlayerId == player.Id) && (grp.GameRoomId == gameRoom.Id));
            
            if (previousGameRoomPlayer != null)
            {
                _logger.LogInformation("Player had previously join this room...");
                previousGameRoomPlayer.IsInRoom = true;
                await _repositoryGameRoomPlayer.Update(previousGameRoomPlayer);

                return previousGameRoomPlayer;
            }

            _logger.LogInformation("First time player is joining this room...");
            var gameRoomPlayer = new GameRoomPlayer()
            {
                GameRoom = gameRoom,
                Player = player,
                IsHost = isHost,
                IsBanned = false,
                IsInRoom = true,
            };

            await _repositoryGameRoomPlayer.Save(gameRoomPlayer);
            return gameRoomPlayer;
        }
    }
}