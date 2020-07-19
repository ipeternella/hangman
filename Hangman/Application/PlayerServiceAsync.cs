using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hangman.Models;
using Hangman.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hangman.Application
{
    public class NewPlayerData
    {
        [Required]
        public string Name { get; set; }
    }

    public class PlayerServiceAsync : IPlayerServiceAsync
    {
        private readonly IHangmanRepositoryAsync<Player> _repository;
        private readonly ILogger<PlayerServiceAsync> _logger;
        
        public PlayerServiceAsync(IHangmanRepositoryAsync<Player> repository, ILogger<PlayerServiceAsync> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        public async Task<Player> GetById(Guid id)
        {
            var player = await _repository.GetById(id);
            
            return player;
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            var players = await _repository.All();
            
            return players;
        }
        
        public async Task<Player> Create(NewPlayerData newPlayerData)
        {
            var newPlayer = new Player {Name = newPlayerData.Name};
            await _repository.Save(newPlayer);

            return newPlayer;
        }
    }
}