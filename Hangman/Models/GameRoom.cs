using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hangman.Models
{
    public class GameRoom : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        
        // fk to join table for many-to-many relationships to access Users
        public ICollection<GameRoomPlayer> GameRoomPlayers { get; set; }
    }
    
    public class Player : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        
        // fk to join table for many-to-many relationships to access GameRooms
        public ICollection<GameRoomPlayer> GameRoomPlayers { get; set; }
    }
    
    public class GameRoomPlayer : BaseEntity
    {
        [Required]
        public Guid GameRoomId { get; set; }
        
        [Required]
        public GameRoom GameRoom { get; set; }
        
        [Required]
        public Player Player { get; set; }
        
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