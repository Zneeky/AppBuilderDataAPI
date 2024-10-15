using AppBuilderDataAPI.Data;
using AppBuilderDataAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBuilderDataAPI.Controllers
{
    [Route("api/trainingPrograms")]
    [ApiController]
    public class TrainingProgramsController : ControllerBase
    {
        private readonly AppBuilderDataDbContext _context;
        public TrainingProgramsController( AppBuilderDataDbContext context) 
        {
            _context = context;
        }

        // GET: api/trainingPrograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingProgram>>> GetTrainingPrograms()
        {
            return await _context.TrainingPrograms.ToListAsync();
        }

        // GET: api/trainingPrograms/5/exercises
        [HttpGet("{trainingProgramId}/exercises")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises(int trainingProgramId)
        {
            var trainingProgram = await _context.TrainingPrograms
                .Include(tp => tp.Exercises) // Include Exercises
                .FirstOrDefaultAsync(tp => tp.TrainingProgramId == trainingProgramId);

            if (trainingProgram == null)
            {
                return NotFound();
            }

            var exercises = trainingProgram.Exercises.ToList();

            return exercises;
        }

        [HttpGet("/exercises/{exerciseId}")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercise(int exerciseId)
        {
            var exercise = await _context.Exercises.FindAsync(exerciseId);

            if (exercise == null)
            {
                return NotFound();
            }
            var exerciseList = new List<Exercise> { exercise };
            return exerciseList;
        }

        //[HttpGet("/exercises/{exerciseId}/chart")]
        //public async Task<ActionResult<IEnumerable<ExerciseChartItemDto>>> GetExerciseChart(int exerciseId)
        //{
        //    var exercise = await _context.Exercises.FindAsync(exerciseId);

        //    if (exercise == null)
        //    {
        //        return NotFound();
        //    }

        //    var exerciseChartItems = await _context.ExerciseChartItems
        //        .Where(eci => eci.ExerciseId == exerciseId)
        //        .ToListAsync();

        //    return exerciseChartItems.Select(eci => ExerciseChartItemMapper(eci)).ToList();
        //}

        //private ExerciseDto ExerciseMapper(Exercise exercise)
        //{
        //    return new ExerciseDto
        //    {
        //        ExerciseId = exercise.ExerciseId,
        //        Name = exercise.Name,
        //        Description = exercise.Description,
        //        ImageUrl = exercise.ImageUrl
        //    };
        //}

        //private ExerciseChartItemDto ExerciseChartItemMapper(ExerciseChartItem exerciseChartItem)
        //{
        //    return new ExerciseChartItemDto
        //    {
        //        ExerciseChartItemId = exerciseChartItem.Exercise
        //    };
        //}
    }
}
