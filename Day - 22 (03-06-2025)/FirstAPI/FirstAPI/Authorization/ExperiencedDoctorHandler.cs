using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Repositories;

namespace FirstAPI.Authorization {
    public class ExperiencedDoctorHandler : AuthorizationHandler<ExperiencedDoctorRequirement>
    {
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IDoctorService _doctorService;

        public ExperiencedDoctorHandler(IRepository<int, Doctor> doctorRepository,
                                        IDoctorService doctorService)
        {
            _doctorRepository = doctorRepository;
            _doctorService = doctorService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ExperiencedDoctorRequirement requirement)
        {
            var usernameClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (usernameClaim == null)
                return;

            string username = usernameClaim.Value;

            var doctor = await _doctorService.GetDoctorByEmail(username); 
            if (doctor == null)
                return;

            var experience = doctor.YearsOfExperience;

            if (experience >= requirement.MinimumExperienceYears)
            {
                context.Succeed(requirement);
            }
        }

    }

}