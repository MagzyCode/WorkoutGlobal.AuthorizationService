using AutoMapper;
using WorkoutGlobal.AuthorizationServiceApi.Models;
using WorkoutGlobal.AuthorizationServiceApi.Models.Dto;

namespace WorkoutGlobal.AuthorizationServiceApi.AutoMapping
{
    /// <summary>
    /// Class for configure mapping rules via AutoMapper library.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Ctor for set mapping rules for models and DTOs.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<DefaultRegistrationInfoDto, UserCredential>();

            CreateMap<UserRegistrationDto, DefaultRegistrationInfoDto>();

            CreateMap<UserRegistrationDto, UserAccount>();
        }
    }
}
