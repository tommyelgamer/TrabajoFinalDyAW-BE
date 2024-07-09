namespace TrabajoFinalDyAW.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
