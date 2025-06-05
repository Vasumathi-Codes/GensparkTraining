using Microsoft.EntityFrameworkCore;
using ocuNotify.Models;

namespace ocuNotify.Context
{
    public class NotifyContext : DbContext
    {
        public NotifyContext(DbContextOptions<NotifyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }

    }
}
