using System.ComponentModel.DataAnnotations;

namespace Hangman.DTOs
{
    public class GuessLetterDTO
    {
        [Required]
        public string GuessLetter { get; set; } = default!; // null-forgiving as this property is required
        [Required]
        public string PlayerName { get; set; } = default!; // null-forgiving as this property is required
    }
}