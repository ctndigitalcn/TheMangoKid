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
        public int CreateNewAlbum(string email, string albumName, int totalTrack)
        {
            logic = new GeneralLogics();
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(email);

            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUnUsedAlbumPurchaseRecordOf(account);
                if (GetListOfUnUsedPurchase.Count > 0)
                {
                    Album album = new Album();

                    album.Id = logic.CreateUniqueId();
                    album.Album_Name = albumName;
                    album.Total_Track = totalTrack;
                    album.Album_Creation_Date = logic.CurrentIndianTime();
                    album.Submitted_Track = 0;
                    album.PurchaseTrack_RefNo = GetListOfUnUsedPurchase.First().Id;

                    var resultCreateAlbum = AlbumCQ.CreateAlbum(album);
                    if (resultCreateAlbum == 1)
                    {
                        var purchaseRecord = purchaseCQ.GetPurchaseRecordById(album.PurchaseTrack_RefNo);

                        purchaseRecord.Usage_Date = logic.CurrentIndianTime();
                        int resultPurchaseRecordUpdate = purchaseCQ.UpdatePurchaseRecord(purchaseRecord);

                        if (resultPurchaseRecordUpdate == 1)
                        {
                            //Album created, PurchaseRecord is modified with UsageDate. Operation Completed successfully
                            return 1;
                        }
                        else
                        {
                            //Internal error occured while updating the record in PurchaseRecord table.Operation failed
                            return 4;
                        }
                    }
                    else
                    {
                        //Album creation failed
                        return 3;
                    }
                }
                else
                {
                    //No purchase left to create an music album.
                    return 2;
                }
            }
            else
            {
                //No Account Found
                return 0;
            }
        }

        public int DeleteAlbum(Guid albumId)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();

            var albumObject = AlbumCQ.GetAlbumById(albumId);
            if (albumObject != null)
            {
                var trackListOftheAlbum = AlbumCQ.GetAllTracksOfAlbum(albumId);
                if (trackListOftheAlbum.Count == 0)
                {
                    var resultOfDeletingAlbum = AlbumCQ.DeleteAlbum(albumObject);
                    if (resultOfDeletingAlbum == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 5;
                    }
                }
                else if (trackListOftheAlbum.Count > 0)
                {
                    var resultOfRemovingSolos = AlbumCQ.RemoveSingleTracksFromAlbum(trackListOftheAlbum);
                    if (resultOfRemovingSolos == 1)
                    {
                        var resultOfDeletingAlbum = AlbumCQ.DeleteAlbum(albumObject);
                        if (resultOfDeletingAlbum == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            return 4;
                        }
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }

        public int EditAlbum(Guid albumId, string albumName, int totalTrack)
        {
            AlbumQueriesCommands albumCQ = new AlbumQueriesCommands();

            Album album = albumCQ.GetAlbumById(albumId);

            if (album != null)
            {
                if (albumCQ.AlbumEmptiness(album) != 1)
                {
                    //Can't edit album as one song alredy registered under the album
                    return 2;
                }
                album.Album_Name = albumName;
                album.Total_Track = totalTrack;

                var result = albumCQ.EditAlbumDetails(album);

                if (result != 1)
                {
                    //Internal Error occured while changing data for the album
                    return 3;
                }else
                {
                    //Album details changed Successfully
                    return 1;
                }
            }
            else
            {
                //No album found with the Id provided
                return 0;
            }
        }

        public int CountOfAlbumsCanBeCreatedBy(string email)
        {
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(email);
            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUnUsedAlbumPurchaseRecordOf(account);

                //Returning the count of the unused purchase of albums for the user
                return GetListOfUnUsedPurchase.Count;
            }
            else
            {
                //No Account Found
                return 0;
            }
        }

        public int CountOfAlbumsAlreadyCreatedBy(string email)
        {
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(email);
            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUsedAlbumPurchaseRecordOf(account);

                //Returning the count of the unused purchase of albums for the user
                return GetListOfUnUsedPurchase.Count;
            }
            else
            {
                //No Account Found
                return 0;
            }
        }

        public List<Album> GetAllTheAlbumsOf(string email)
        {
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();

            return AlbumCQ.GetAllAlbumsOf(AuthCQ.GetAccountByEmail(email));
        }

        public Album GetAlbumById(Guid albumId)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();
            return AlbumCQ.GetAlbumById(albumId);
        }

        public List<SingleTrackDetail> GetTrackDetailsOfAlbum(Guid albumId)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();

            var result = AlbumCQ.GetAllTracksOfAlbum(albumId);

            //Result could be null or a list consists of Tracks
            return result;
        }

        public List<AlbumTrackMaster> GetAllTracksWithAlbumDetails(Guid albumId)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();

            var result = AlbumCQ.GetAllTracksWithAlbumDetails(albumId);

            //Result could be null or a list consists of Tracks with store submission report and all.
            return result;
        }

        public bool IsAccountContainsThisAlbum(string email, Guid albumId)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            var account = AuthCQ.GetAccountByEmail(email.ToLower());
            if (account == null)
            {
                return false;
            }

            var albums = AlbumCQ.GetAllAlbumsOf(account);
            if (albums.Count > 0)
            {
                if (albums.Any(rec=>rec.Id == albumId))
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

        public bool IsAlbumFull(Guid albumId)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();
            if (AlbumCQ.AlbumEmptiness(AlbumCQ.GetAlbumById(albumId))==3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AlbumTrackMaster GetAlbumDetail(Guid albumId, Guid trackId)
        {
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();
            return AlbumCQ.GetAlbumTrackObject(albumId, trackId);
        }

        public bool IsAlbumExpired(Guid albumId)
        {
            logic = new GeneralLogics();
            AlbumQueriesCommands AlbumCQ = new AlbumQueriesCommands();
            if (AlbumCQ.GetAlbumById(albumId).PurchaseRecord.Usage_Exp_Date < logic.CurrentIndianTime())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<AlbumTrackMaster> GetAllAlbumsWithTracks()
        {
            AlbumQueriesCommands albumCQ = new AlbumQueriesCommands();
            return albumCQ.GetAllAlbumsWithTrackDetail();
        }


        public int UpdateStoreSubmissionStatusForAlbumTrack(Guid albumId, Guid trackId, int statusCode)
        {
            AlbumQueriesCommands albumCQ = new AlbumQueriesCommands();
            if (albumCQ.UpdateStoreSubmissionStatus(albumId, trackId, statusCode) == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
