namespace ModsenTestEvent.Application.Interfaces;

public interface IGetRangeByEventIdUseCase
{
    Task<IEnumerable<ParticipantDto>> ExecuteAsync(int eventId);
}