using ocuNotify.Context;
using ocuNotify.Interfaces;
using ocuNotify.Models;
using Microsoft.EntityFrameworkCore;

namespace ocuNotify.Repositories
{
    public class UploadedFileRepository : Repository<int, UploadedFile>
    {
         
        private readonly NotifyContext _context;

        public UploadedFileRepository(NotifyContext notifyContext): base(notifyContext) 
        {
        }

        public override async Task<IEnumerable<UploadedFile>> GetAll()
        {
            return await _context.UploadedFiles.ToListAsync();
        }

        public override async Task<UploadedFile> Get(int id)
        {
            return await _context.UploadedFiles.FindAsync(id);
        }

    }
}