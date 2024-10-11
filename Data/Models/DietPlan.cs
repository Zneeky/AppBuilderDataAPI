using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class DietPlan
    {
        [Key]
        public int DietPlanId { get; set; } // Primary key

        [Required(ErrorMessage = "Diet plan name is required")]
        [StringLength(100, ErrorMessage = "Diet plan name can't be longer than 100 characters")]
        public string Name { get; set; } = null!; // Name of the diet plan (e.g., "Weight Loss Plan")

        [Required(ErrorMessage = "Body type is required")]
        [StringLength(50, ErrorMessage = "Body type can't be longer than 50 characters")]
        public string BodyType { get; set; } = null!; // E.g., "Ectomorph", "Mesomorph", etc.

        // List of daily meal suggestions
        public IEnumerable<Meal> Meals { get; set; } // Meals suggested in this diet plan

        // Constructor without parameters
        public DietPlan()
        {
            Meals = new List<Meal>();
        }
    }
}
