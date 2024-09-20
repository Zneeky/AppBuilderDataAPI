using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; } // Primary key

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters")]
        public string FullName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50")]
        public int YearsOfExperience { get; set; }

        public string PictureUrl { get; set; } = null!; // Link to profile picture

        public ICollection<PersonalTrainingSession> PersonalTrainingSessions { get; set; } = null!;
        public ICollection<GroupSession> GroupSessions { get; set; } = null!;

        public Trainer()
        {

           PersonalTrainingSessions = new List<PersonalTrainingSession>();
            GroupSessions = new List<GroupSession>();
        }
    }
}
