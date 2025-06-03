using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Authorization {
    public class ExperiencedDoctorRequirement : IAuthorizationRequirement
    {
        public int MinimumExperienceYears { get; }

        public ExperiencedDoctorRequirement(int years)
        {
            MinimumExperienceYears = years;
        }
    }
}
