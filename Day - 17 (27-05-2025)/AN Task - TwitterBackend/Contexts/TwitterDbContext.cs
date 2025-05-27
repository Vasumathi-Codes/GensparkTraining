using Microsoft.EntityFrameworkCore;
using TwitterBackend.Models;

namespace TwitterBackend.Contexts {
    public class TwitterDbContext : DbContext
    {
        public TwitterDbContext(DbContextOptions<TwitterDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Tweet> Tweets => Set<Tweet>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Hashtag> Hashtags => Set<Hashtag>();
        public DbSet<TweetHashtag> TweetHashtags => Set<TweetHashtag>();
        public DbSet<Follow> Follows => Set<Follow>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FolloweeId });

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.TweetId });

            modelBuilder.Entity<TweetHashtag>()
                .HasKey(th => new { th.TweetId, th.HashtagId });

            modelBuilder.Entity<TweetHashtag>()
                .HasOne(th => th.Tweet)
                .WithMany(t => t.TweetHashtags)
                .HasForeignKey(th => th.TweetId);

            modelBuilder.Entity<TweetHashtag>()
                .HasOne(th => th.Hashtag)
                .WithMany(h => h.TweetHashtags)
                .HasForeignKey(th => th.HashtagId);
        }
    }
}

