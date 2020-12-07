using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ExtendedPlayController : Controller
    {
        BusinessLogics businessLogics;
        GeneralLogics logics;

        [HttpGet]
        public ActionResult CreateOnlyEp()
        {
            ViewBag.Title = "Add Ep";
            return View("AddorEditEp");
        }

        [HttpGet]
        public ActionResult EditOnlyEp(Guid epId)
        {
            //string userEmail = Session["LoginEmail"].ToString();
            string userEmail = "koushik.official1999@gmail.com";

            businessLogics = new BusinessLogics();

            if (userEmail != null)
            {
                if (businessLogics.IsAccountContainsThisEp(userEmail, epId))
                {
                    ViewBag.Title = "Edit Album";

                    var epObject = businessLogics.GetEpById(epId);
                    if (epObject != null)
                    {
                        ViewBag.EpDetail = epObject;
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No Ep found with the Id provided";
                    }
                    return View("AddorEditEp");
                }
                else
                {
                    TempData["ErrorMsg"] = "You are trying to modify an Ep that does not belongs to you";
                    return RedirectToAction("Index", "UserProfile");
                }
            }
            else
            {
                return RedirectToAction("Index", "UserProfile");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOnlyEp(string epName, string totalTrack)
        {
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            List<string> inputStrings = new List<string> { epName, totalTrack };

            string userEmail = Session["LoginEmail"].ToString();
            //string userEmail = "koushik.official1999@gmail.com";
            if (userEmail != null)
            {
                if (!logics.ContainsAnyNullorWhiteSpace(inputStrings))
                {
                    if (logics.ContainsOnlyDigits(totalTrack))
                    {
                        if(Convert.ToInt32(totalTrack) > 7)
                        {
                            ViewBag.ErrorMsg = "An Ep can be created with 7 or less amount of tracks";
                        }
                        else
                        {
                            var result = businessLogics.CreateNewEp(userEmail, epName, Convert.ToInt32(totalTrack));

                            if (result == 0)
                            {
                                ViewBag.ErrorMsg = "No Account is associated with the email address from which user is trying to create the Ep.";
                            }

                            if (result == 1)
                            {
                                return RedirectToAction("Index", "UserProfile");
                            }

                            if (result == 2)
                            {
                                ViewBag.ErrorMsg = "No purchase left to create an music Ep";
                            }

                            if (result == 3 || result == 4)
                            {
                                ViewBag.ErrorMsg = "Internal error occured while creating the Ep";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Track number field must contains only number. Invalid input given";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "No fields should be left empty";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Authentication");
            }
            return View("AddorEditEp");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOnlyEp(Guid epId, string epName, string totalTrack)
        {
            ViewBag.Title = "Edit Ep";
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            List<string> inputStrings = new List<string> { epName, totalTrack };

            //string userEmail = Session["LoginEmail"].ToString();
            string userEmail = "koushik.official1999@gmail.com";
            if (userEmail != null)
            {
                if (!logics.ContainsAnyNullorWhiteSpace(inputStrings))
                {
                    if (logics.ContainsOnlyDigits(totalTrack))
                    {
                        if (Convert.ToInt32(totalTrack) > 7)
                        {
                            TempData["ErrorMsg"] = "An Ep can be created with 7 or less amount of tracks";
                            //ViewBag.ErrorMsg = "An Ep can be created with 7 or less amount of tracks";
                        }
                        else
                        {
                            var result = businessLogics.EditEp(epId, epName, Convert.ToInt32(totalTrack));

                            if (result == 0)
                            {
                                ViewBag.ErrorMsg = "No Account is associated with the email address from which user is trying to modify the Ep.";
                            }
                            if (result == 1)
                            {
                                return RedirectToAction("Index", "UserProfile");
                            }
                            if (result == 2)
                            {
                                ViewBag.ErrorMsg = "No purchase left to create an music Ep";
                            }
                            if (result == 3 || result == 4)
                            {
                                ViewBag.ErrorMsg = "Internal error occured while creating the Ep";
                            }
                        }
                        
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Track number field must contains only number. Invalid input given";
                        //ViewBag.ErrorMsg = "Track number field must contains only number. Invalid input given";
                    }
                }
                else
                {
                    TempData["ErrorMsg"] = "No fields should be left empty";
                    //ViewBag.ErrorMsg = "No fields should be left empty";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Authentication");
            }
            return RedirectToAction("EditOnlyEp", new { epID = epId });
        }

        //For individual user use
        [HttpGet]
        public ActionResult DeleteEp(Guid epId)
        {
            //string userEmail = Session["LoginEmail"].ToString();
            string userEmail = "koushik.official1999@gmail.com";

            if (userEmail == null)
            {
                return RedirectToAction("Index", "UserProfile");
            }

            businessLogics = new BusinessLogics();

            if (businessLogics.IsAccountContainsThisEp(userEmail, epId))
            {
                var result = businessLogics.DeleteEp(epId);
                /*
            0 = No Album found. Operation failed.
            1 = Operation done successfully.
            2 = Operation Faild while deleting album. Internal error occured.
            3 = An Record Couldn't deleted from AlbumTrackMaster Table due to internal error.
            4 = Operation Failed in the level of AlbumTrackMaster Label.
            5 = Error while deleting a solo track that belongs to only the album
            6 = Track fetching failed.
            7 = Error occured while deleting a valid track
            8 = A track from the album is already submitted to the store. can't delete album
            9 = Error while updating the associated purchase record. User can't create an album with the valid purchase.
             */
                if (result != 1)
                {
                    TempData["ErrorMsg"] = "Error occured while deleting Ep";
                }
                return RedirectToAction("Index", "UserProfile");
            }
            else
            {
                TempData["ErrorMsg"] = "You are trying to remove an Ep that doesn't belongs to you";
                return RedirectToAction("Index", "UserProfile");
            }
        }

        //For individual user and admin use
        [HttpGet]
        public ActionResult ShowIndividualEpSongs(Guid epId)
        {
            businessLogics = new BusinessLogics();

            var ep = businessLogics.GetEpById(epId);
            ViewBag.Ep = ep;

            //get the list of all tracks of the same Ep
            ViewBag.details = businessLogics.GetAllTracksWithEpDetails(epId);

            ViewBag.IsEpFull = businessLogics.IsEpFull(epId);

            return View("DisplayEp");
        }

        //For Admin use only
        [HttpGet]
        public ActionResult GetAllCompletedEps()
        {
            return View();
        }
    }
}