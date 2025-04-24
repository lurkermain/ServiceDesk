using Microsoft.AspNetCore.Mvc;

namespace Diddi.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View(); // Views/Home/Index.cshtml
		}
		/*public IActionResult ServiceDesk()
		{
			return View();
		}*/
	}
}
