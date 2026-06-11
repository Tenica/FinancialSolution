using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialSolution.Infrastructure.Repositories;

public class CountryRepository
    : GenericRepository<Country>, ICountryRepository
{
    public CountryRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Country?> GetCountryWithCurrencyAsync(Guid countryId)
    {
        return await _context.Countries
            .Include(x => x.Currency)
            .FirstOrDefaultAsync(x => x.Id == countryId);
    }
}