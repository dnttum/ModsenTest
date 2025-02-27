namespace ModsenTestEvent.Application.Interfaces;

public interface ICreateUseCase<TDto>
{
    Task<TDto?> ExecuteAsync(TDto dto);
}