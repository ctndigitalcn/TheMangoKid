using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UserProfileController : Controller
    {
        [Authorize(Roles ="user")]
        // GET: UserProfile
        public ActionResult Index()
        {
            return View();
        }
    }
}