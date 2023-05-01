using Microsoft.AspNetCore.Mvc;


namespace Customer.Areas.Ar.Controllers
{
    [Area("ar")]
    [Route("ar")]
    public class B2BController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
