using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class PersonalTrainingSession
    {
        [Key]
        public int SessionId { get; set; } // Primary key

        [Required]
        [ForeignKey(nameof(Member))] // Foreign key for Member
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!; // Navigation property

        [Required]
        [ForeignKey(nameof(Trainer))] // Foreign key for Trainer
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!; // Navigation property

        [Required(ErrorMessage = "Session date is required")]
        public DateTime SessionDate { get; set; } // Date and time of the session

        [Required(ErrorMessage = "Duration is required")]
        [Range(30, 180, ErrorMessage = "Duration must be between 30 and 180 minutes")]
        public int Duration { get; set; } // Duration of the session in minutes

    }
}
