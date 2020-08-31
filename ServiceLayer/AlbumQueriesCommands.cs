using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AlbumQueriesCommands
    {
        public int CreateAlbum(Album albumObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Albums.Add(albumObject);
                    db.SaveChanges();
                    //Successfully created album;
                    return 1;
                }
                catch
                {
                    //Internal error occured while creating album.
                    return 0;
                }
            }
        }

        public int EditAlbumDetails(Album albumObject)
        {
            if (AlbumEmptiness(albumObject)!=1)
            {
                //Can't edit album as one song alredy registered under the album
                return 2;
            }
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Entry(albumObject).State = EntityState.Modified;
                    db.SaveChanges();
                    //Album details changed Successfully
                    return 1;
                }
                catch
                {
                    //Internal Error occured while changing data for the album
                    return 0;
                }
            }
        }

        public int DeleteAlbum(Album albumObject)
        {
            if (IsAnyTrackOfTheAlbumSubmitted(albumObject))
            {
                //Can't delete album as one of the song from the album already submitted to the store.
                return 7;
            }
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    if(db.AlbumTrackMasters.Any(rec=>rec.Album_Id==albumObject.Id && rec.StoreSubmissionStatus == 1))
                    {
                        //A song of the store is already published. can't delete the album
                        return 7;
                    }

                    db.Entry(albumObject).State = EntityState.Deleted;
                    db.SaveChanges();
                    try
                    {
                        var purchaseRecord = db.PurchaseRecords.Find(albumObject.PurchaseTrack_RefNo);
                        if (purchaseRecord != null)
                        {
                            purchaseRecord.Usage_Date = null;
                            db.Entry(purchaseRecord).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            //Error while updating the associated purchase record. User can't create an album with the valid purchase
                            return 8;
                        }
                        var result = db.AlbumTrackMasters.Where(res => res.Album_Id == albumObject.Id).ToList();
                        if (result.Count == 0)
                        {
                            //If Album has no track then operation successfull
                            return 1;
                        }
                        else
                        {
                            foreach(var item in result)
                            {
                                //Remove that record having the same album name 
                                try
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                    db.SaveChanges();
                                    //If a track belongs to only that album not in Extended play category and Solo category then delete that track
                                    try
                                    {
                                        if (!db.EpTrackMasters.Any(track => track.Track_Id == item.Track_Id) && !db.SoloTrackMasters.Any(track => track.Track_Id == item.Track_Id))
                                        {
                                            var Track = db.SingleTrackDetails.Find(item.Track_Id);
                                            if (Track != null)
                                            {
                                                try
                                                {
                                                    db.Entry(Track).State = EntityState.Deleted;
                                                    db.SaveChanges();
                                                }
                                                catch
                                                {
                                                    //Error occured while deleting a valid track
                                                    return 6;
                                                }
                                                
                                            }
                                            else
                                            {
                                                //Track fetching failed.
                                                return 5;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        //Error while deleting a solo track that belongs to only the album
                                        return 4;
                                    }
                                }
                                catch
                                {
                                    //A Record Couldn't deleted from AlbumTrackMaster Table due to internal error.
                                    return 2;
                                }
                                
                            }
                            //Operation completed Successfully
                            return 1;
                        }
                    }
                    catch
                    {
                        //Operation Failed in the level of AlbumTrackMaster Label.
                        return 3;
                    }
                }
                catch
                {
                    //Operation Faild while deleting album. Internal error occured;
                    return 0;
                }
            }
        }

        public int AlbumEmptiness(Album albumObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                var result = db.Albums.Find(albumObject.Id);
                if (result != null)
                {
                    if (result.Submitted_Track == 0)
                    {
                        //Album is totally empty
                        return 1;
                    }else if(result.Submitted_Track>=0 && result.Submitted_Track != result.Total_Track)
                    {
                        //Album is partially empty
                        return 2;
                    }
                    else
                    {
                        //Album is full
                        return 3;
                    }
                }
                else
                {
                    //Album not found
                    return 2;
                }
            }
        }

        public bool IsAnyTrackOfTheAlbumSubmitted(Album albumObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.AlbumTrackMasters.Where(rec => rec.Album_Id == albumObject.Id).Any(rec => rec.StoreSubmissionStatus == 1);
            }
        }

        public List<Album> GetAllAlbums()
        {
            using(DatabaseContext db=new DatabaseContext())
            {
                return db.Albums.ToList();
            }
        }

        public List<Album> GetAllAlbumsOf(Account accountObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Where(album => album.PurchaseRecord.Account.Email == accountObject.Email.ToLower()).ToList();
            }        
        }

        public List<Album> GetAllAlbumsBetween(DateTime startDate, DateTime endDate)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Where(rec => (rec.Album_Creation_Date > startDate) && (rec.Album_Creation_Date < endDate)).ToList();
            }
        }
        public List<Album> GetAllAlbumsBetweenDesc(DateTime startDate, DateTime endDate)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Where(rec => (rec.Album_Creation_Date > startDate) && (rec.Album_Creation_Date < endDate)).OrderByDescending(rec=>rec.Album_Creation_Date).ToList();
            }
        }

        public List<Album> GetAllAlbumsOf(DateTime startCreationDate, DateTime endCreationDate, Account accountObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Where(rec => (rec.Album_Creation_Date > startCreationDate) && (rec.Album_Creation_Date < endCreationDate) && (rec.PurchaseRecord.Account == accountObject)).ToList();
            }
        }
        public List<Album> GetAllAlbumsOfDesc(DateTime startCreationDate, DateTime endCreationDate, Account accountObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Where(rec => (rec.Album_Creation_Date > startCreationDate) && (rec.Album_Creation_Date < endCreationDate) && (rec.PurchaseRecord.Account == accountObject)).OrderByDescending(rec=>rec.Album_Creation_Date).ToList();
            }
        }

        public List<SingleTrackDetail> GetAllTracksOfAlbum(Guid albumId)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.AlbumTrackMasters.Where(rec => rec.Album_Id == albumId).Select(rec=>rec.SingleTrackDetail).ToList();
            }
        }

        public Album GetAlbumById(Guid id)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Find(id);
            }
        }
        public Album GetAlbumByPurchaseRecord(PurchaseRecord purchaseRecordObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Where(rec => rec.PurchaseRecord == purchaseRecordObject).SingleOrDefault();
            }
        }

        public bool IfAlbumAlreadyExistsWith(PurchaseRecord purchaseRecordObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Any(rec=>rec.PurchaseRecord==purchaseRecordObject);
            }
        }
    }
}
