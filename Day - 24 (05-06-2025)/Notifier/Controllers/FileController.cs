
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ocuNotify.Misc;
using ocuNotify.Models;
using ocuNotify.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;


[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
    private readonly IRepository<int, UploadedFile> _fileRepository;
    private readonly IHubContext<NotifyHub> _hubContext;

    public FileController(IRepository<int, UploadedFile> fileRepository, IHubContext<NotifyHub> hubContext)
    {
        _fileRepository = fileRepository;
        _hubContext = hubContext;

        if (!Directory.Exists(_uploadPath))
            Directory.CreateDirectory(_uploadPath);
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "HRAdmin")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        Console.WriteLine($"UPLOAD RAA VENNNAAAA");
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
        }

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(_uploadPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
        }


        // var uploadedBy = User.Identity?.Name ?? "Unknown";
       var uploadedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                        ?? User.Identity?.Name
                                        ?? "Unknown";

        var uploadedFile = new UploadedFile
        {
            FileName = file.FileName,
            FilePath = filePath,
            UploadedBy = uploadedBy,
            UploadedAt = DateTime.UtcNow
        };
        // Print the uploaded file object
        // Console.WriteLine("UploadedFile Object:");
        // Console.WriteLine($"FileName   : {uploadedFile.FileName}");
        // Console.WriteLine($"FilePath   : {uploadedFile.FilePath}");
        // Console.WriteLine($"UploadedBy : {uploadedFile.UploadedBy}");
        // Console.WriteLine($"UploadedAt : {uploadedFile.UploadedAt}");

        await _fileRepository.Add(uploadedFile);

        await _hubContext.Clients.All.SendAsync("NewFileUploaded", new
        {
            uploadedFile.FileName,
            uploadedFile.UploadedBy,
            uploadedFile.UploadedAt
        });
        return Ok(new { message = "File uploaded successfully." });
    }


    [HttpGet("{filename}")]
    [Authorize]
    public async Task<IActionResult> GetFile(string filename)
    {
        var file = Directory.GetFiles(_uploadPath)
                            .FirstOrDefault(f => Path.GetFileName(f).Contains(filename, System.StringComparison.OrdinalIgnoreCase));

        if (file == null)
            return NotFound("File not found.");

        var fileBytes = await System.IO.File.ReadAllBytesAsync(file);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(file));
    }
}
