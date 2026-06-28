using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IRepositoryResolver _resolver;

    public CustomerController(
        IRepositoryResolver resolver)
    {
        _resolver = resolver;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetAll(
            [FromQuery]
            CustomerQueryDto query)
    {
        var repo =
            _resolver.GetRepository(
                HttpContext);

        var customers =
            await repo.GetAllAsync(
                query);

        return Ok(
            new ApiResponse<
                PagedResponse<
                    CustomerResponseDto>>
            {
                StatusCode = 200,
                Success = true,
                Message =
                    "Customers fetched successfully",
                Data = customers
            });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetById(int id)
    {
        var repo =
            _resolver.GetRepository(
                HttpContext);

        var customer =
            await repo.GetByIdAsync(id);

        if(customer == null)
        {
            throw new NotFoundException(
                "Customer not found");
        }

        return Ok(
            new ApiResponse<
                CustomerResponseDto>
            {
                StatusCode = 200,
                Success = true,
                Message =
                    "Customer fetched successfully",
                Data = customer
            });
    } 

    [HttpPost]
    public async Task<IActionResult>
        Create(
            [FromBody]
            CreateCustomerDto dto)
    {
        var repo =
            _resolver.GetRepository(
                HttpContext);

        var customer =
            await repo.CreateAsync(
                dto);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<
                CustomerResponseDto>
            {
                StatusCode = 201,
                Success = true,
                Message =
                    "Customer created successfully",
                Data = customer
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult>
        Update(
            int id,
            [FromBody]
            UpdateCustomerDto dto)
    {
        var repo =
            _resolver.GetRepository(
                HttpContext);

        var customer =
            await repo.UpdateAsync(
                id,
                dto);

        if(customer == null)
        {
            throw new NotFoundException(
                "Customer not found");
        }

        return Ok(
            new ApiResponse<
                CustomerResponseDto>
            {
                StatusCode = 200,
                Success = true,
                Message =
                    "Customer updated successfully",
                Data = customer
            });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        Delete(int id)
    {
        var repo =
            _resolver.GetRepository(
                HttpContext);

        var deleted =
            await repo.DeleteAsync(
                id);

        if(!deleted)
        {
            throw new NotFoundException(
                "Customer not found");
        }

        return Ok(
            new ApiResponse<object>
            {
                StatusCode = 200,
                Success = true,
                Message =
                    "Customer deleted successfully",
                Data = null
            });
    }
}