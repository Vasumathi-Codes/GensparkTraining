using FirstAPI.Contexts;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Repositories
{
    public class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
    {
        public DoctorSpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<DoctorSpeciality> Get(int key)
        {
            var doctorSpeciality = await _clinicContext.DoctorSpecialities
                .SingleOrDefaultAsync(p => p.SerialNumber == key);
            return doctorSpeciality ?? throw new Exception("No doctorSpeciality with the given SerialNumber");
        }

        public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
        {
            var doctorSpecialities = await _clinicContext.DoctorSpecialities.ToListAsync();
            if (doctorSpecialities.Count == 0)
                throw new Exception("No DoctorSpecialities in the database");
            return doctorSpecialities;
        }
    }
}
