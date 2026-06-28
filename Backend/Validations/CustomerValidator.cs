using Backend.Exceptions;
using Backend.Models.DTOs;

namespace Backend.Validations;

public static class CustomerValidator
{
    public static void Validate(CreateCustomerDto dto)
    {
        ValidateCommon(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.DateOfBirth,
            dto.City);
    }
    public static void Validate(
        UpdateCustomerDto dto)
    {
        ValidateCommon(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.DateOfBirth,
            dto.City);
    }
    private static void ValidateCommon( string FirstName,
        string LastName,
        string Email,
        DateTime Dob,
        string City)
    {
        var errors = new List<string>();
        //firstname
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            errors.Add("First Name is required");
        }
        if(FirstName?.Length < 2)
        {
            errors.Add(
                "First Name must be at least 2 characters");
        }
        //lastname
        if (string.IsNullOrWhiteSpace(LastName))
        {
            errors.Add("Last Name is required");
        }
        if(LastName?.Length < 2)
        {
            errors.Add(
                "Last Name must be at least 2 characters");
        }
        //email
        if (string.IsNullOrWhiteSpace(Email))
        {
            errors.Add("Email is required");
        }
        if(Email?.Length < 6)
        {
            errors.Add(
                "Enter proper Email Address");
        }
        //city
        if (string.IsNullOrWhiteSpace(City))
        {
            errors.Add("City is required");
        }
        //any error
        if(errors.Any())
        {
            throw new ValidationException(
                errors);
        }
        
    }
}