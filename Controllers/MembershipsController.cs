using AppBuilderDataAPI.Data;
using AppBuilderDataAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBuilderDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipsController : ControllerBase
    {
        private readonly AppBuilderDataDbContext _context;

        public MembershipsController(AppBuilderDataDbContext context)
        {
            _context = context;
        }
        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membership>>> GetMembers()
        {
            return await _context.Memberships.ToListAsync();
        }
    }
}
