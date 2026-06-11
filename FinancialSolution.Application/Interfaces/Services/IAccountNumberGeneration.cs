namespace FinancialSolution.Application.Interfaces.Services;

public interface IAccountNumberGenerator
{
    string Generate(string currencyCode);
}