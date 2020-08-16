using AutoMapper;
using Hangman.Models;

namespace Hangman.DTOs.Mappings
{
    public class Domain2DTO : Profile
    {
        public Domain2DTO()
        {
            CreateMap<GameRoomPlayer, PlayerInRoomDTO>();
        }
    }
}