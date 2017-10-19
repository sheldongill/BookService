using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    /// <summary>
    ///  returns current build version of service
    /// </summary>
    [Route("/version")]
    public class VersionController : Controller
    {
        public VersionController()
        {
        }

        /// <summary>
        /// Retrieve the version information: Major, Minor, Revision and build
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(System.Reflection.Assembly.GetEntryAssembly().GetName().Version);
        }
    }
}
