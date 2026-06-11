public interface IAuditService
{
    Task LogAsync(
        Guid customerId,
        string action,
        string description,
        string? ipAddress);
}