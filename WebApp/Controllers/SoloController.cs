using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace WebApp.Controllers
{
    public class SoloController : Controller
    {
        GeneralLogics logic;
        BusinessLogics businessLogics;
        // GET: Solo
        [HttpGet]
        public ActionResult AddTrackToAlbum(Guid albumId)
        {
            logic = new GeneralLogics();

            return View();
        }
        [HttpPost]
        public ActionResult AddTrackToAlbum(Guid albumId, string TrackTitle, string ArtistName, bool ArtistAlreadyInSpotify, string ArtistSpotifyUrl, DateTime ReleaseDate, string Genre, string CopyrightClaimerName, string AuthorName, string ComposerName, string ArrangerName, string ProducerName, bool AlreadyHaveAnISRC, string ISRC_Number, int PriceTier, bool ExplicitContent, bool IsTrackInstrumental, string LyricsLanguage, string TrackZipFileLink, string ArtWork_Link)
        {
            List<string> inputElements = new List<string> { TrackTitle, ArtistName, Genre, CopyrightClaimerName, AuthorName, ComposerName, ArrangerName, ProducerName, TrackZipFileLink, ArtWork_Link };
            List<string> inputElements1 = new List<string> { TrackTitle, ArtistName, Genre, CopyrightClaimerName, AuthorName, ComposerName, ArrangerName, ProducerName };

            logic = new GeneralLogics();

            if (!logic.ContainsAnyNullorWhiteSpace(inputElements))
            {
                if (logic.ContainsOnlyAlphabets(inputElements1))
                {
                    if (ArtistAlreadyInSpotify && !String.IsNullOrWhiteSpace(ArtistSpotifyUrl))
                    {
                        if (AlreadyHaveAnISRC && !String.IsNullOrWhiteSpace(ISRC_Number))
                        {
                            if (ReleaseDate == null || ReleaseDate < logic.CurrentIndianTime())
                            {
                                ViewBag.ErrorMsg = "Invalid Release Date";
                            }
                            else
                            {
                                if (IsTrackInstrumental && String.IsNullOrWhiteSpace(LyricsLanguage))
                                {
                                    businessLogics = new BusinessLogics();
                                    //Code to add the track to the album
                                    var result = businessLogics.CreateNewTrackForAlbum(albumId, TrackTitle, ArtistName, ArtistAlreadyInSpotify, ArtistSpotifyUrl, ReleaseDate, Genre, CopyrightClaimerName, AuthorName, ComposerName, ArrangerName, ProducerName, AlreadyHaveAnISRC, ISRC_Number, PriceTier, ExplicitContent, IsTrackInstrumental, LyricsLanguage, TrackZipFileLink, ArtWork_Link);

                                    if (result == 1)
                                    {
                                        return RedirectToAction("ShowIndividualAlbumSongs", "Album", new { albumId = albumId });
                                    }
                                    else
                                    {
                                        Console.Write(result);
                                        ViewBag.ErrorMsg = "Internal Error occured";
                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "If the track is instrumental then it should not contain any lyrics language";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Provide the ISRC number for the track";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Provide the artist profile link for spotify";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "only alphabets are allowed.Invalid inputs given";
                }
                
            }
            else
            {
                ViewBag.ErrorMsg = "Fields can't be left empty";
            }
            return View();
        }
    }
}