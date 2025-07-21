using TrainingVideoAPI.Interfaces;
using TrainingVideoAPI.Models;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace TrainingVideoAPI.Services
{
    public class TrainingVideoService : ITrainingVideoService
    {
        private readonly IRepository<int, TrainingVideo> _repository;
        private readonly IConfiguration _configuration;

        public TrainingVideoService(IRepository<int, TrainingVideo> repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<TrainingVideo>> GetAllVideos()
        {
            return await _repository.GetAll();
        }

        public async Task<TrainingVideo?> GetVideoById(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<TrainingVideo> UploadVideoAsync(IFormFile file, string title, string description)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            // 1. Read Azure config
            var connectionString = _configuration["AzureBlobStorage:ConnectionString"];
            var containerName = _configuration["AzureBlobStorage:ContainerName"];

            // 2. Create container client
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            // 3. Upload file
            var blobClient = containerClient.GetBlobClient(file.FileName);
            await using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            var blobUrl = blobClient.Uri.ToString();

            // 4. Save video metadata to DB
            var video = new TrainingVideo
            {
                Title = title,
                Description = description,
                UploadDate = DateTime.UtcNow,
                BlobUrl = blobUrl
            };

            return await _repository.Add(video);
        }

        public async Task<TrainingVideo> AddVideo(TrainingVideo video)
        {
            return await _repository.Add(video);
        }

        public async Task<TrainingVideo> UpdateVideo(int id, TrainingVideo video)
        {
            return await _repository.Update(id, video);
        }

        public async Task<TrainingVideo> DeleteVideo(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
