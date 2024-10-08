using System;
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
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var member1 = await _context.Members.FirstOrDefaultAsync();
            var list = new List<Member> { member1 };
            return list;
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

            return Ok(member);
        }

        // Login and return a JWT token
        [HttpPost("login")]
        public async Task<ActionResult<Member>> Login(LoginDto loginDTO)
        {
            // Find the member by email
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == loginDTO.Email);

            if (member == null || loginDTO.Password != member.Password)
            {
                return Unauthorized("Invalid email or password.");
            }

            return member;
        }


        // GET: api/Members/5
        [HttpGet("member/{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            var memberDto = new MemberDto
            {
                FullName = member.FullName
            };

            return member;
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}
