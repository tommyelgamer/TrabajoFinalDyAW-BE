using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalDyAW.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(255, MinimumLength = 1)]
        public string Username { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }
        public IEnumerable<string> Permissions { get; set; } = Enumerable.Empty<string>();
    }
}
