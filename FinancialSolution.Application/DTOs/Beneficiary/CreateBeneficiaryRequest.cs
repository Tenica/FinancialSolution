namespace FinancialSolution.Application.DTOs.Beneficiary;

public class CreateBeneficiaryRequest
{
    public string BeneficiaryName { get; set; } = default!;

    public string AccountNumber { get; set; } = default!;

    public string BankName { get; set; } = default!;
}