using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class SuperAdminController : Controller
    {
        BusinessLogics businessLogics;

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["LoginEmail"] == null)
            {
                return RedirectToAction("Logout", "Authentication");
            }
            businessLogics = new BusinessLogics();

            var pendingAlbumsGroup = businessLogics.GetAllAlbumsWithTracks()
                .Where(obj => obj.SingleTrackDetail.ReleaseDate <= DateTime.Now.AddHours(23) && obj.StoreSubmissionStatus==2)
                .ToList();


            var pendingEps = businessLogics.GetAllEpsWithTracks()
                .Where(obj => obj.SingleTrackDetail.ReleaseDate <= DateTime.Now.AddHours(23) && obj.StoreSubmissionStatus == 2)
                .ToList();

            var pendingSolos = businessLogics.GetAllSolosWithTracks()
                .Where(obj => obj.SingleTrackDetail.ReleaseDate <= DateTime.Now.AddHours(23) && obj.StoreSubmissionStatus == 2)
                .ToList();

            ViewBag.PendingAlbumsWithTrackDetail = pendingAlbumsGroup;
            ViewBag.PendingEpsWithTrackDetail = pendingEps;
            ViewBag.PendingSolosWithTrackDetail = pendingSolos;

            return View();
        }

        [HttpGet]
        public ActionResult AssignAdminRole(Guid accountId)
        {
            businessLogics = new BusinessLogics();
            if (true)
            {

            }
            else
            {
                TempData["ErrorMsg"] = "Some error occured while changing the role of the account.";
            }
            return RedirectToAction("ManageUsers","SuperAdmin");
        }

        [HttpGet]
        public ActionResult AssignSuperAdminRole(Guid accountId)
        {
            businessLogics = new BusinessLogics();
            if (true)
            {

            }
            else
            {
                TempData["ErrorMsg"] = "Some error occured while changing the role of the account.";
            }
            return RedirectToAction("ManageUsers", "SuperAdmin");
        }

        [HttpGet]
        public ActionResult DeactivateAccount(Guid accountId)
        {
            businessLogics = new BusinessLogics();
            if (true)
            {

            }
            else
            {
                TempData["ErrorMsg"] = "Some error occured while changing the role of the account.";
            }
            return RedirectToAction("ManageUsers", "SuperAdmin");
        }

        [HttpGet]
        public ActionResult PermanantlyDeleteAccount(Guid accountId)
        {
            businessLogics = new BusinessLogics();
            if (true)
            {

            }
            else
            {
                TempData["ErrorMsg"] = "Some error occured while changing the role of the account.";
            }
            return RedirectToAction("ManageUsers", "SuperAdmin");
        }

        [HttpGet]
        public ActionResult ManageUsers()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MarkAsSubmittedTo(Guid trackId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageSupport()
        {
            return View();
        }
    }
}