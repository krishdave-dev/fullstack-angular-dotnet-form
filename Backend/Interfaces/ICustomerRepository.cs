using Backend.Models.DTOs;
namespace Backend.Interfaces;
public interface ICustomerRepository
{
    Task<PagedResponse<CustomerResponseDto>> GetAllAsync(CustomerQueryDto query);
    Task<CustomerResponseDto?> GetByIdAsync(int id);

    Task<CustomerResponseDto> CreateAsync(
        CreateCustomerDto dto
    );
    Task<CustomerResponseDto?> UpdateAsync(
        int id,
        UpdateCustomerDto dto);

    Task<bool> DeleteAsync(int id);

}