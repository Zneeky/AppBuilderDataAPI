using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class TrainingProgram
    {
        [Key]
        public int TrainingProgramId { get; set; } // Primary key

        [Required(ErrorMessage = "Training program title is required")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        public string Title { get; set; } = null!; // Title of the training program (e.g., "Strength Training Program")

        // List of exercises in the training program
        public ICollection<Exercise> Exercises { get; set; }

        // Constructor without parameters
        public TrainingProgram()
        {
            Exercises = new List<Exercise>();
        }
    }
}
