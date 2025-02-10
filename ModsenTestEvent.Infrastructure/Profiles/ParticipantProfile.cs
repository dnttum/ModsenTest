namespace ModsenTestEvent.Infrastructure.Profiles;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<Participant, ParticipantDto>();
        
        CreateMap<ParticipantDto, Participant>();
    }
}