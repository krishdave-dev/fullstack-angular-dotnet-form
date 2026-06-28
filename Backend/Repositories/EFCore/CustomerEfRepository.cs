using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models.DTOs;
using Backend.Models.Entities;
using Backend.Validations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.EFCore;

public class CustomerEfRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerEfRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

   public async Task<
    PagedResponse<CustomerResponseDto>>
    GetAllAsync(
        CustomerQueryDto query)
{
    var customers =
        _context.Customers
            .Where(x => !x.IsDeleted);

    if(!string.IsNullOrWhiteSpace(
        query.Search))
    {
        customers =
            customers.Where(x =>
                x.FirstName.Contains(
                    query.Search)
                ||
                x.LastName.Contains(
                    query.Search)
                ||
                x.City.Contains(
                    query.Search));
    }

    var total =
        await customers.CountAsync();

    var data =
        await customers
            .Skip(
                (query.PageNumber-1)
                * query.PageSize)
            .Take(query.PageSize)
            .Select(x =>
                new CustomerResponseDto
                {
                    Id = x.Id,
                    FirstName =
                        x.FirstName,
                    LastName =
                        x.LastName,
                    Email =
                        x.Email,
                    DateOfBirth =
                        x.DateOfBirth,
                    City =
                        x.City
                })
            .ToListAsync();

    return new PagedResponse
        <CustomerResponseDto>
    {
        Data = data,
        PageNumber =
            query.PageNumber,
        PageSize =
            query.PageSize,
        TotalRecords =
            total
    };
}
    public async Task<CustomerResponseDto?>
        GetByIdAsync(int id)
    {

        var customer =
            await _context.Customers
                .Where(x =>
                    x.Id == id &&
                    !x.IsDeleted)
                .FirstOrDefaultAsync();

        if (customer == null)
            throw new NotFoundException("Customer not found");

        return new CustomerResponseDto
        {
            Id = customer.Id,
            FirstName =
                customer.FirstName,
            LastName =
                customer.LastName,
            Email =
                customer.Email,
            DateOfBirth =
                customer.DateOfBirth,
            City =
                customer.City
        };
    }

    public async Task<CustomerResponseDto>
    CreateAsync(
        CreateCustomerDto dto)
{
    CustomerValidator.Validate(
        dto);

    try
    {
        var customer =
            new Customer
            {
                FirstName =
                    dto.FirstName,
                LastName =
                    dto.LastName,
                Email =
                    dto.Email,
                DateOfBirth =
                    dto.DateOfBirth,
                City =
                    dto.City
            };

        _context.Customers
            .Add(customer);

        await _context
            .SaveChangesAsync();

        return
            (await GetByIdAsync(
                customer.Id))!;
    }
    catch(DbUpdateException ex)
    {
        throw new Exception(
            "Failed to create customer",
            ex);
    }
}
    public async Task<
    CustomerResponseDto?>
    UpdateAsync(
        int id,
        UpdateCustomerDto dto)
{
    CustomerValidator.Validate(
        dto);

    var customer =
        await _context.Customers
            .FirstOrDefaultAsync(
                x =>
                x.Id == id &&
                !x.IsDeleted);

    if(customer == null)
    {
        throw new NotFoundException(
            "Customer not found");
    }

    try
    {
        customer.FirstName =
            dto.FirstName;

        customer.LastName =
            dto.LastName;

        customer.Email =
            dto.Email;

        customer.DateOfBirth =
            dto.DateOfBirth;

        customer.City =
            dto.City;

        customer.UpdatedAt =
            DateTime.UtcNow;

        await _context
            .SaveChangesAsync();

        return await
            GetByIdAsync(id);
    }
    catch(DbUpdateException ex)
    {
        throw new Exception(
            "Failed to update customer",
            ex);
    }
}
   public async Task<bool>
    DeleteAsync(int id)
{
    var customer =
        await _context.Customers
            .FirstOrDefaultAsync(
                x =>
                x.Id == id &&
                !x.IsDeleted);

    if(customer == null)
    {
        throw new NotFoundException(
            "Customer not found");
    }

    try
    {
        customer.IsDeleted =
            true;

        customer.DeletedAt =
            DateTime.UtcNow;

        await _context
            .SaveChangesAsync();

        return true;
    }
    catch(DbUpdateException ex)
    {
        throw new Exception(
            "Failed to delete customer",
            ex);
    }
}
}