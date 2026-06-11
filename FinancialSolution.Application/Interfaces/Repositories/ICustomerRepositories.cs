using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Application.Interfaces.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);

    Task<Customer?> GetByBvnAsync(string bvn);

    Task<Customer?> GetCustomerWithWalletAsync(Guid customerId);
}


//By adding : IGenericRepository<Customer>, you are combining interfaces.
//This means any class that implements ICustomerRepository doesn't just have to write the two methods listed here; it is forced to implement all 5 methods from the generic interface plus these 2 new ones.

//You get the standard CRUD operations for free, but strictly locked to the Customer type.

//These are your domain-specific queries.You are adding methods that only make sense for a Customer.

//Notice how these perfectly match the unique database rules we looked at in your CustomerConfiguration file?

//You made Email unique.

//You made BVN unique.

//In a financial application, these two methods are critical for Registration and Login.
//Before you create a new user account, your service layer will call _customerRepository.GetByEmailAsync(newEmail).If it returns a customer instead of null, your API knows to stop and return a "400 Bad Request: Email already in use" error.

//The Beauty of Clean Architecture
//Because you have separated your interfaces, your controllers only need to ask for ICustomerRepository.

//The controller gets access to:

//GetByIdAsync() (Inherited)

//GetAllAsync() (Inherited)

//AddAsync() (Inherited)

//Update() (Inherited)

//Delete() (Inherited)

//GetByEmailAsync() (Specialized)

//GetByBvnAsync() (Specialized)

//...all without needing to know how Entity Framework is actually getting that data out of the SQL