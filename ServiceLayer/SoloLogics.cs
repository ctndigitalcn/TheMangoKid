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
                logic = new GeneralLogics();
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
                   atm.StoreSubmissionStatus = 0;

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
                               if (albumObject.Submitted_Track == 1)
                               {
                                   PurchaseRecord pr = PurchaseCQ.GetPurchaseRecordById(albumObject.PurchaseTrack_RefNo);
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
    }
}
