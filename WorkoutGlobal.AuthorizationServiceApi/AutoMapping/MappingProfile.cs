using AutoMapper;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Models;

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

            CreateMap<UserCredentialDto, UserCredential>().ReverseMap();

            CreateMap<UserAccount, UserAccountDto>().ReverseMap();

            CreateMap<UpdationUserAccountDto, UserAccountDto>();
        }
    }
}
