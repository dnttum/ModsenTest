namespace ModsenTestEvent.Application.Interfaces;

public interface IGetAllUseCase<TDto>
{
    Task<IEnumerable<TDto>> ExecuteAsync(PageParamsDto pageParamsDto, CancellationToken cancellationToken);
}