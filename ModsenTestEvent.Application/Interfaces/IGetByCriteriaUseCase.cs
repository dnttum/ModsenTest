namespace ModsenTestEvent.Application.Interfaces;

public interface IGetByCriteriaUseCase<TDto>
{
    Task<IEnumerable<TDto>> ExecuteAsync(DateTime? date, string? location, string? category);
}