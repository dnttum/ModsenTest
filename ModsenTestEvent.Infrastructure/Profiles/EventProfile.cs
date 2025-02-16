namespace ModsenTestEvent.Infrastructure.Profiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventDto>(); 
        
        CreateMap<EventDto, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); 
    }
}