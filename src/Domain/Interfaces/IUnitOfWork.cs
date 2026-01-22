namespace UserCrud.Domain.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}