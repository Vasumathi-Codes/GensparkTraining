using TrainingVideoAPI.Models;

namespace TrainingVideoAPI.Interfaces
{
    public interface ITrainingVideoService
    {
        Task<IEnumerable<TrainingVideo>> GetAllVideos();
        Task<TrainingVideo?> GetVideoById(int id);
        Task<TrainingVideo> AddVideo(TrainingVideo video);
        Task<TrainingVideo> UpdateVideo(int id, TrainingVideo video);
        Task<TrainingVideo> DeleteVideo(int id);
        Task<TrainingVideo> UploadVideoAsync(IFormFile file, string title, string description);

    }
}
