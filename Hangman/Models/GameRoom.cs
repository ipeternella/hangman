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
        public ICollection<GameRoomUser> GameRoomUsers { get; set; }
    }
    
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        
        // fk to join table for many-to-many relationships to access GameRooms
        public ICollection<GameRoomUser> GameRoomUsers { get; set; }
    }
    
    public class GameRoomUser : BaseEntity
    {
        [Required]
        public Guid GameRoomId { get; set; }
        
        [Required]
        public GameRoom GameRoom { get; set; }
        
        [Required]
        public User User { get; set; }
        
        [Required]
        public User UserId { get; set; }
        
        [Required]
        public bool IsHost { get; set; }
        
        [Required]
        public bool IsBanned { get; set; }
    } 
}