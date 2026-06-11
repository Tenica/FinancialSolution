using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(
        RefreshToken refreshToken);

    Task<RefreshToken?>
        GetByTokenAsync(string token);

    Task<List<RefreshToken>> GetByCustomerIdAsync(
    Guid customerId);
}