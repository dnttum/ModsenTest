namespace ModsenTestEvent.Application.Interfaces;

public interface IUpdateUseCase<TDto>
{
    Task ExecuteAsync(int id, TDto dto);
}