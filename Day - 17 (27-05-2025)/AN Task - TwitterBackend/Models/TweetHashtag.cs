using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBackend.Models {
    public class TweetHashtag
    {
        public int TweetId { get; set; }
        public int HashtagId { get; set; }

        [ForeignKey("TweetId")]
        public Tweet? Tweet { get; set; }

        [ForeignKey("HashtagId")]
        public Hashtag? Hashtag { get; set; }
    }
}