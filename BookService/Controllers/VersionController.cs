using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    /// <summary>
    ///  returns current build version of service
    /// </summary>
    [Route("/version")]
    [AllowAnonymous]
    public class VersionController : Controller
    {
        /// <summary>
        /// Retrieve the version information: Major, Minor, Revision and build
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(System.Version), 200)]
        public IActionResult Get()
        {
            return Ok(System.Reflection.Assembly.GetEntryAssembly().GetName().Version);
        }
    }
}
