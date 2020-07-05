using System;
using System.ComponentModel.DataAnnotations;

namespace Hangman.Models
{
    public class GameRoom : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}