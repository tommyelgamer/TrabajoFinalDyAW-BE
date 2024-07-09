namespace TrabajoFinalDyAW.Presenters
{
    public class UserPresenter
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
