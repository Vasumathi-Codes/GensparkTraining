using FirstAPI.Contexts;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Repositories
{
    public  class DoctorRepository : Repository<int, Doctor>
    {
        public DoctorRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Doctor> Get(int key)
        {
            var doctor = await _clinicContext.Doctors.SingleOrDefaultAsync(p => p.Id == key);

            return doctor??throw new Exception("No Doctor with the given ID");
        }

        public override async Task<IEnumerable<Doctor>> GetAll()
        {
            var doctors = await _clinicContext.Doctors.ToListAsync();
            if (doctors.Count == 0)
                throw new Exception("No Doctors in the database");
            return doctors;
        }
    }
}

