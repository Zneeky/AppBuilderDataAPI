﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppBuilderDataAPI.Data;
using AppBuilderDataAPI.Data.Models;
using Microsoft.CodeAnalysis.Scripting;
using AppBuilderDataAPI.Data.DTOs;

namespace AppBuilderDataAPI.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly AppBuilderDataDbContext _context;

        public MembersController(AppBuilderDataDbContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
        {
            var members = await _context.Members.ToListAsync();
            var memberDtos = members.Select(m => mapperMember(m)).ToList();

            return memberDtos;
        }

        // Register a new member
        [HttpPost("register")]
        public async Task<ActionResult<MemberDto>> Register(RegisterDto registerMember)
        {
            // Check if the email already exists
            if (_context.Members.Any(m => m.Email == registerMember.Email))
            {
                return Unauthorized("Email is already in use.");
            }

            var member = new Member
            {
                FullName = registerMember.FullName,
                Email = registerMember.Email,
                Password = registerMember.Password, // Store the hashed password
                DateOfBirth = DateTime.Now, // Replace with actual value from input
                MembershipId = 1, // Assign an actual membership ID based on your logic
                RegistrationDate = DateTime.Now,
                IsActive = true // Initially active
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return mapperMember(member);
        }

        // Login and return a JWT token
        [HttpPost("login")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> Login(LoginDto loginDTO)
        {
            // Find the member by email
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == loginDTO.Email);

            if (member == null || loginDTO.Password != member.Password)
            {
                return Unauthorized("Invalid email or password.");
            }

            var list = new List<MemberDto> { mapperMember(member) };
            return list;
        }


        // GET: api/Members/5
        //If I do not return a list the AppBuilder config doesn't work, so I return a list with one element, I get a warning "This resource contains a single object"
        [HttpGet("member/{id}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }
            var list = new List<MemberDto> { mapperMember(member) };
            return list;
        }

        [HttpGet("member/{id}/trainingSessions")]

        public async Task<ActionResult<IEnumerable<MemberTrainingSessionDto>>> GetMemberTrainingSessions(int id)
        {
            var member = await _context.Members.FindAsync(id);
            var trainingPersonalSessions = await _context.PersonalTrainingSessions
                .Include(ts => ts.Trainer)
                .Where(ts => ts.MemberId == id)
                .Select(ts => new MemberTrainingSessionDto
                {
                    TrainerName= ts.Trainer.FullName,
                    TrainerPicUrl = ts.Trainer.PictureUrl,
                    DateTime = ts.SessionDate.ToString(),
                    Type = "Personal Training"
                })
                .ToListAsync();

            var trainingGroupSessions = await _context.GroupSessions
                .Include(gs => gs.Trainer)
                .Where(gs => gs.Members.Any(m=> m.MemberId==id))
                .Select(ts => new MemberTrainingSessionDto
                {
                    TrainerName = ts.Trainer.FullName,
                    TrainerPicUrl = ts.Trainer.PictureUrl,
                    DateTime = ts.SessionDate.ToString(),
                    Type = "Group Training"
                })
                .ToListAsync();

            var trainingSessions = trainingPersonalSessions.Concat(trainingGroupSessions).ToList();

            return trainingSessions;
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        private MemberDto mapperMember(Member member)
        {
            return new MemberDto
            {
                MemberId = member.MemberId,
                FullName = member.FullName,
                Email = member.Email,
                Password = member.Password,
                DateOfBirth = member.DateOfBirth,
                RegistrationDate = member.RegistrationDate.Date,
                IsActive = member.IsActive,
                ProfilePicUrl = member.ProfilePicUrl
            };
        }
    }
}
