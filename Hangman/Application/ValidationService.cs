using FluentValidation;
using Hangman.DTOs;
using Hangman.Models;
using Hangman.Repository.Interfaces;
using System.Linq;

namespace Hangman.Application
{
    public class GameRoomPlayerValidator : AbstractValidator<JoinRoomDTO>
    {
        private readonly IGameRoomServiceAsync _gameRoomService;
        private readonly IPlayerServiceAsync _playerService;

        public GameRoomPlayerValidator(IGameRoomServiceAsync gameRoomService,
            IPlayerServiceAsync playerService)
        {
            _gameRoomService = gameRoomService;
            _playerService = playerService;

            RuleFor(dto => dto.GameRoomId)
            .MustAsync(async (gameRoomId, cancellation) =>
            {
                var gameRoom = await _gameRoomService.GetById(gameRoomId);
                return gameRoom != null;
            }).WithMessage("Game room was not found.");

            RuleFor(dto => dto.PlayerName).NotEmpty()
            .MustAsync(async (playerName, cancellation) =>
            {
                var player = await playerService.GetByPlayerName(playerName);
                return player != null;
            }).WithMessage("Player was not found.");
        }
    }
}