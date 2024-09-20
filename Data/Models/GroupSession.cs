using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class GroupSession
    {
        [Key]
        public int GroupSessionId { get; set; } // Primary key

        [Required(ErrorMessage = "Session name is required")]
        [StringLength(100, ErrorMessage = "Session name can't be longer than 100 characters")]
        public string SessionName { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Trainer))] // Foreign key for Trainer
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!; // Navigation property

        [Required(ErrorMessage = "Session date is required")]
        public DateTime SessionDate { get; set; } // Date and time of the group session

        [Required(ErrorMessage = "Duration is required")]
        [Range(30, 180, ErrorMessage = "Duration must be between 30 and 180 minutes")]
        public int Duration { get; set; } // Duration of the session in minutes

        [Required(ErrorMessage = "Max participants are required")]
        [Range(1, 100, ErrorMessage = "Max participants must be between 1 and 100")]
        public int MaxParticipants { get; set; } // Max number of people allowed

        // List of members that have booked this group session
        public ICollection<Member> Members { get; set; } = null!;

        // Constructor without parameters
        public GroupSession()
        {
            Members = new List<Member>();
        }
    }
}
