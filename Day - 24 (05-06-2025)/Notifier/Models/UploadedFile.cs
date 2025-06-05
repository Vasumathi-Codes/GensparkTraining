using System;

namespace ocuNotify.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }                      
        public string FileName { get; set; }                
        public string FilePath { get; set; }             
        public string UploadedBy { get; set; }              
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow; 
    }
}
