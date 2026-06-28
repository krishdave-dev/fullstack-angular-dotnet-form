using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models.DTOs;
using Backend.Validations;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Backend.Repositories.ADO;

public class CustomerAdoRepository
    : ICustomerRepository
{
    private readonly DbConnectionFactory
        _connectionFactory;

    public CustomerAdoRepository(
        DbConnectionFactory connectionFactory)
    {
        _connectionFactory =
            connectionFactory;
    }

    public async Task<PagedResponse<CustomerResponseDto>>
        GetAllAsync(CustomerQueryDto query)
    {
        try
        {
            var customers =
            new List<CustomerResponseDto>();

            using var con =
                _connectionFactory
                    .CreateConnection();

            using var cmd =
                new SqlCommand(
                    "sp_Customers_GetAll",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@PageNumber",
                query.PageNumber);

            cmd.Parameters.AddWithValue(
                "@PageSize",
                query.PageSize);

            cmd.Parameters.AddWithValue(
                "@Search",
                (object?)query.Search
                ?? DBNull.Value);

            await con.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                customers.Add(
                    new CustomerResponseDto
                    {
                        Id = reader.GetInt32(
                            "Id"),

                        FirstName =
                            reader.GetString(
                                "FirstName"),

                        LastName =
                            reader.GetString(
                                "LastName"),

                        Email =
                            reader.GetString(
                                "Email"),

                        DateOfBirth =
                            reader.GetDateTime(
                                "DateOfBirth"),

                        City =
                            reader.GetString(
                                "City")
                    });
            }

            return new PagedResponse
                <CustomerResponseDto>
            {
                Data = customers,
                PageNumber =
                    query.PageNumber,
                PageSize =
                    query.PageSize,
                TotalRecords =
                    customers.Count
            };
        }
        catch (SqlException ex)
        {
            throw new Exception("Database Error Occured.", ex);
        }

    }

    public async Task<CustomerResponseDto?>
    GetByIdAsync(int id)
    {
        try
        {
            using var con =
                _connectionFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "sp_Customers_GetById",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@Id",
                id);

            await con.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new CustomerResponseDto
                {
                    Id = reader.GetInt32("Id"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Email = reader.GetString("Email"),
                    DateOfBirth =
                        reader.GetDateTime(
                            "DateOfBirth"),
                    City =
                        reader.GetString(
                            "City")
                };
            }

            throw new NotFoundException(
                "Customer not found");
        }
        catch (SqlException ex)
        {
            throw new Exception(
                "Database error occurred.",
                ex);
        }
    }
    public async Task<CustomerResponseDto>
    CreateAsync(CreateCustomerDto dto)
    {
        try
        {
            CustomerValidator.Validate(dto);

            using var con =
                _connectionFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "sp_Customers_Insert",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@FirstName",
                dto.FirstName);

            cmd.Parameters.AddWithValue(
                "@LastName",
                dto.LastName);

            cmd.Parameters.AddWithValue(
                "@Email",
                dto.Email);

            cmd.Parameters.AddWithValue(
                "@DateOfBirth",
                dto.DateOfBirth);

            cmd.Parameters.AddWithValue(
                "@City",
                dto.City);

            await con.OpenAsync();

            var id =
                Convert.ToInt32(
                    await cmd.ExecuteScalarAsync());

            return
                (await GetByIdAsync(id))!;
        }
        catch (SqlException ex)
        {
            throw new Exception(
                "Failed to create customer.",
                ex);
        }
    }
    public async Task<CustomerResponseDto?>
    UpdateAsync(
        int id,
        UpdateCustomerDto dto)
    {
        try
        {
            CustomerValidator.Validate(dto);

            using var con =
                _connectionFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "sp_Customers_Update",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@Id",
                id);

            cmd.Parameters.AddWithValue(
                "@FirstName",
                dto.FirstName);

            cmd.Parameters.AddWithValue(
                "@LastName",
                dto.LastName);

            cmd.Parameters.AddWithValue(
                "@Email",
                dto.Email);

            cmd.Parameters.AddWithValue(
                "@DateOfBirth",
                dto.DateOfBirth);

            cmd.Parameters.AddWithValue(
                "@City",
                dto.City);

            await con.OpenAsync();

            var rows =
                await cmd.ExecuteNonQueryAsync();

            if (rows == 0)
            {
                throw new NotFoundException(
                    "Customer not found");
            }

            return await GetByIdAsync(id);
        }
        catch (SqlException ex)
        {
            throw new Exception(
                "Failed to update customer.",
                ex);
        }
    }

    public async Task<bool>
DeleteAsync(int id)
    {
        try
        {
            using var con =
                _connectionFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "sp_Customers_Delete",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@Id",
                id);

            await con.OpenAsync();

            var rows =
                await cmd.ExecuteNonQueryAsync();

            if (rows == 0)
            {
                throw new NotFoundException(
                    "Customer not found");
            }

            return true;
        }
        catch (SqlException ex)
        {
            throw new Exception(
                "Failed to delete customer.",
                ex);
        }
    }
}