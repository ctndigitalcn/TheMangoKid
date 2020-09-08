using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        BusinessLogics businessLogics;
        GeneralLogics logics;

        [HttpGet]
        public ActionResult CreateOnlyAlbum()
        {
            ViewBag.Title = "Add Album";
            return View("AddorEditAlbum");
        }

        [HttpGet]
        public ActionResult EditOnlyAlbum(Guid albumId)
        {
            string userEmail = Session["LoginEmail"].ToString();
            //string userEmail = "koushik.official199@gmail.com";

            businessLogics = new BusinessLogics();

            if (userEmail != null)
            {
                if (businessLogics.IsAccountContainsThisAlbum(userEmail, albumId))
                {
                    ViewBag.Title = "Edit Album";

                    var albumObject = businessLogics.GetAlbumById(albumId);
                    if (albumObject != null)
                    {
                        ViewBag.AlbumDetail = albumObject;
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No album found with the Id provided";
                    }
                    return View("AddorEditAlbum");
                }
                else
                {
                    TempData["ErrorMsg"] = "You are trying to modify an Album that does not belongs to you";
                    return RedirectToAction("Index", "UserProfile");
                }
            }
            else
            {
                return RedirectToAction("Index", "UserProfile");
            }        
        }

        [HttpPost]
        public ActionResult CreateOnlyAlbum(string albumName, string totalTrack)
        {
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            List<string> inputStrings = new List<string> { albumName, totalTrack };

            string userEmail = Session["LoginEmail"].ToString();
            //string userEmail = "koushik.official1999@gmail.com";
            if (userEmail != null)
            {
                if (!logics.ContainsAnyNullorWhiteSpace(inputStrings))
                {
                    if (logics.ContainsOnlyDigits(totalTrack))
                    {
                        var result = businessLogics.CreateNewAlbum(userEmail, albumName, Convert.ToInt32(totalTrack));

                        if (result == 0)
                        {
                            ViewBag.ErrorMsg = "No Account is associated with the email address from which user is trying to create the album.";
                        }
                        if (result == 1)
                        {
                            return RedirectToAction("Index", "UserProfile");
                        }
                        if (result == 2)
                        {
                            ViewBag.ErrorMsg = "No purchase left to create an music album";
                        }
                        if (result == 3 || result == 4)
                        {
                            ViewBag.ErrorMsg = "Internal error occured while creating the album";
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
            return View("AddorEditAlbum");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOnlyAlbum(string albumId, string albumName, string totalTrack)
        {
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            List<string> inputStrings = new List<string> { albumName, totalTrack };

            string userEmail = Session["LoginEmail"].ToString();
            //string userEmail = "koushik.official1999@gmail.com";
            if (userEmail != null)
            {
                if (!logics.ContainsAnyNullorWhiteSpace(inputStrings))
                {
                    if (logics.ContainsOnlyDigits(totalTrack))
                    {
                        var result = businessLogics.EditAlbum(Guid.Parse(albumId), albumName, Convert.ToInt32(totalTrack));

                        if (result == 0)
                        {
                            ViewBag.ErrorMsg = "No album found with the Id provided";
                        }
                        if (result == 1)
                        {
                            return RedirectToAction("Index", "UserProfile");
                        }
                        if (result == 2)
                        {
                            ViewBag.ErrorMsg = "Can't edit album as one song alredy registered under the album";
                        }
                        if (result == 3)
                        {
                            ViewBag.ErrorMsg = "Internal error occured while creating the album";
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
            return View("AddorEditAlbum");
        }

        //For individual user use
        [HttpGet]
        public ActionResult DeleteAlbum(Guid albumId)
        {
            string userEmail = Session["LoginEmail"].ToString();
            //string userEmail = "koushik.official1999@gmail.com";

            if (userEmail == null)
            {
                return RedirectToAction("Index", "UserProfile");
            }

            businessLogics = new BusinessLogics();

            if(businessLogics.IsAccountContainsThisAlbum(userEmail, albumId))
            {
                var result = businessLogics.DeleteAlbum(albumId);
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
                    TempData["ErrorMsg"] = "Error occured while deleting album";
                }
                return RedirectToAction("Index", "UserProfile");
            }
            else
            {
                TempData["ErrorMsg"] = "You are trying to remove an Album that doesn't belongs to you";
                return RedirectToAction("Index", "UserProfile");
            }
        }

        //For individual user and admin use
        [HttpGet]
        public ActionResult ShowIndividualAlbumSongs(Guid albumId)
        {
            businessLogics = new BusinessLogics();

            var album = businessLogics.GetAlbumById(albumId);
            ViewBag.Album = album;

            //get the list of all tracks of the same album
            ViewBag.details = businessLogics.GetAllTracksWithAlbumDetails(albumId);

            ViewBag.IsAlbumFull = businessLogics.IsAlbumFull(albumId);

            return View("DisplayAlbum");
        }

        //For Admin use only
        [HttpGet]
        public ActionResult GetAllCompletedAlbums()
        {
            return View();
        }
    }
}