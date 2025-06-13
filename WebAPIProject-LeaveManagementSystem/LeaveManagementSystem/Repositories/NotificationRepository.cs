using LeaveManagementSystem.Contexts;
using LeaveManagementSystem.Interfaces;
using LeaveManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Repositories
{
    public class NotificationRepository : Repository<Guid, Notification>, IRepository<Guid, Notification>
    {
        public NotificationRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public override async Task<Notification> Get(Guid key)
        {
            return await _applicationDbContext.Notifications.FirstOrDefaultAsync(n => n.Id == key);
        }

        public override async Task<IEnumerable<Notification>> GetAll()
        {
            return await _applicationDbContext.Notifications.ToListAsync();
        }
    }

}