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

            RuleFor(dto => dto.PlayerId).NotEmpty()
            .MustAsync(async (playerId, cancellation) =>
            {
                var player = await playerService.GetById(playerId);
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

                if (gameRoomPlayerData == null || !gameRoomPlayerData.IsInRoom) return false;
                return true;
            }).WithMessage("Player is not the room.");
        }
    }

    public class GuessWordCreationHostValidation : AbstractValidator<GuessWordDTO>
    {
        private readonly IGameRoomServiceAsync _gameRoomService;
        private readonly IPlayerServiceAsync _playerService;

        public GuessWordCreationHostValidation(IGameRoomServiceAsync gameRoomService,
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

                if (gameRoomPlayerData == null || !gameRoomPlayerData.IsInRoom) return false;

                // TODO: activate so only hosts can create guess words
                // if (!gameRoomPlayerData.IsHost) return false;

                return true;
            }).WithMessage("Player is not the room.");
        }
    }
}