using FinancialSolution.Domain.Entities;

public interface IPasswordResetTokenRepository
{
    Task AddAsync(
        PasswordResetToken token);

    Task<PasswordResetToken?>
        GetByTokenAsync(string token);
}