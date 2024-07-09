namespace TrabajoFinalDyAW.DTOs
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public IEnumerable<string>? Permissions { get; set; }
    }
}
