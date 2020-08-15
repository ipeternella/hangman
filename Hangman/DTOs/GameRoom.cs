using System.ComponentModel.DataAnnotations;

namespace Hangman.DTOs
{
    public class GameRoomDTO
    {
        [Required]
        public string Name { get; set; } = default!;
    }

    public class JoinRoomDTO
    {
        [Required]
        public string PlayerName { get; set; } = default!; // null-forgiving as this property is required
    }
}