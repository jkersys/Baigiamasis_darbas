namespace UTP_Web_API.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<LocalUser> LocalUsers { get; set; }
    }
}
