using DomainModel;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    //[Authorize(Roles = "user")]
    public class UserProfileController : Controller
    {
        BusinessLogics businessLogics;
        GeneralLogics logics;

        [HttpGet]
        public ActionResult Index()
        {
            //if (Session["LoginEmail"] == null)
            //{
            //    return RedirectToAction("Logout", "Authentication");
            //}

            businessLogics = new BusinessLogics();

            //string userEmail = Session["LoginEmail"].ToString();
            string userEmail = "koushik.official1999@gmail.com";

            // to help view to find out if it will allow user to add bank details
            if (businessLogics.IsBankDetailsExistsOf(userEmail))
            {
                ViewBag.BankAccountSubmissionStatus = 1;
            }
            else
            {
                ViewBag.BankAccountSubmissionStatus = 0;
            }

            //to help view to find out how many Albums can be created by user
            int AlbumsAlreadyCreated = businessLogics.CountOfAlbumsAlreadyCreatedBy(userEmail);
            int AlbumCountLeftToCreate = businessLogics.CountOfAlbumsCanBeCreatedBy(userEmail) - AlbumsAlreadyCreated;
            ViewBag.AlbumCount = AlbumCountLeftToCreate;

            //pass the album details
            if (AlbumsAlreadyCreated > 0)
            {
                ViewBag.Albums = businessLogics.GetAllTheAlbumsOf(userEmail);
            }

            //Code space for ep and singles

            return View();
        }

        [HttpGet]
        public ActionResult PurchaseAlbum()
        {
            businessLogics = new BusinessLogics();
            var result = businessLogics.PurchaseAlbumFor("koushik.official1999@gmail.com");
            if (result != 1)
            {
                ViewBag.ErrorMsg = "Purchase was not successfull";
            }
            return RedirectToAction("Index", "UserProfile");
        }

    }
}