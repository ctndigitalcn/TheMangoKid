﻿using ServiceLayer;
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
            ViewBag.AlbumId = albumId;
            ViewBag.Title = "Add Track";
            return View("AddorEditTrack");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTrackToAlbum(Guid albumId, string TrackTitle, string ArtistName, string ArtistAlreadyInSpotify, string ArtistSpotifyUrl, DateTime ReleaseDate, string Genre, string CopyrightClaimerName, string AuthorName, string ComposerName, string ArrangerName, string ProducerName, string AlreadyHaveAnISRC, string ISRC_Number, string PriceTier, string ExplicitContent, string IsTrackInstrumental, string LyricsLanguage, string TrackZipFileLink, string ArtWork_Link)
        {
            ViewBag.AlbumId = albumId;
            List<string> mandetoryFieldsInput = new List<string> { TrackTitle, ArtistName, Genre, CopyrightClaimerName, AuthorName, ComposerName, ArrangerName, ProducerName, PriceTier, TrackZipFileLink, ArtWork_Link };
            List<string> mandetoryBoolFields = new List<string> { ArtistAlreadyInSpotify, AlreadyHaveAnISRC, ExplicitContent, IsTrackInstrumental };

            logic = new GeneralLogics();

            if (!logic.ContainsAnyNullorWhiteSpace(mandetoryFieldsInput))
            {
                if (!logic.ContainsAnyNullorWhiteSpace(mandetoryBoolFields))
                {
                    bool isArtistOnSpotify=false, isTrackHasISRC=false, isTrackHasExplicitContent=false, isTrackInstrumental=false;
                    if (ArtistAlreadyInSpotify == "yes")
                    {
                        isArtistOnSpotify = true;
                    }
                    if (AlreadyHaveAnISRC == "yes")
                    {
                        isTrackHasISRC = true;
                    }
                    if (ExplicitContent == "yes")
                    {
                        isTrackHasExplicitContent = true;
                    }
                    if (IsTrackInstrumental == "yes")
                    {
                        isTrackInstrumental = true;
                    }

                    if(!isArtistOnSpotify && !logic.ContainsOnlyAlphabets(ArtistSpotifyUrl))
                    {
                        if (!isTrackHasISRC && String.IsNullOrWhiteSpace(ISRC_Number.Trim()))
                        {
                            if (isTrackInstrumental && String.IsNullOrWhiteSpace(LyricsLanguage.Trim()))
                            {
                                if (ReleaseDate!=null && ReleaseDate > logic.CurrentIndianTime())
                                {
                                    //Code to add the song to the album
                                    businessLogics = new BusinessLogics();
                                    var result = businessLogics.CreateNewTrackForAlbum(albumId, TrackTitle, ArtistName, isArtistOnSpotify, ArtistSpotifyUrl, ReleaseDate, Genre, CopyrightClaimerName, AuthorName, ComposerName, ArrangerName, ProducerName, isTrackHasISRC, ISRC_Number, Convert.ToInt32(PriceTier), isTrackHasExplicitContent, isTrackInstrumental, LyricsLanguage, TrackZipFileLink, ArtWork_Link);
                                    if (result == 1)
                                    {
                                        return RedirectToAction("ShowIndividualAlbumSongs", "Album", new { albumId = albumId });
                                    }
                                    else if (result==7) 
                                    {
                                        ViewBag.ErrorMsg = "Your purchase has expired. you can't add the track to the album.";
                                    }
                                    else if (result == 8)
                                    {
                                        ViewBag.ErrorMsg = "You can't add the track as the album is full.";
                                    }
                                    else
                                    {
                                        Console.Write(result);
                                        ViewBag.ErrorMsg = "Internal Error occured";
                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "Provide a valid Date to release your track";
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "If it's an instrumental track then leave the Lyrics Language field empty";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "If you have ISRC number for the track then select yes and provide the number. Otherwise select no and leave the field empty.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "If artist is already on spotify then select yes and provide the link of the artist. Otherwise select no and leave the field empty.";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Select proper options from dropdowns";
                }

            }
            else
            {
                ViewBag.ErrorMsg = "Mandetory Fields can't be left empty";
            }
            return View("AddorEditTrack");
        }
    }
}