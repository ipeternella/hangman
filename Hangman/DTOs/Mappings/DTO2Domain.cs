using AutoMapper;
using Hangman.Models;

namespace Hangman.DTOs.Mappings
{
    public class DTO2Domain : Profile
    {
        public DTO2Domain()
        {
            CreateMap<GameRoomDTO, GameRoom>();
        }
    }
}