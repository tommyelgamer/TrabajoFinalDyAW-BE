using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalDyAW.DTOs
{
    public class BasicCredsDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
