using AppBuilderDataAPI.Data;
using AppBuilderDataAPI.Data.DTOs;
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

        [HttpPost("api/trainer/availability/{trainerId}/{date}")]
        public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetTrainerAvailability(int trainerId, string date)
        {
            string format = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Eastern European Summer Time)'";
            var availability = await GetTrainerAvailabilityAsync(trainerId, DateTime.ParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture));
            //var availability = await GetTrainerAvailabilityAsync(trainerId, DateTime.Parse(date));
            return availability;
        }

        private async Task<List<TimeSlotDto>> GetTrainerAvailabilityAsync(int trainerId, DateTime date)
        {
            var timeSlots = GenerateTimeSlots(date);
            var personalSessions = await _context.PersonalTrainingSessions
                .Where(s => s.TrainerId == trainerId && s.SessionDate.Date.Day == date.Date.Day)
                .ToListAsync();

            var groupSessions = await _context.GroupSessions
                .Where(g => g.TrainerId == trainerId && g.SessionDate.Date.Day == date.Date.Day)
                .ToListAsync();

            var availability = timeSlots.Select(slot =>
            {
                var isGroupSession = (slot.Hour == 10 && slot.Minute == 0) || (slot.Hour == 19 && slot.Minute == 0);
                var sessionType = isGroupSession ? "Group Session" : "Personal Training";

                var isAvailable = !personalSessions.Any(s => s.SessionDate <= slot && s.SessionDate.AddMinutes(s.Duration) > slot) &&
                                  !groupSessions.Any(g => g.SessionDate <= slot && g.SessionDate.AddMinutes(g.Duration) > slot);

                return new TimeSlotDto
                {
                    Time = slot.ToString("HH:mm"),
                    IsAvailable = isAvailable,
                    SessionType = sessionType
                };
            }).ToList();

            return availability;
        }

    }
}
