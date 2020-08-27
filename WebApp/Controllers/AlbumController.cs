using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddAlbum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAlbum(string albumName, string totalTrack)
        {
            return View();
        }
    }
}