using FinancialSolution.Application.Interfaces.Services;

namespace FinancialSolution.Application.Services;

public class AccountNumberGenerator : IAccountNumberGenerator
{
    public string Generate(string currencyCode)
    {
        var random = Random.Shared.Next(10000000, 99999999);

        return $"{currencyCode}{random}";
    }
}