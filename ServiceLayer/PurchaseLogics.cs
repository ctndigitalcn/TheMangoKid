using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
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

        public int PurchaseEpFor(string userEmail)
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
                pr.Purchased_Category = "ep";
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

        public int PurchaseSoloFor(string userEmail)
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
                pr.Purchased_Category = "solo";
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
                //Account not found
                return 2;
            }
        }

        public bool IsAccountContainsThisPurchase(string email, Guid purchaseId)
        {
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();
            var accountObject = AuthCQ.GetAccountByEmail(email);
            if (accountObject != null)
            {
                PurchaseRecordQueriesCommands prCQ = new PurchaseRecordQueriesCommands();
                return prCQ.GetAllPurchaseRecordsOf(accountObject).Any(rec=>rec.Id== purchaseId);
            }
            else
            {
                //No account found with this email
                return false;
            }
        }

        public bool IsPurchaseExpired(Guid purchaseId)
        {
            logic = new GeneralLogics();
            PurchaseRecordQueriesCommands prCQ = new PurchaseRecordQueriesCommands();
            return prCQ.IsPurchaseExpired(purchaseId, logic.CurrentIndianTime());
        }

    }
}
