using FinancialSolution.Application.DTOs.Wallet;
using FluentValidation;

namespace FinancialSolution.Application.Validators;

public class FundWalletRequestValidator
    : AbstractValidator<FundWalletRequest>
{
    public FundWalletRequestValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}