using Microsoft.EntityFrameworkCore;
using TrainingVideoAPI.Models;

namespace TrainingVideoAPI.Data;

public class VideoDbContext : DbContext
{
    public VideoDbContext(DbContextOptions<VideoDbContext> options) : base(options) { }

    public DbSet<TrainingVideo> TrainingVideos => Set<TrainingVideo>();
}
