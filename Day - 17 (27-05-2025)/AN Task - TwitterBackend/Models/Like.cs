using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBackend.Models {
    public class Like
    {
        public int UserId { get; set; }
        public int TweetId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("TweetId")]
        public Tweet? Tweet { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }

}