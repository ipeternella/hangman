using System.ComponentModel.DataAnnotations;

namespace Hangman.DTOs
{
    public class GuessWordDTO
    {
        [Required]
        public string GuessWord { get; set; } = default!; // null-forgiving as this property is required
        [Required]
        public string PlayerName { get; set; } = default!; // null-forgiving as this property is required
    }
}