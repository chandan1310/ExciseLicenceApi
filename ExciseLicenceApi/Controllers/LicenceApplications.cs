using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExciseLicenceApi.Data;
using ExciseLicenceApi.Models;

namespace ExciseLicenceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenceApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // EF Core context is injected automatically here
        public LicenceApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LicenceApplications?financialYear=2026-2027
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarFirstRegistration>>> GetByFinancialYear([FromQuery] string financialYear = "2026-2027")
        {
            if (string.IsNullOrEmpty(financialYear))
            {
                return BadRequest("Financial year is required.");
            }


            var results = await _context.BarFirstRegistrations
                .Where(b => b.FinancialYear == financialYear)
                .ToListAsync();

            return Ok(results);
        }
    }
}