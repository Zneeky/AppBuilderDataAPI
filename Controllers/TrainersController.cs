using AppBuilderDataAPI.Data;
using AppBuilderDataAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBuilderDataAPI.Controllers
{
    [Route("api/trainers")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly AppBuilderDataDbContext _context;

        public TrainersController(AppBuilderDataDbContext context)
        {
            _context = context;
        }

        // GET: api/Trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetMembers()
        {
            return await _context.Trainers.ToListAsync();
        }



        private List<DateTime> GenerateTimeSlots(DateTime date)
        {
            var slots = new List<DateTime>();
            var startTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);

            for (int i = 0; i < 12; i++) // 9 AM to 9 PM
            {
                slots.Add(startTime.AddHours(i));
            }
            return slots;
        }


    }
}
