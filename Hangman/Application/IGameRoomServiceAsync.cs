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
        public Task<GuessWordResponseDTO> CreateGuessWord(GuessWordDTO guessWordDTO);
        public Task<PlayerInRoomDTO> JoinRoom(JoinRoomDTO joinRoomDTO);
        public Task<PlayerInRoomDTO> LeaveRoom(LeaveRoomDTO leaveRoomDTO);
        public Task<GameRoomPlayer?> GetPlayerRoomData(Guid gameRoomId, Guid playerId);
        public Task<GuessLetter> CreateGuessLetter(GuessWord guessWord, string guessLetter);
        public Task<GameStateData> UpdateGameRoundState(GuessWord guessWord, string guessLetterString);
        public IEnumerable<string> GetGuessWordStateSoFar(GuessWord guessWord);
    }
}