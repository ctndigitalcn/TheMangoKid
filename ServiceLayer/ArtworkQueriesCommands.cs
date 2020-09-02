using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ArtworkQueriesCommands
    {
        public int AddArtWork(ArtworkDetail artworkObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.ArtworkDetails.Add(artworkObject);
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    //internal error occured
                    return 0;
                }
            }
        }

        public ArtworkDetail GetArtworkByLink(string linkOfArtwork)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.ArtworkDetails.Where(rec => rec.Artwork_Link == linkOfArtwork).FirstOrDefault();
            }
        }
    }
}
