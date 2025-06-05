using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ocuNotify.Misc
{
    public class NotifyHub : Hub
    {
        public async Task SendFileUploadedNotification(string fileName, string uploadedBy, DateTime uploadedAt)
        {
            await Clients.All.SendAsync("NewFileUploaded", new
            {
                FileName = fileName,
                UploadedBy = uploadedBy,
                UploadedAt = uploadedAt
            });
        }
    }
}

