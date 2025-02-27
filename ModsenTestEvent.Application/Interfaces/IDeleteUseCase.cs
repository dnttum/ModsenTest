namespace ModsenTestEvent.Application.Interfaces;

public interface IDeleteUseCase
{
    Task ExecuteAsync(int id);
}