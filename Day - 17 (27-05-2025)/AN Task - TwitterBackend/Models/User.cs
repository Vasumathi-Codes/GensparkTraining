namespace TwitterBackend.Models {
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Tweet>? Tweets { get; set; }
        public ICollection<Follow>? Followers { get; set; }
        public ICollection<Follow>? Following { get; set; }
    }

}