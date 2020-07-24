using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hangman.Application
{
    public interface IGameRoomServiceAsync
    {
        public Task<GameRoom> GetById(Guid id);
        public Task<IEnumerable<GameRoom>> GetAll();
        public Task<GameRoom> Create(NewGameRoomData newGameRoomData);
        public Task<GameRoomPlayer> JoinRoom(GameRoom gameRoom, Player player);
    }
}