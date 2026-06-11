using FinancialSolution.Application.DTOs.Customer;

namespace FinancialSolution.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<CustomerResponse> RegisterCustomerAsync(RegisterCustomerRequest request);

    Task<CustomerResponse?> GetCustomerByEmailAsync(string email);
}

//What it does: This is the contract for your Business Logic.This is the brain of your application.

//It deals in DTOs(Data Transfer Objects) : Notice that it does not accept or return a raw Customer.It takes in a RegisterCustomerRequest and returns a CustomerResponse.This is a massive security win. It means sensitive database columns (like PasswordHash) can never accidentally be sent to the user.

//It Enforces Rules: The implementation of this service is where you hash the password, check if the email already exists, and generate the account number.

//The "Manager": This is the restaurant manager.The manager takes the customer's order (the Request), tells the waiter to go get ingredients from the kitchen (calls the Repository), cooks the meal (applies business logic), and hands a finished, safe plate to the customer (the Response).

//The Flow (Why you need both)
//If you didn't separate these, your API controller would be doing everything, which leads to messy, unreadable, and untestable code. Because you created these two distinct interfaces, your application now flows like a beautifully organized assembly line:

//The Phone/Web App sends a JSON request.

//The API Controller receives it. It has no business logic and no database code.It simply hands the request to ICustomerService.

//The ICustomerService runs the business rules (like hashing the password). When it needs to save or fetch data, it asks ICustomerRepository.

//The ICustomerRepository talks to SQL Server using Entity Framework.

//This is textbook, enterprise-grade architecture.