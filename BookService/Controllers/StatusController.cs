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
    [Route("/status")]
    public class StatusController : Controller
    {
        private AppDbContext dbContext;

        public StatusController(AppDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Checks the basic health status of this application as it's running.
        /// </summary>
        /// <returns>200 - Running if okay.</returns>
        /// <returns>500 if there are no migrations or the database isn't readable.</returns>
        /// <returns>503 if there is a problem connecting to the database.</returns>
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

