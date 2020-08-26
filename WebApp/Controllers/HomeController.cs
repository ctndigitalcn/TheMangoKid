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
            return View();
        }
        public ActionResult MusicDistribution()
        {
            return View();
        }
        public ActionResult Pricing()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return RedirectToAction("SignUp", "Authentication");
        }
        public ActionResult Signin()
        {
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Logout()
        {
            return RedirectToAction("Logout", "Authentication");
        }
    }
}