using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookService.Models;

namespace BookService.Controllers
{
    [Route("/[controller]")]
    public class HealthCheckController : Controller
    {
        private AppDBContext ctx;

        public HealthCheckController(AppDBContext context)
        {
            ctx = context;
        }

        public async Task<IActionResult> GetAll()
        {
            var migrations = await ctx.Database.GetAppliedMigrationsAsync();

            if (migrations.Count() > 0)
            {
                return Ok("Running");
            }
            return StatusCode(500,"Broken");
        }
    }
}
