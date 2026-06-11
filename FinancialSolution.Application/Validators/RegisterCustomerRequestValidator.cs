using FinancialSolution.Application.DTOs.Customer;
using FluentValidation;

namespace FinancialSolution.Application.Validators;

public class RegisterCustomerRequestValidator
    : AbstractValidator<RegisterCustomerRequest>
{
    public RegisterCustomerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();

        RuleFor(x => x.BVN)
            .NotEmpty()
            .Length(11);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.CountryId)
            .NotEmpty();
    }
}