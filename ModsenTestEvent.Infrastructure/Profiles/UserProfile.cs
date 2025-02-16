namespace ModsenTestEvent.Infrastructure.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegistrationRequestDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<User, UserDto>();
    }
}