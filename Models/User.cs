using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class User
    {
        public int Id { get; set; }
        
        
        [Required]
        public string Username { get; set; } 

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string Role { get; set; }   // Admin hoặc User
    }
}