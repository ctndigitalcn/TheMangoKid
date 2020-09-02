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
    }
}
