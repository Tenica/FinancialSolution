using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Application.Interfaces.Repositories;

public interface ICountryRepository : IGenericRepository<Country>
{
    Task<Country?> GetCountryWithCurrencyAsync(Guid countryId);
}