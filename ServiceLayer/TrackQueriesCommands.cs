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
    public class TrackQueriesCommands
    {
        public int AddTrack(SingleTrackDetail trackObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.SingleTrackDetails.Add(trackObject);
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    //Internal error occured while adding the track
                    return 0;
                }
            }
        }

        public int AddtoAlbumTrackMaster(AlbumTrackMaster atmObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.AlbumTrackMasters.Add(atmObject);
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    //internal error occured while adding track to album track master table
                    return 0;
                }
            }
        }

        public int AddtoEpTrackMaster(EpTrackMaster etmObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.EpTrackMasters.Add(etmObject);
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    //internal error occured while adding track to album track master table
                    return 0;
                }
            }
        }

        public int UpdateTrack(SingleTrackDetail trackObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Entry(trackObject).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    //internal error occured while updating data of the track
                    return 0;
                }
            }
        }

        public int DeleteTrack(SingleTrackDetail trackObject)
        {
            using(DatabaseContext db=new DatabaseContext())
            {
                try
                {
                    db.Entry(trackObject).State = EntityState.Deleted;
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public SingleTrackDetail FindTrackById(Guid trackId)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.SingleTrackDetails.Where(rec=>rec.Id==trackId).SingleOrDefault();
            }
        }

    }
}
