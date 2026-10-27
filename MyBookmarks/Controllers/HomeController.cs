using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookmarks.ViewModels;
using System.Diagnostics;

namespace MyBookmarks.Controllers
{
    /// <summary>
    /// Serves the public home, privacy and error pages.
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the application's home page.
        /// </summary>
        /// <returns>The home view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the privacy page.
        /// </summary>
        /// <returns>The privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page with the current request identifier.
        /// </summary>
        /// <returns>The error view with diagnostic request information.</returns>
        /// <remarks>
        /// The response is not cached so request-specific information is not reused.
        /// </remarks>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
