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
                var result = AlbumCQ.DeleteAlbum(albumObject);
                if (result == 0)
                {
                    //Operation Faild while deleting album. Internal error occured;
                    return 2;
                }else if (result == 1)
                {
                    //Operation completed Successfully
                    return 1;
                }else if (result == 2)
                {
                    //A Record Couldn't deleted from AlbumTrackMaster Table due to internal error.
                    return 3;
                }
                else if (result == 3)
                {
                    //Operation Failed in the level of AlbumTrackMaster Label.
                    return 4;
                }
                else if (result == 4)
                {
                    //Error while deleting a solo track that belongs to only the album
                    return 5;
                }
                else if (result == 5)
                {
                    //Track fetching failed.
                    return 6;
                }
                else
                {
                    //Error occured while deleting a valid track
                    return 7;
                }
            }
            else
            {
                //No Album found. Operation failed.
                return 0;
            }
        }

        public int EditAlbum(Guid albumId, string albumName, int totalTrack)
        {
            AlbumQueriesCommands albumCQ = new AlbumQueriesCommands();

            Album album = albumCQ.GetAlbumById(albumId);

            if (album != null)
            {
                album.Album_Name = albumName;
                album.Total_Track = totalTrack;

                var result = albumCQ.EditAlbumDetails(album);

                if (result == 0)
                {
                    //Internal Error occured while changing data for the album
                    return 2;
                }else if (result == 1)
                {
                    //Album details changed Successfully
                    return 1;
                }
                else
                {
                    //Can't edit album as one song alredy registered under the album
                    return 3;
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
    }
}
