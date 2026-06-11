using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class DeviceLog : BaseEntity
{
    public Guid CustomerId { get; set; }

    public string DeviceName { get; set; } = default!;

    public string IPAddress { get; set; } = default!;

    public DateTime LoginDate { get; set; }

    // Navigation Property
    public Customer Customer { get; set; } = default!;
}