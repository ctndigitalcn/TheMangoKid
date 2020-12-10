using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public partial class BusinessLogics
    {
        public int CreateNewTrackForAlbum(Guid albumId, string TrackTitle, string ArtistName, bool ArtistAlreadyInSpotify, string ArtistSpotifyUrl, DateTime ReleaseDate, string Genre, string CopyrightClaimerName, string AuthorName, string ComposerName, string ArrangerName, string ProducerName, bool AlreadyHaveAnISRC, string ISRC_Number, int PriceTier, bool ExplicitContent, bool IsTrackInstrumental, string LyricsLanguage, string TrackZipFileLink, string ArtWork_Link)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();
            PurchaseRecordQueriesCommands PurchaseCQ = new PurchaseRecordQueriesCommands();
            logic = new GeneralLogics();

            var albumObject = AlbumCQ.GetAlbumById(albumId);

            if (albumObject != null)
            {
                if (PurchaseCQ.GetPurchaseRecordById(albumObject.PurchaseTrack_RefNo).Usage_Exp_Date < logic.CurrentIndianTime())
                {
                    //Can't add more track in the album as purchase expired
                    return 7;
                }

                if (albumObject.Total_Track <= albumObject.Submitted_Track)
                {
                    //can't add more track in the album as the album is full
                    return 8;
                }

                byte ArtistSpotifyAppearance = 1;
                byte PresenceOfISRCnumber = 1;
                byte PresenceOfExplicitContent = 1;
                byte InstrumentalTrackPresence = 1;

                if (ArtistAlreadyInSpotify == false)
                {
                    ArtistSpotifyAppearance = 0;
                }
                if (AlreadyHaveAnISRC == false)
                {
                    PresenceOfISRCnumber = 0;
                }
                if (IsTrackInstrumental == false)
                {
                    InstrumentalTrackPresence = 0;
                }
                if (ExplicitContent == false)
                {
                    PresenceOfExplicitContent = 0;
                }
                SingleTrackDetail std = new SingleTrackDetail();

                std.Id = logic.CreateUniqueId();
                std.TrackTitle = TrackTitle;
                std.ArtistName = ArtistName;
                std.ArtistAlreadyInSpotify = ArtistSpotifyAppearance;
                std.ArtistSpotifyUrl = ArtistSpotifyUrl;
                std.ReleaseDate = ReleaseDate;
                std.Genre = Genre;
                std.CopyrightClaimerName = CopyrightClaimerName;
                std.AuthorName = AuthorName;
                std.ComposerName = ComposerName;
                std.ArrangerName = ArrangerName;
                std.ProducerName = ProducerName;
                std.AlreadyHaveAnISRC = PresenceOfISRCnumber;
                std.ISRC_Number = ISRC_Number;
                std.PriceTier = PriceTier;
                std.ExplicitContent = PresenceOfExplicitContent;
                std.IsTrackInstrumental = InstrumentalTrackPresence;
                std.LyricsLanguage = LyricsLanguage;
                std.TrackZipFileLink = TrackZipFileLink;
                std.ArtworkFileLink = ArtWork_Link.Trim();

                TrackQueriesCommands TrackCQ = new TrackQueriesCommands();
                //add single track
                var singleTrackSaveResult = TrackCQ.AddTrack(std);

                if (singleTrackSaveResult == 1)
                {
                   //Link track with the album. Work on albumtrackMaster table
                   AlbumTrackMaster atm = new AlbumTrackMaster();

                   atm.Id = logic.CreateUniqueId();
                   atm.Album_Id = albumId;
                   atm.Track_Id = std.Id;
                   atm.Submitted_At = logic.CurrentIndianTime();
                    //Status = pending
                   atm.StoreSubmissionStatus = 2;

                   var atmSaveResult = TrackCQ.AddtoAlbumTrackMaster(atm);

                   if (atmSaveResult == 1)
                   {
                       //increment the number of the submitted track for the album
                       albumObject.Submitted_Track = albumObject.Submitted_Track + 1;

                       var albumEditResult = AlbumCQ.EditAlbumDetails(albumObject);

                       if (albumEditResult == 1)
                       {
                           //if it's the first track of the album then set purchase record usage expire time
                           albumObject = AlbumCQ.GetAlbumById(albumId);
                           if (albumObject != null)
                           {
                               //if (albumObject.Submitted_Track == 1)
                               //{
                                    //PurchaseRecord pr = PurchaseCQ.GetPurchaseRecordById(albumObject.PurchaseTrack_RefNo);
                                    //if (pr != null)
                                    //{
                                    //     pr.Usage_Exp_Date = logic.CurrentIndianTime().AddHours(24);
                                    //     var purchaseEditResult = PurchaseCQ.UpdatePurchaseRecord(pr);
                                    //     if (purchaseEditResult == 1)
                                    //     {
                                    //          //purchase expire date set
                                    //          return 1;
                                    //     }
                                    //     else
                                    //     {
                                    //         //Error while setting the expire date
                                    //         return 11;
                                    //     }
                                    //}
                                    //else
                                    //{
                                    //    //error while fetching the purchase record of the album
                                    //    return 10;
                                    //}
                               //}
                               return 1;
                           }
                           else
                           {
                               //error while fetching the album
                               return 9;
                           }
                       }
                       else
                       {
                           //Error occured while updating album record
                           return 6;
                       }
                    }
                    else
                    {
                        //Error occured while adding albumTrackMaster record
                        return 5;
                    }
                }
                else
                {
                     //Error occured while saving single track to the database
                     return 4;
                }
                    
            }
            else
            {
                //No album found with the provided Album Id
                return 0;
            }
        }

        public int CreateNewTrackForEp(Guid epId, string TrackTitle, string ArtistName, bool ArtistAlreadyInSpotify, string ArtistSpotifyUrl, DateTime ReleaseDate, string Genre, string CopyrightClaimerName, string AuthorName, string ComposerName, string ArrangerName, string ProducerName, bool AlreadyHaveAnISRC, string ISRC_Number, int PriceTier, bool ExplicitContent, bool IsTrackInstrumental, string LyricsLanguage, string TrackZipFileLink, string ArtWork_Link)
        {
            EPQueriesCommands EpCQ = new EPQueriesCommands();
            PurchaseRecordQueriesCommands PurchaseCQ = new PurchaseRecordQueriesCommands();
            logic = new GeneralLogics();

            var epObject = EpCQ.GetEpById(epId);

            if (epObject != null)
            {
                if (PurchaseCQ.GetPurchaseRecordById(epObject.PurchaseTrack_RefNo).Usage_Exp_Date < logic.CurrentIndianTime())
                {
                    //Can't add more track in the album as purchase expired
                    return 7;
                }

                if (epObject.Total_Track <= epObject.Submitted_Track)
                {
                    //can't add more track in the album as the album is full
                    return 8;
                }

                byte ArtistSpotifyAppearance = 1;
                byte PresenceOfISRCnumber = 1;
                byte PresenceOfExplicitContent = 1;
                byte InstrumentalTrackPresence = 1;

                if (ArtistAlreadyInSpotify == false)
                {
                    ArtistSpotifyAppearance = 0;
                }
                if (AlreadyHaveAnISRC == false)
                {
                    PresenceOfISRCnumber = 0;
                }
                if (IsTrackInstrumental == false)
                {
                    InstrumentalTrackPresence = 0;
                }
                if (ExplicitContent == false)
                {
                    PresenceOfExplicitContent = 0;
                }
                SingleTrackDetail std = new SingleTrackDetail();

                std.Id = logic.CreateUniqueId();
                std.TrackTitle = TrackTitle;
                std.ArtistName = ArtistName;
                std.ArtistAlreadyInSpotify = ArtistSpotifyAppearance;
                std.ArtistSpotifyUrl = ArtistSpotifyUrl;
                std.ReleaseDate = ReleaseDate;
                std.Genre = Genre;
                std.CopyrightClaimerName = CopyrightClaimerName;
                std.AuthorName = AuthorName;
                std.ComposerName = ComposerName;
                std.ArrangerName = ArrangerName;
                std.ProducerName = ProducerName;
                std.AlreadyHaveAnISRC = PresenceOfISRCnumber;
                std.ISRC_Number = ISRC_Number;
                std.PriceTier = PriceTier;
                std.ExplicitContent = PresenceOfExplicitContent;
                std.IsTrackInstrumental = InstrumentalTrackPresence;
                std.LyricsLanguage = LyricsLanguage;
                std.TrackZipFileLink = TrackZipFileLink;
                std.ArtworkFileLink = ArtWork_Link.Trim();

                TrackQueriesCommands TrackCQ = new TrackQueriesCommands();
                //add single track
                var singleTrackSaveResult = TrackCQ.AddTrack(std);

                if (singleTrackSaveResult == 1)
                {
                    //Link track with the album. Work on albumtrackMaster table
                    EpTrackMaster etm = new EpTrackMaster();

                    etm.Id = logic.CreateUniqueId();
                    etm.Ep_Id = epId;
                    etm.Track_Id = std.Id;
                    etm.Submitted_At = logic.CurrentIndianTime();
                    //Status = pending
                    etm.StoreSubmissionStatus = 2;

                    var etmSaveResult = TrackCQ.AddtoEpTrackMaster(etm);

                    if (etmSaveResult == 1)
                    {
                        //increment the number of the submitted track for the album
                        epObject.Submitted_Track = epObject.Submitted_Track + 1;

                        var epEditResult = EpCQ.EditEpDetails(epObject);

                        if (epEditResult == 1)
                        {
                            //if it's the first track of the album then set purchase record usage expire time
                            epObject = EpCQ.GetEpById(epId);
                            if (epObject != null)
                            {
                                if (epObject.Submitted_Track == 1)
                                {
                                    PurchaseRecord pr = PurchaseCQ.GetPurchaseRecordById(epObject.PurchaseTrack_RefNo);
                                    if (pr != null)
                                    {
                                        pr.Usage_Exp_Date = logic.CurrentIndianTime().AddHours(24);
                                        var purchaseEditResult = PurchaseCQ.UpdatePurchaseRecord(pr);
                                        if (purchaseEditResult == 1)
                                        {
                                            //purchase expire date set
                                            return 1;
                                        }
                                        else
                                        {
                                            //Error while setting the expire date
                                            return 11;
                                        }
                                    }
                                    else
                                    {
                                        //error while fetching the purchase record of the album
                                        return 10;
                                    }
                                }
                                return 1;
                            }
                            else
                            {
                                //error while fetching the album
                                return 9;
                            }
                        }
                        else
                        {
                            //Error occured while updating album record
                            return 6;
                        }
                    }
                    else
                    {
                        //Error occured while adding albumTrackMaster record
                        return 5;
                    }
                }
                else
                {
                    //Error occured while saving single track to the database
                    return 4;
                }

            }
            else
            {
                //No album found with the provided Album Id
                return 0;
            }
        }

        public int CreateNewTrackForSolo(Guid purchaseId, string TrackTitle, string ArtistName, bool ArtistAlreadyInSpotify, string ArtistSpotifyUrl, DateTime ReleaseDate, string Genre, string CopyrightClaimerName, string AuthorName, string ComposerName, string ArrangerName, string ProducerName, bool AlreadyHaveAnISRC, string ISRC_Number, int PriceTier, bool ExplicitContent, bool IsTrackInstrumental, string LyricsLanguage, string TrackZipFileLink, string ArtWork_Link)
        {
            byte ArtistSpotifyAppearance = 1;
            byte PresenceOfISRCnumber = 1;
            byte PresenceOfExplicitContent = 1;
            byte InstrumentalTrackPresence = 1;

            if (ArtistAlreadyInSpotify == false)
            {
                ArtistSpotifyAppearance = 0;
            }
            if (AlreadyHaveAnISRC == false)
            {
                PresenceOfISRCnumber = 0;
            }
            if (IsTrackInstrumental == false)
            {
                InstrumentalTrackPresence = 0;
            }
            if (ExplicitContent == false)
            {
                PresenceOfExplicitContent = 0;
            }
            SingleTrackDetail std = new SingleTrackDetail();
            GeneralLogics logic = new GeneralLogics();
            std.Id = logic.CreateUniqueId();
            std.TrackTitle = TrackTitle;
            std.ArtistName = ArtistName;
            std.ArtistAlreadyInSpotify = ArtistSpotifyAppearance;
            std.ArtistSpotifyUrl = ArtistSpotifyUrl;
            std.ReleaseDate = ReleaseDate;
            std.Genre = Genre;
            std.CopyrightClaimerName = CopyrightClaimerName;
            std.AuthorName = AuthorName;
            std.ComposerName = ComposerName;
            std.ArrangerName = ArrangerName;
            std.ProducerName = ProducerName;
            std.AlreadyHaveAnISRC = PresenceOfISRCnumber;
            std.ISRC_Number = ISRC_Number;
            std.PriceTier = PriceTier;
            std.ExplicitContent = PresenceOfExplicitContent;
            std.IsTrackInstrumental = InstrumentalTrackPresence;
            std.LyricsLanguage = LyricsLanguage;
            std.TrackZipFileLink = TrackZipFileLink;
            std.ArtworkFileLink = ArtWork_Link.Trim();

            TrackQueriesCommands TrackCQ = new TrackQueriesCommands();
            //add single track
            var singleTrackSaveResult = TrackCQ.AddTrack(std);

            if (singleTrackSaveResult == 1)
            {
                //Link track with the SoloMaster table. Work on albumtrackMaster table
                SoloTrackMaster stm = new SoloTrackMaster();

                stm.Id = logic.CreateUniqueId();
                stm.Track_Id = std.Id;
                stm.PurchaseTrack_RefNo = purchaseId;
                stm.Submitted_At = logic.CurrentIndianTime();
                //Status = pending
                stm.StoreSubmissionStatus = 2;

                var stmSaveResult = TrackCQ.AddtoSoloTrackMaster(stm);

                if (stmSaveResult == 1)
                {
                    PurchaseRecordQueriesCommands PurchaseCQ = new PurchaseRecordQueriesCommands();
                    PurchaseRecord pr = PurchaseCQ.GetPurchaseRecordById(purchaseId);
                    if (pr != null)
                    {
                        pr.Usage_Date = logic.CurrentIndianTime();
                        pr.Usage_Exp_Date = logic.CurrentIndianTime().AddHours(24);
                        var purchaseEditResult = PurchaseCQ.UpdatePurchaseRecord(pr);
                        if (purchaseEditResult == 1)
                        {
                            //purchase expire date set
                            return 1;
                        }
                        else
                        {
                            //Error while setting the expire date
                            return 4;
                        }
                    }
                    else
                    {
                        //error while fetching the purchase record of the album
                        return 3;
                    }

                }
                else
                {
                    //Error occured while adding soloTrackMaster record
                    return 2;
                }
            }
            else
            {
                //Error occured while saving single track to the database
                return 0;
            }
        }

        public SingleTrackDetail GetTrackById(Guid trackId)
        {
            TrackQueriesCommands TrackCQ = new TrackQueriesCommands();

            return TrackCQ.FindTrackById(trackId);
        }

        public int EditTrack(Guid trackId, string TrackTitle, string ArtistName, bool ArtistAlreadyInSpotify, string ArtistSpotifyUrl, DateTime ReleaseDate, string Genre, string CopyrightClaimerName, string AuthorName, string ComposerName, string ArrangerName, string ProducerName, bool AlreadyHaveAnISRC, string ISRC_Number, int PriceTier, bool ExplicitContent, bool IsTrackInstrumental, string LyricsLanguage, string TrackZipFileLink, string ArtWork_Link)
        {
            byte ArtistSpotifyAppearance = 1;
            byte PresenceOfISRCnumber = 1;
            byte PresenceOfExplicitContent = 1;
            byte InstrumentalTrackPresence = 1;

            if (ArtistAlreadyInSpotify == false)
            {
                ArtistSpotifyAppearance = 0;
            }
            if (AlreadyHaveAnISRC == false)
            {
                PresenceOfISRCnumber = 0;
            }
            if (IsTrackInstrumental == false)
            {
                InstrumentalTrackPresence = 0;
            }
            if (ExplicitContent == false)
            {
                PresenceOfExplicitContent = 0;
            }

            SingleTrackDetail std = new SingleTrackDetail();
            std.Id = trackId;
            std.TrackTitle = TrackTitle;
            std.ArtistName = ArtistName;
            std.ArtistAlreadyInSpotify = ArtistSpotifyAppearance;
            std.ArtistSpotifyUrl = ArtistSpotifyUrl;
            std.ReleaseDate = ReleaseDate;
            std.Genre = Genre;
            std.CopyrightClaimerName = CopyrightClaimerName;
            std.AuthorName = AuthorName;
            std.ComposerName = ComposerName;
            std.ArrangerName = ArrangerName;
            std.ProducerName = ProducerName;
            std.AlreadyHaveAnISRC = PresenceOfISRCnumber;
            std.ISRC_Number = ISRC_Number;
            std.PriceTier = PriceTier;
            std.ExplicitContent = PresenceOfExplicitContent;
            std.IsTrackInstrumental = InstrumentalTrackPresence;
            std.LyricsLanguage = LyricsLanguage;
            std.TrackZipFileLink = TrackZipFileLink;
            std.ArtworkFileLink = ArtWork_Link.Trim();

            TrackQueriesCommands TrackCQ = new TrackQueriesCommands();
            //Edit single track
            var singleTrackEditResult = TrackCQ.UpdateTrack(std);

            if (singleTrackEditResult == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int DeleteTrack(Guid trackId)
        {
            TrackQueriesCommands TrackCQ = new TrackQueriesCommands();
            var track = TrackCQ.FindTrackById(trackId);
            if (track != null)
            {
                int trackDeleteResult = TrackCQ.DeleteTrack(track);
                if (trackDeleteResult == 1)
                {
                    //Operation done successfully
                    return 1;
                }
                else
                {
                    //Internal error
                    return 0;
                }
            }
            else
            {
                //track fetching failed
                return 3;
            }
        }

        //For solo music logic
        public int CountOfSolosAlreadyCreatedBy(string userEmail)
        {
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(userEmail);
            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUsedSoloPurchaseRecordOf(account);

                //Returning the count of the unused purchase of Eps for the user
                return GetListOfUnUsedPurchase.Count;
            }
            else
            {
                //No Account Found
                return 0;
            }
        }
        public int CountOfSolosCanBeCreatedBy(string userEmail)
        {
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(userEmail);
            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUnUsedSoloPurchaseRecordOf(account);

                //Returning the count of the unused purchase of Eps for the user
                return GetListOfUnUsedPurchase.Count;
            }
            else
            {
                //No Account Found
                return 0;
            }
        }
        public List<SoloTrackMaster> GetAllTheSolosOf(string email)
        {
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();
            SoloQueriesCommands soloCQ = new SoloQueriesCommands();

            return soloCQ.GetAllSolosOf(AuthCQ.GetAccountByEmail(email));
        }
        public bool IsAccountContainsThisSolo(string email, Guid soloId)
        {
            SoloQueriesCommands soloCQ = new SoloQueriesCommands();
            AuthQueriesCommands authCQ = new AuthQueriesCommands();
            var account = authCQ.GetAccountByEmail(email.ToLower());
            if (account == null)
            {
                return false;
            }
            var solos = soloCQ.GetAllSolosOf(account);
            if (solos.Count > 0)
            {
                if (solos.Any(rec => rec.SingleTrackDetail.Id == soloId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int UpdateISRCNumberOfTrack(Guid trackId, string trackISRCnumber)
        {
            var trackDetail = GetTrackById(trackId);
            if (trackDetail != null)
            {
                trackDetail.ISRC_Number = trackISRCnumber;
                TrackQueriesCommands trackCQ = new TrackQueriesCommands();
                int updateTrackResult = trackCQ.UpdateTrack(trackDetail);
                if (updateTrackResult == 1)
                {
                    //Successfully ISRC number assigned
                    return 1;
                }
                else
                {
                    //Failed to assign ISRC number for the track
                    return 0;
                }
            }
            else
            {
                //Track doesn't exist
                return 0;
            }
        }

        public List<SoloTrackMaster> GetAllSolosWithTracks()
        {
            SoloQueriesCommands soloCQ = new SoloQueriesCommands();
            return soloCQ.GetAllSolosWithTrackDetail();
        }
    }
}
