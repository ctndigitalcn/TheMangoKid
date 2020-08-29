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
        public int PurchaseAlbumFor(string userEmail)
        {
            PurchaseRecordQueriesCommands PrCQ = new PurchaseRecordQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();
            var account = AuthCQ.GetAccountByEmail(userEmail);

            if (account != null)
            {
                logic = new GeneralLogics();
                var purchaseId = logic.CreateUniqueId();

                PurchaseRecord pr = new PurchaseRecord();

                pr.Id = purchaseId;
                pr.Purchased_Category = "album";
                pr.Account_Id = account.Id;
                pr.PurchaseDate = logic.CurrentIndianTime();

                var result = PrCQ.AddPurchaseRecord(pr);

                if (result == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 2;
            }
        }
    }
}
