using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string Forbidden()
        {
            return "Forbidden";
        }

        public string TokenNull()
        {
            return "TokenNull";
        }

        public string NotFound1()
        {
            return "NotFound1";
        }

        public string AdminPage()
        {
            return "Admin Page";
        }


        
        public string MemberPage()
        {
            return "MemberPage";
        }
    }
}
