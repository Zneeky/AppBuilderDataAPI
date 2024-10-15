using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; } // Primary key

        [Required(ErrorMessage = "Exercise name is required")]
        [StringLength(100, ErrorMessage = "Exercise name can't be longer than 100 characters")]
        public string Name { get; set; } = null!; // Name of the exercise (e.g., "Bench Press")

        [Required(ErrorMessage = "Reps range is required")]
        [StringLength(50, ErrorMessage = "Reps range can't be longer than 50 characters")]
        public string RepsRange { get; set; } = null!;// A string that holds the reps range (e.g., "8-12 reps")

        [Required(ErrorMessage = "Target muscle group is required")]
        [StringLength(100, ErrorMessage = "Target muscle group can't be longer than 100 characters")]
        public string MuscleGroupTarget { get; set; } = null!; // The muscle group targeted by the exercise

        [Required(ErrorMessage = "PicUrl is required")]
        public string PicUrl { get; set; } = null!; // The equipment needed for the exercise

    }
}
