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
                    //Internal Error occured while changing data for the Ep
                    return 0;
                }
            }
        }

        public int DeleteEp(ExtendedPlay epObject)
        {
            if (IsAnyTrackOfTheEpSubmitted(epObject))
            {
                //Can't delete Ep as one of the song from the Ep already submitted to the store.
                return 7;
            }
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    if (db.EpTrackMasters.Any(rec => rec.Ep_Id == epObject.Id && rec.StoreSubmissionStatus == 1))
                    {
                        //A song of the store is already published. can't delete the Ep
                        return 7;
                    }

                    db.Entry(epObject).State = EntityState.Deleted;
                    db.SaveChanges();
                    try
                    {
                        var purchaseRecord = db.PurchaseRecords.Find(epObject.PurchaseTrack_RefNo);
                        if (purchaseRecord != null)
                        {
                            purchaseRecord.Usage_Date = null;
                            db.Entry(purchaseRecord).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            //Error while updating the associated purchase record. User can't create an ep with the valid purchase
                            return 8;
                        }
                        var result = db.EpTrackMasters.Where(res => res.Ep_Id == epObject.Id).ToList();
                        if (result.Count == 0)
                        {
                            //If ep has no track then operation successfull
                            return 1;
                        }
                        else
                        {
                            foreach (var item in result)
                            {
                                //Remove that record having the same ep name 
                                try
                                {
                                    db.Entry(item).State = EntityState.Deleted;
                                    db.SaveChanges();
                                    //If a track belongs to only that ep not in Extended play category and Solo category then delete that track
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
                                        //Error while deleting a solo track that belongs to only the ep
                                        return 4;
                                    }
                                }
                                catch
                                {
                                    //A Record Couldn't deleted from EPTrackMaster Table due to internal error.
                                    return 2;
                                }

                            }
                            //Operation completed Successfully
                            return 1;
                        }
                    }
                    catch
                    {
                        //Operation Failed in the level of EPTrackMaster Label.
                        return 3;
                    }
                }
                catch
                {
                    //Operation Faild while deleting ep. Internal error occured;
                    return 0;
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
                    else if (result.Submitted_Track >= 0 && result.Submitted_Track != result.Total_Track)
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
                return db.ExtendedPlays.Where(ep => ep.PurchaseRecord.Account.Email == accountObject.Email.ToLower()).ToList();
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
                return db.ExtendedPlays.Find(epId);
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
    }
}
