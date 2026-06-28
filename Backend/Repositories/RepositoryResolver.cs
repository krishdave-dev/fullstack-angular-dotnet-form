using Backend.Interfaces;
using Backend.Repositories.ADO;
using Backend.Repositories.EFCore;

namespace Backend.Repositories;

public class RepositoryResolver : IRepositoryResolver
{
    private readonly IServiceProvider _serviceProvider;

    public RepositoryResolver(
        IServiceProvider serviceProvider
    )
    {
        _serviceProvider = serviceProvider;
    }

    public ICustomerRepository GetRepository( HttpContext context)
    {
        var mode = context.Request.Headers["x-data-access"]
        .ToString()
        .ToLower();

        return mode switch
        {
            "ado" => _serviceProvider.GetRequiredService<CustomerAdoRepository>(),

            "ef" => _serviceProvider.GetRequiredService<CustomerEfRepository>(),

            _ => _serviceProvider.GetRequiredService<CustomerEfRepository>()
        };
    }
}