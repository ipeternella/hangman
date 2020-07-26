using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Models;

namespace Hangman.Application
{
    public interface IPlayerServiceAsync
    {
        public Task<Player?> GetById(Guid id);
        public Task<IEnumerable<Player>> GetAll();
        public Task<Player> Create(NewPlayerData newPlayerData);
        public Task<Player?> GetByPlayerName(string playerName);
    }
}