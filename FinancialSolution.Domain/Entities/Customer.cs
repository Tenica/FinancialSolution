using FinancialSolution.Domain.Common;
using FinancialSolution.Domain.Enums;

namespace FinancialSolution.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string BVN { get; set; } = default!;

    public bool IsBvnVerified { get; set; }

    public DateTime? BvnVerifiedAt { get; set; }

    public bool IsActive { get; set; } = true;

    public UserRole Role { get; set; }

    public string PasswordHash { get; set; } = default!;



    // Navigation Properties
    public Profile? Profile { get; set; }

    public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();

    public ICollection<DeviceLog> DeviceLogs { get; set; } = new List<DeviceLog>();

    public ICollection<RefreshToken> RefreshTokens { get; set; }
    = new List<RefreshToken>();

    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
        = new List<PasswordResetToken>();
}