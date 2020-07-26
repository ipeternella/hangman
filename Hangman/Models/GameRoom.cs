using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Hangman.Models
{
    public class GameRoom : BaseEntity
    {
        [Required] 
        [MaxLength(255)] 
        public string Name { get; set; } = null!;
        
        // fk to join table for many-to-many relationships to access Users
        public ICollection<GameRoomPlayer> GameRoomPlayers { get; set; } = null!;
        
        // one-to-many
        public ICollection<GuessWord> GuessWords { get; set; } = null!;
    }
    
    public class GuessWord : BaseEntity
    {
        [Required]
        public GameRoom GameRoom { get; set; } = null!;

        [Required] 
        public Guid GameRoomId { get; set; }
        
        [Required] 
        [MaxLength(255)] 
        public string Word { get; set; } = null!;

        public ICollection<GuessLetter> GuessLetters { get; set; } = null!;
    }
    
    public class GuessLetter : BaseEntity
    {
        [Required]
        public GuessWord GuessWord { get; set; } = null!;

        [Required]
        public Guid GuessWordId { get; set; }
        
        [Required] 
        [MaxLength(1)] 
        public string Letter { get; set; } = null!;
    }
    
    public class Player : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        // fk to join table for many-to-many relationships to access GameRooms
        // ignored on jsons: past of a user may bring too many results
        [JsonIgnore]
        public ICollection<GameRoomPlayer> GameRoomPlayers { get; set; } = null!;
    }
    
    public class GameRoomPlayer : BaseEntity
    {
        [Required]
        public Guid GameRoomId { get; set; }
        
        [Required]
        public GameRoom GameRoom { get; set; } = null!;

        [Required]
        public Player Player { get; set; } = null!;

        [Required]
        public Guid PlayerId { get; set; }
        
        [Required]
        public bool IsHost { get; set; }
        
        [Required]
        public bool IsBanned { get; set; }
        
        [Required]
        public bool IsInRoom { get; set; }
    }
}