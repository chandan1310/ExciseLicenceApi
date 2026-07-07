using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExciseLicenceApi.Data;
using ExciseLicenceApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ExciseLicenceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallanCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChallanCheckController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChallanCheck
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallanCheck>>> GetChallans([FromQuery] string challanNo = null)
        {
            IQueryable<ChallanCheck> query = _context.ChallanChecks;

            if (!string.IsNullOrEmpty(challanNo))
            {
                query = query.Where(c => c.ChallanNo == challanNo);
            }

            var results = await query.ToListAsync();
            return Ok(results);
        }
    }
}