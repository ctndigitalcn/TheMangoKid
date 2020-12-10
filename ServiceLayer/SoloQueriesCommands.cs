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
    public class SoloQueriesCommands
    {
        public List<SoloTrackMaster> GetAllSolosOf(Account account)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.SoloTrackMasters.Where(rec => rec.PurchaseRecord.Account_Id == account.Id && rec.PurchaseRecord.Purchased_Category.Equals("Solo", StringComparison.CurrentCultureIgnoreCase)).Include(rec => rec.SingleTrackDetail).ToList();
            }
        }

        public List<SoloTrackMaster> GetAllSolosWithTrackDetail()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.SoloTrackMasters.Include(rec => rec.SingleTrackDetail).ToList();
            }
        }
    }

}
