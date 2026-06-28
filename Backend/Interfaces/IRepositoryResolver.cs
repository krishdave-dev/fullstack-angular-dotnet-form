namespace Backend.Interfaces;

public interface IRepositoryResolver
{
    ICustomerRepository GetRepository(
        HttpContext context
    );
}