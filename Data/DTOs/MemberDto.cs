using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.DTOs
{
    public class MemberDto
    {
        public int MemberId { get; set; } // Primary key

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public DateTime RegistrationDate { get; set; } // When the member joined the gym

        public bool IsActive { get; set; } // Status of membership
    }
}
