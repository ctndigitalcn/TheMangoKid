using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Logout", "Authentication");
            }
            else
            {
                return View();
            }
        }
        public ActionResult MusicDistribution()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "UserProfile");
            }
            return View();
        }
        public ActionResult Pricing()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "UserProfile");
            }
            return View();
        }
    }
}