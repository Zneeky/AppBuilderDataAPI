using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppBuilderDataAPI.Data.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; } // Primary key

        // Personal Information
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        // Membership Details
        [Required]
        [ForeignKey(nameof(Membership))]
        public int MembershipId { get; set; } // Foreign key to Membership
        public Membership Membership { get; set; } = null!; // Navigation property

        // Activity Tracking
        public ICollection<PersonalTrainingSession> PersonalTrainingSessions { get; set; }
        public ICollection<GroupSession> GroupSessions { get; set; }

        // Additional Information
        [Required]
        public DateTime RegistrationDate { get; set; } // When the member joined the gym

        [Required]
        public bool IsActive { get; set; } // Status of membership

        // Constructor without parameters
        public Member()
        {
            PersonalTrainingSessions = new List<PersonalTrainingSession>();
            GroupSessions = new List<GroupSession>();
        }
    }
}
