using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
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

        public int EditAlbumDetailsByUser(Album albumObject)
        {
            if (AlbumEmptiness(albumObject) != 1)
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
                return 0;
            }
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    Guid? purchaseId = albumObject.PurchaseTrack_RefNo;

                    db.Entry(albumObject).State = EntityState.Deleted;
                        db.SaveChanges();
                        try
                        {
                            var purchaseRecord = db.PurchaseRecords.Find(purchaseId);
                            if (purchaseRecord != null)
                            {
                                purchaseRecord.Usage_Date = null;
                                db.Entry(purchaseRecord).State = EntityState.Modified;
                                db.SaveChanges();
                                return 1;
                            }
                            else
                            {
                                //Error while updating the associated purchase record. User can't create an album with the valid purchase
                                return 4;
                            }
                        }
                        catch
                        {
                            return 3;
                        }
                }
                catch(Exception ex)
                {
                    string msg = ex.ToString();
                    
                    //Operation Faild while deleting album. Internal error occured;
                    return 2;
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
                    }else if(result.Submitted_Track>0 && result.Submitted_Track != result.Total_Track)
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

        public List<AlbumTrackMaster> GetAllTracksWithAlbumDetails(Guid albumId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.AlbumTrackMasters.Where(rec => rec.Album_Id == albumId).Include(rec => rec.SingleTrackDetail).ToList();
            }
        }

        public Album GetAlbumById(Guid? albumId)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.Albums.Where(rec=>rec.Id==albumId).Include(rec=>rec.PurchaseRecord).SingleOrDefault();
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

        public AlbumTrackMaster GetAlbumTrackObject(Guid albumId, Guid trackId)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.AlbumTrackMasters.Where(rec => rec.Album_Id == albumId && rec.Track_Id == trackId).Include(rec=>rec.Album).Include(rec=>rec.SingleTrackDetail).SingleOrDefault();
            }
        }

        public int RemoveSingleTracksFromAlbum(List<SingleTrackDetail> soloTrackList)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    foreach (var item in soloTrackList)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
