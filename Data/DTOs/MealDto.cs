using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.DTOs
{
    public class MealDto
    {
        public int MealId { get; set; } // Primary key

        public string Name { get; set; } = null!;

        public string Recipe { get; set; } = null!;

        public string Type { get; set; } = null!; // breakfast, lunch, dinner, snack

        public int Calories { get; set; } // Number of calories in the meal

        public int Protein { get; set; } // Amount of protein in grams

        public int Carbs { get; set; } // Amount of carbohydrates in grams

        public int Fat { get; set; } // Amount of fat in grams

        public string PicUrl { get; set; } = null!;
    }
}
