using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hangman.Application
{
    public interface IGameRoomServiceAsync
    {
        public Task<GameRoom?> GetById(Guid id);
        public Task<IEnumerable<GameRoom>> GetAll();
        public Task<IEnumerable<GuessWord>> GetAllGuessedWords(Guid gameRoomId);
        public Task<GameRoom> Create(NewGameRoomData newGameRoomData);
        public Task<GuessWord> CreateGuessWord(GameRoom gameRoom, string guessWord);
        public Task<GameRoomPlayer> JoinRoom(GameRoom gameRoom, Player player, bool isHost = false);
        public Task<GameRoomPlayer> LeaveRoom(GameRoomPlayer gameRoomPlayer);
        public Task<GameRoomPlayer?> GetPlayerRoomData(GameRoom gameRoom, Player player);
        public Task<GuessLetter> CreateGuessLetter(GameRound gameRound, string guessLetter);
        // public Task<GuessLetter> UpdateGuessWordRoundState(GuessLetter guessLetter);
    }
}