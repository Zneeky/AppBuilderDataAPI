﻿using System.ComponentModel.DataAnnotations;

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

        public int Calories { get; set; } // Number of calories in the meal

        public int Protein { get; set; } // Amount of protein in grams

        public int Carbs { get; set; } // Amount of carbohydrates in grams

        public int Fat { get; set; } // Amount of fat in grams

        public IEnumerable<DietPlan> DietPlans { get; set; } = null!;

        public string PicUrl { get; set; } = null!;

        public Meal()
        {
            DietPlans = new List<DietPlan>();
        }
    }
}
