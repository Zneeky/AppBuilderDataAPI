using AppBuilderDataAPI.Data;
using AppBuilderDataAPI.Data.DTOs;
using AppBuilderDataAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBuilderDataAPI.Controllers
{
    [Route("api/dietPlans")]
    [ApiController]
    public class DietPlansController : ControllerBase
    {
        private readonly AppBuilderDataDbContext _context;
        public DietPlansController( AppBuilderDataDbContext context) 
        {
            _context = context;
        }

        // GET: api/dietPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DietPlan>>> GetDietPlans()
        {
            return await _context.DietPlans.ToListAsync();
        }

        // GET: api/dietPlans/5/meals
        [HttpGet("{dietPlanId}/meals")]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetMeals(int dietPlanId)
        {
            var dietPlan = await _context.DietPlans
                .Include(dp => dp.Meals) // Include Meals
                .FirstOrDefaultAsync(dp => dp.DietPlanId == dietPlanId);

            if (dietPlan == null)
            {
                return NotFound();
            }

            var meals = dietPlan.Meals.Take(4).Select(m => MealMapper(m)).ToList();

            return meals;
        }

        [HttpGet("/meals/{mealId}")]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetMeal(int mealId)
        {
            var meal = await _context.Meals.FindAsync(mealId);

            if (meal == null)
            {
                return NotFound();
            }
            var mealList = new List<MealDto> { MealMapper(meal) };
            return mealList;
        }

        [HttpGet("/meals/{mealId}/chart")]
        public async Task<ActionResult<IEnumerable<MealChartItemDto>>> GetMealChart(int mealId)
        {
            var meal = await _context.Meals.FindAsync(mealId);

            if (meal == null)
            {
                return NotFound();
            }
            var listMealMacrosDtos = new List<MealChartItemDto>
            {
                new MealChartItemDto
                {
                    MacrosName = "Protein",
                    Quantity = meal.Protein,
                    Summary = $"Has {meal.Protein}g of protein"
                },
                new MealChartItemDto
                {
                    MacrosName = "Fat",
                    Quantity = meal.Fat,
                    Summary = $"Has {meal.Fat}g of fat"
                },
                new MealChartItemDto
                {
                    MacrosName = "Carbs",
                    Quantity = meal.Carbs,
                    Summary = $"Has {meal.Carbs}g of carbs"
                },
            };
            
            return listMealMacrosDtos;
        }

        private MealDto MealMapper(Meal meal)
        {
            return new MealDto
            {
                MealId = meal.MealId,
                Name = meal.Name,
                Recipe = meal.Recipe,
                Calories = meal.Calories,
                Protein = meal.Protein,
                Carbs = meal.Carbs,
                Fat = meal.Fat,
                PicUrl = meal.PicUrl
            };
        }
    }
}
