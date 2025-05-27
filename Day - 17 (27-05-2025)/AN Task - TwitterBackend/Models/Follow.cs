using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TwitterBackend.Models {
    public class Follow
    {
        public int FollowerId { get; set; }
        public int FolloweeId { get; set; }

        [ForeignKey("FollowerId")]
        public User? Follower { get; set; }
        [ForeignKey("FolloweeId")]
        public User? Followee { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.Now;
    }

}