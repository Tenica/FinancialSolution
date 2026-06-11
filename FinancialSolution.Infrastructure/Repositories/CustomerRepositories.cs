using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialSolution.Infrastructure.Repositories;

public class CustomerRepository
    : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context)
        : base(context)
    { 
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Customer?> GetByBvnAsync(string bvn)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.BVN == bvn);
    }

    public async Task<Customer?> GetCustomerWithWalletAsync(Guid customerId)
    {
        return await _context.Customers
            .Include(x => x.Wallets)
                .ThenInclude(x => x.Currency)
            .FirstOrDefaultAsync(x => x.Id == customerId);
    }
}

//The : base(context) syntax is you acting as a middleman.You are catching the package and instantly throwing it up to the parent.

//Here is the exact translation of what the code is doing in plain English:

//public CustomerRepository(ApplicationDbContext context)

//The.NET System: "Hey CustomerRepository, I am creating you now. Here is the Database Manager (context) you asked for."

//: base(context)

//CustomerRepository: "Thanks! But I inherit from a parent class (GenericRepository). Before I can finish building myself, I need to build my parent. Hey Parent, catch! Here is the context you demanded."

//{ } (The curly braces)

//CustomerRepository: "Now that my parent is fully built and happy, I can do my own specific setup inside these curly braces. (In this case, I have nothing extra to do, so I'll leave them empty)."

//Summary
//base is a C# keyword that literally means "My Parent Class."

//When you put it next to a constructor, you are just funneling variables upward so your parent class has the data it needs to initialize itself properly.