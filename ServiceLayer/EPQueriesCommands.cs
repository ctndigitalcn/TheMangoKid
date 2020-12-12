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
    public class EPQueriesCommands
    {
        public int CreateEP(ExtendedPlay epObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.ExtendedPlays.Add(epObject);
                    db.SaveChanges();
                    //Successfully created EP;
                    return 1;
                }
                catch
                {
                    //Internal error occured while creating Ep.
                    return 0;
                }
            }
        }

        public int EditEpDetails(ExtendedPlay epObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Entry(epObject).State = EntityState.Modified;
                    db.SaveChanges();
                    //Ep details changed Successfully
                    return 1;
                }
                catch
                {
                    //Internal Error occured while changing data for the Ep
                    return 0;
                }
            }
        }

        public int EditAlbumDetailsByUser(ExtendedPlay epObject)
        {
            if (EPEmptiness(epObject) != 1)
            {
                //Can't edit Ep as one song alredy registered under the Ep
                return 2;
            }
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Entry(epObject).State = EntityState.Modified;
                    db.SaveChanges();
                    //Ep details changed Successfully
                    return 1;
                }
                catch
                {
                    //Internal Error occured while changing data for the album
                    return 0;
                }
            }
        }

        public int DeleteEp(ExtendedPlay epObject)
        {
            if (IsAnyTrackOfTheEpSubmitted(epObject))
            {
                //Can't delete Ep as one of the song from the album already submitted to the store.
                return 0;
            }
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    Guid? purchaseId = epObject.PurchaseTrack_RefNo;

                    db.Entry(epObject).State = EntityState.Deleted;
                    db.SaveChanges();
                    try
                    {
                        var purchaseRecord = db.PurchaseRecords.Find(purchaseId);
                        if (purchaseRecord != null)
                        {
                            purchaseRecord.Usage_Date = null;
                            purchaseRecord.Usage_Exp_Date = null;
                            db.Entry(purchaseRecord).State = EntityState.Modified;
                            db.SaveChanges();
                            return 1;
                        }
                        else
                        {
                            //Error while updating the associated purchase record. User can't create an Ep with the valid purchase
                            return 4;
                        }
                    }
                    catch
                    {
                        return 3;
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.ToString();

                    //Operation Faild while deleting album. Internal error occured;
                    return 2;
                }
            }
        }

        public int EPEmptiness(ExtendedPlay epObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var result = db.ExtendedPlays.Find(epObject.Id);
                if (result != null)
                {
                    if (result.Submitted_Track == 0)
                    {
                        //EP is totally empty
                        return 1;
                    }
                    else if (result.Submitted_Track > 0 && result.Submitted_Track != result.Total_Track)
                    {
                        //EP is partially empty
                        return 2;
                    }
                    else
                    {
                        //EP is full
                        return 3;
                    }
                }
                else
                {
                    //EP not found
                    return 2;
                }
            }
        }

        public bool IsAnyTrackOfTheEpSubmitted(ExtendedPlay epObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.EpTrackMasters.Where(rec => rec.Ep_Id == epObject.Id).Any(rec => rec.StoreSubmissionStatus == 1);
            }
        }

        public List<ExtendedPlay> GetAllEps()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.ToList();
            }
        }

        public List<ExtendedPlay> GetAllEpsOf(Account accountObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Where(ep => ep.PurchaseRecord.Account.Email == accountObject.Email.ToLower()).Include(rec=>rec.PurchaseRecord).ToList();
            }
        }

        public List<ExtendedPlay> GetAllEpsBetween(DateTime startDate, DateTime endDate)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Where(ep => (ep.Ep_Creation_Date > startDate) && (ep.Ep_Creation_Date < endDate)).ToList();
            }
        }
        public List<ExtendedPlay> GetAllEpsBetweenDesc(DateTime startDate, DateTime endDate)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Where(ep => (ep.Ep_Creation_Date > startDate) && (ep.Ep_Creation_Date < endDate)).OrderByDescending(rec => rec.Ep_Creation_Date).ToList();
            }
        }

        public List<ExtendedPlay> GetAllEpsOf(DateTime startCreationDate, DateTime endCreationDate, Account accountObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Where(ep => (ep.Ep_Creation_Date > startCreationDate) && (ep.Ep_Creation_Date < endCreationDate) && (ep.PurchaseRecord.Account == accountObject)).ToList();
            }
        }
        public List<ExtendedPlay> GetAllEpsOfDesc(DateTime startCreationDate, DateTime endCreationDate, Account accountObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Where(ep => (ep.Ep_Creation_Date > startCreationDate) && (ep.Ep_Creation_Date < endCreationDate) && (ep.PurchaseRecord.Account == accountObject)).OrderByDescending(rec => rec.Ep_Creation_Date).ToList();
            }
        }

        public List<SingleTrackDetail> GetAllTracksOfEp(Guid epId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.EpTrackMasters.Where(ep => ep.Ep_Id == epId).Select(ep => ep.SingleTrackDetail).ToList();
            }
        }

        public ExtendedPlay GetEpById(Guid epId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Where(rec => rec.Id == epId).Include(rec => rec.PurchaseRecord).SingleOrDefault();
            }
        }
        public ExtendedPlay GetEpByPurchaseRecord(PurchaseRecord purchaseRecordObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Where(rec => rec.PurchaseRecord == purchaseRecordObject).SingleOrDefault();
            }
        }

        public bool IfEpAlreadyExistsWith(PurchaseRecord purchaseRecordObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.ExtendedPlays.Any(rec => rec.PurchaseRecord == purchaseRecordObject);
            }
        }

        public int RemoveSingleTracksFromEp(List<SingleTrackDetail> soloTrackList)
        {
            using (DatabaseContext db = new DatabaseContext())
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

        public List<EpTrackMaster> GetAllTracksWithEpDetails(Guid epId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.EpTrackMasters.Where(rec => rec.Ep_Id == epId).Include(rec => rec.SingleTrackDetail).ToList();
            }
        }

        public List<EpTrackMaster> GetAllEpsWithTrackDetail()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.EpTrackMasters.Include(rec => rec.SingleTrackDetail).Include(rec=>rec.ExtendedPlay).ToList();
            }
        }

        public int UpdateStoreSubmissionStatus(Guid epId, Guid trackId, int statusCode)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var data = db.EpTrackMasters.Where(rec => rec.Ep_Id == epId && rec.Track_Id == trackId).SingleOrDefault();
                    data.StoreSubmissionStatus = statusCode;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    //Failed to update the status
                    return 0;
                }
            }
        }
    }
}
