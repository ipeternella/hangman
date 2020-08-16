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

    public class PlayerPreviouslyInRoomValidator : AbstractValidator<LeaveRoomDTO>
    {
        private readonly IGameRoomServiceAsync _gameRoomService;
        private readonly IPlayerServiceAsync _playerService;

        public PlayerPreviouslyInRoomValidator(IGameRoomServiceAsync gameRoomService,
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

            RuleFor(dto => dto.PlayerId).NotEmpty()
            .MustAsync(async (dto, playerId, cancellation) =>
            {
                var player = await playerService.GetById(playerId);
                return player != null;
            }).WithMessage("Player was not found.");

            RuleFor(dto => dto.GameRoomId).NotEmpty()
            .MustAsync(async (dto, gameRoomId, cancellation) =>
            {
                var gameRoomPlayerData = await gameRoomService.GetPlayerRoomData(dto.GameRoomId, dto.PlayerId);

                if (gameRoomPlayerData == null) return gameRoomPlayerData != null;  // can't be null
                return gameRoomPlayerData.IsInRoom;
            }).WithMessage("Player is not the room.");
        }
    }
}