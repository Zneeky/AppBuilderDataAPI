using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; } // Primary key

        [Required(ErrorMessage = "Meal name is required")]
        [StringLength(100, ErrorMessage = "Meal name can't be longer than 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Recipe is required")]
        public string Recipe { get; set; } = null!;

        public string Type { get; set; } = null!; // breakfast, lunch, dinner, snack
    }
}
