using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BookService.ControllerExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BookService.Controllers
{
    [Route("/health-check")]
    public class HealthCheckController : Controller
    {
        private AppDbContext dbContext;

        public HealthCheckController(AppDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Checks the basic health of this application as it's running.
        /// </summary>
        /// <returns>200 - Running if okay.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Can we connect to the database server and execute a SQL query?
                var result = await dbContext.Database.ExecuteSqlCommandAsync("SELECT 1");
                if (result == 0)
                {
                    return this.InternalServerError("Couldn't execute SQL select!");
                    //return StatusCode(StatusCodes.Status500InternalServerError, "Couldn't execute SQL select!");
                }

                // Have any EF migrations run on this database?
                var migrations = await dbContext.Database.GetAppliedMigrationsAsync();
                if (migrations.Count() > 0)
                {
                    return Ok("Running");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "No migrations");
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
    }
}
