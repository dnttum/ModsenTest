namespace ModsenTestEvent.Application.Interfaces;

public interface IRegisterUseCase<in TInDto, TOutDto>
{
    Task<TOutDto> ExecuteAsync(TInDto dto, CancellationToken cancellationToken = default);
}