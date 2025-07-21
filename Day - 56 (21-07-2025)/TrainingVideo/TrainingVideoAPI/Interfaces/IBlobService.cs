
namespace TrainingVideoAPI.Interfaces {
    public interface IBlobService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }

}