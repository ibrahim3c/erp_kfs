using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Dtos
{
    public class AuthResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresOn { get; set; }

        public EmployeeAuthResponse? EmployeeDetails { get; set; }
    }

    public class EmployeeAuthResponse
    {
        public Guid Id { get; set; }
        public string? Email{ get; set; }
        public string? Phone { get; set; }
        public string? NationalId { get; set; }
        public string? JobTitleName { get; set; }
        public string? Name { get; set; }

        public bool IsActive { get; init; }

        public DateTime HireDate { get; init; }
        public DateTime? DateOfBirth { get; init; }
    }
}
