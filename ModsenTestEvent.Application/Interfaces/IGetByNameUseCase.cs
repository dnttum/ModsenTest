namespace ModsenTestEvent.Application.Interfaces;

public interface IGetByNameUseCase<TDto>
{
    Task<TDto> ExecuteAsync(string name);
}