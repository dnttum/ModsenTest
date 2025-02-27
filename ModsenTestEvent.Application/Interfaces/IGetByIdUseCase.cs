namespace ModsenTestEvent.Application.Interfaces;

public interface IGetByIdUseCase<TDto>
{
    Task<TDto?> ExecuteAsync(int id);
}