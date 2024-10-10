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

        [HttpGet("api/trainer/availability")]
        public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetTrainerAvailability([FromBody] TrainerAvailabilityRequestDto request)
        {
            var availability = await GetTrainerAvailabilityAsync(request.TrainerId, request.Date);
            return availability;
        }

        private async Task<List<TimeSlotDto>> GetTrainerAvailabilityAsync(int trainerId, DateTime date)
        {
            var timeSlots = GenerateTimeSlots(date);
            var personalSessions = await _context.PersonalTrainingSessions
                .Where(s => s.TrainerId == trainerId && s.SessionDate.Date == date.Date)
                .ToListAsync();

            var groupSessions = await _context.GroupSessions
                .Where(g => g.TrainerId == trainerId && g.SessionDate.Date == date.Date)
                .ToListAsync();

            var availability = timeSlots.Select(slot =>
            {
                var isGroupSession = (slot.Hour == 10 && slot.Minute == 0) || (slot.Hour == 19 && slot.Minute == 0);
                var sessionType = isGroupSession ? "Group Session" : "Personal Training";

                var isAvailable = !personalSessions.Any(s => s.SessionDate <= slot && s.SessionDate.AddMinutes(s.Duration) > slot) &&
                                  !groupSessions.Any(g => g.SessionDate <= slot && g.SessionDate.AddMinutes(g.Duration) > slot);

                return new TimeSlotDto
                {
                    Time = slot,
                    IsAvailable = isAvailable,
                    SessionType = sessionType
                };
            }).ToList();

            return availability;
        }

    }
}
