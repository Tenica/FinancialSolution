using FinancialSolution.Application.DTOs.Transaction;
using FluentValidation;

namespace FinancialSolution.Application.Validators;

public class TransferRequestValidator
    : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.SenderAccountNumber)
            .NotEmpty();

        RuleFor(x => x.ReceiverAccountNumber)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}