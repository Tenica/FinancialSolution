using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Admin
{
    public class CustomerDetailResponse
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Role { get; set; } = default!;

        public bool IsActive { get; set; }

        public string BVN { get; set; } = default!;

        public bool IsBvnVerified { get; set; }

        public DateTime? BvnVerifiedAt { get; set; }
    }
}
