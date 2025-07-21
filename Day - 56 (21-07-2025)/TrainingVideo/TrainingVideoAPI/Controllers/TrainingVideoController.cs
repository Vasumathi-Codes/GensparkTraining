using Microsoft.AspNetCore.Mvc;
using TrainingVideoAPI.Interfaces;
using TrainingVideoAPI.Models;

namespace TrainingVideoAPI.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class TrainingVideoController : ControllerBase
    {
        private readonly ITrainingVideoService _service;

        public TrainingVideoController(ITrainingVideoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingVideo>>> GetAll()
        {
            var videos = await _service.GetAllVideos();
            return Ok(videos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrainingVideo>> GetById(int id)
        {
            var video = await _service.GetVideoById(id);
            if (video == null) return NotFound();
            return Ok(video);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo([FromForm] VideoUploadRequest request)
        {
            
            var file = request.File;
            var title = request.Title;
            var description = request.Description;
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

           try {
            var createdVideo = await _service.UploadVideoAsync(file, title, description);
            return CreatedAtAction(nameof(GetById), new {id = createdVideo.Id}, createdVideo);

           } catch (Exception ex){
            return StatusCode(500, $"internal server err : {ex.Message}");
           }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TrainingVideo>> Update(int id, [FromBody] TrainingVideo video)
        {
            try
            {
                var updated = await _service.UpdateVideo(id, video);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<TrainingVideo>> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteVideo(id);
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
