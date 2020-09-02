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
        public int CreateNewEp(string email, string epName, int totalTrack)
        {
            logic = new GeneralLogics();
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            EPQueriesCommands EpCQ = new EPQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(email);

            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUnUsedEpPurchaseRecordOf(account);
                if (GetListOfUnUsedPurchase.Count > 0)
                {
                    ExtendedPlay ep = new ExtendedPlay();

                    ep.Id = logic.CreateUniqueId();
                    ep.Ep_Name = epName;
                    ep.Total_Track = totalTrack;
                    ep.Ep_Creation_Date = logic.CurrentIndianTime();
                    ep.Submitted_Track = 0;
                    ep.PurchaseTrack_RefNo = GetListOfUnUsedPurchase.First().Id;

                    var resultCreateEp = EpCQ.CreateEP(ep);
                    if (resultCreateEp == 1)
                    {
                        var purchaseRecord = purchaseCQ.GetPurchaseRecordById(ep.PurchaseTrack_RefNo);

                        purchaseRecord.Usage_Date = logic.CurrentIndianTime();
                        int resultPurchaseRecordUpdate = purchaseCQ.UpdatePurchaseRecord(purchaseRecord);

                        if (resultPurchaseRecordUpdate == 1)
                        {
                            //Ep created, PurchaseRecord is modified with UsageDate. Operation Completed successfully
                            return 1;
                        }
                        else
                        {
                            //Internal error occured while updating the record in PurchaseRecord table.Operation failed
                            return 4;
                        }
                    }
                    else
                    {
                        //Ep creation failed
                        return 3;
                    }
                }
                else
                {
                    //No purchase left to create an music Ep.
                    return 2;
                }
            }
            else
            {
                //No Account Found
                return 0;
            }
        }

        public int DeleteEp(Guid epId)
        {
            EPQueriesCommands EpCQ = new EPQueriesCommands();
            var epObject = EpCQ.GetEpById(epId);
            if (epObject != null)
            {
                var result = EpCQ.DeleteEp(epObject);
                if (result == 0)
                {
                    //Operation Faild while deleting Ep. Internal error occured;
                    return 2;
                }
                else if (result == 1)
                {
                    //Operation completed Successfully
                    return 1;
                }
                else if (result == 2)
                {
                    //A Record Couldn't deleted from EpTrackMaster Table due to internal error.
                    return 3;
                }
                else if (result == 3)
                {
                    //Operation Failed in the level of EpTrackMaster Label.
                    return 4;
                }
                else if (result == 4)
                {
                    //Error while deleting a solo track that belongs to only the Ep
                    return 5;
                }
                else if (result == 5)
                {
                    //Track fetching failed.
                    return 6;
                }
                else if (result == 7)
                {
                    //A track from the Ep is already submitted to the store. can't delete Ep
                    return 8;
                }
                else if (result == 8)
                {
                    //Error while updating the associated purchase record. User can't create an Ep with the valid purchase
                    return 9;
                }
                else
                {
                    //Error occured while deleting a valid track
                    return 7;
                }
            }
            else
            {
                //No Ep found. Operation failed.
                return 0;
            }
        }

        public int EditEp(Guid epId, string epName, int totalTrack)
        {
            EPQueriesCommands EpCQ = new EPQueriesCommands();

            ExtendedPlay ep = EpCQ.GetEpById(epId);

            if (ep != null)
            {
                ep.Ep_Name = epName;
                ep.Total_Track = totalTrack;

                var result = EpCQ.EditEpDetails(ep);

                if (result == 0)
                {
                    //Internal Error occured while changing data for the Ep
                    return 2;
                }
                else if (result == 1)
                {
                    //Ep details changed Successfully
                    return 1;
                }
                else
                {
                    //Can't edit Ep as one song alredy registered under the Ep
                    return 3;
                }
            }
            else
            {
                //No Ep found with the Id provided
                return 0;
            }
        }

        public int CountOfEpsCanBeCreatedBy(string email)
        {
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(email);
            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUnUsedEpPurchaseRecordOf(account);

                //Returning the count of the unused purchase of Eps for the user
                return GetListOfUnUsedPurchase.Count;
            }
            else
            {
                //No Account Found
                return 0;
            }
        }

        public int CountOfEpsAlreadyCreatedBy(string email)
        {
            PurchaseRecordQueriesCommands purchaseCQ = new PurchaseRecordQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            Account account = AuthCQ.GetAccountByEmail(email);
            if (account != null)
            {
                var GetListOfUnUsedPurchase = purchaseCQ.GetUsedEpPurchaseRecordOf(account);

                //Returning the count of the unused purchase of Eps for the user
                return GetListOfUnUsedPurchase.Count;
            }
            else
            {
                //No Account Found
                return 0;
            }
        }

        public List<ExtendedPlay> GetAllTheEpsOf(string email)
        {
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();
            EPQueriesCommands epCQ = new EPQueriesCommands();

            return epCQ.GetAllEpsOf(AuthCQ.GetAccountByEmail(email));
        }

        public ExtendedPlay GetEpById(Guid epId)
        {
            EPQueriesCommands epCQ = new EPQueriesCommands();
            return epCQ.GetEpById(epId);
        }

        public List<SingleTrackDetail> GetTrackDetailsOfEp(Guid epId)
        {
            EPQueriesCommands epCQ = new EPQueriesCommands();

            var result = epCQ.GetAllTracksOfEp(epId);

            //Result could be null or a list consists of Tracks
            return result;
        }

        public bool IsAccountContainsThisEp(string email, Guid epId)
        {
            EPQueriesCommands EpCQ = new EPQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            var account = AuthCQ.GetAccountByEmail(email.ToLower());
            if (account == null)
            {
                return false;
            }

            var Eps = EpCQ.GetAllEpsOf(account);
            if (Eps != null)
            {
                if (Eps.Any(rec=>rec.Id==epId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
