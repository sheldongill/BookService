using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BookService.Controllers
{
    /// <summary>
    ///  returns current build version of service
    /// </summary>
    [Route("/[controller]")]
    public class VersionController : Controller
    {
        public VersionController()
        {
        }

        /// <summary>
        /// Retrieve the version information: Major, Minor, Patch and additional for build or whatever
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var runtimeVersion = typeof(Startup).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            var tokens = runtimeVersion.Version.Split('.');
            return tokens;
        }
    }
}
