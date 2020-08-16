using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.DTOs;
using Hangman.Models;

namespace Hangman.Application
{
    public interface IGameRoomServiceAsync
    {
        public Task<GameRoom?> GetById(Guid id);
        public Task<IEnumerable<GameRoom>> GetAll();
        public Task<IEnumerable<GuessWord>> GetAllGuessedWords(Guid gameRoomId);
        public Task<GuessWord?> GetGuessedWord(Guid guessWordId);
        public Task<GameRoom> Create(GameRoomDTO gameRoomDTO);
        public Task<GuessWord> CreateGuessWord(GameRoom gameRoom, string guessWord);
        public Task<PlayerInRoomDTO> JoinRoom(JoinRoomDTO joinRoomDTO);
        public Task<GameRoomPlayer> LeaveRoom(GameRoomPlayer gameRoomPlayer);
        public Task<GameRoomPlayer?> GetPlayerRoomData(GameRoom gameRoom, Player player);
        public Task<GuessLetter> CreateGuessLetter(GuessWord guessWord, string guessLetter);
        public Task<GameStateData> UpdateGameRoundState(GuessWord guessWord, string guessLetterString);
        public IEnumerable<string> GetGuessWordStateSoFar(GuessWord guessWord);
    }
}