using Microsoft.EntityFrameworkCore;
using TrainingVideoAPI.Data;
using TrainingVideoAPI.Interfaces;
using TrainingVideoAPI.Models;

namespace TrainingVideoAPI.Repositories
{
    public class TrainingVideoRepository : Repository<int, TrainingVideo>
    {
        public TrainingVideoRepository(VideoDbContext context) : base(context)
        {
        }

        public override async Task<TrainingVideo> Get(int id)
        {
            return await _applicationDbContext.TrainingVideos.FirstOrDefaultAsync(v => v.Id == id);
        }

        public override async Task<IEnumerable<TrainingVideo>> GetAll()
        {
            return await _applicationDbContext.TrainingVideos.ToListAsync();
        }
    }
}
