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
                var trackListOftheEp = EpCQ.GetAllTracksOfEp(epId);
                if (trackListOftheEp.Count == 0)
                {
                    var resultOfDeletingEp = EpCQ.DeleteEp(epObject);
                    if (resultOfDeletingEp == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 5;
                    }
                }
                else if (trackListOftheEp.Count > 0)
                {
                    var resultOfRemovingSolos = EpCQ.RemoveSingleTracksFromEp(trackListOftheEp);
                    if (resultOfRemovingSolos == 1)
                    {
                        var resultOfDeletingAlbum = EpCQ.DeleteEp(epObject);
                        if (resultOfDeletingAlbum == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            return 4;
                        }
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }

        public int EditEp(Guid epId, string epName, int totalTrack)
        {
            EPQueriesCommands EpCQ = new EPQueriesCommands();

            ExtendedPlay ep = EpCQ.GetEpById(epId);

            if (ep != null)
            {
                if (EpCQ.EPEmptiness(ep) != 1)
                {
                    //Can't edit Ep as one song alredy registered under the album
                    return 2;
                }
                ep.Ep_Name = epName;
                ep.Total_Track = totalTrack;

                var result = EpCQ.EditEpDetails(ep);

                if (result != 1)
                {
                    //Internal Error occured while changing data for the Ep
                    return 3;
                }
                else
                {
                    //Ep details changed Successfully
                    return 1;
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
        public List<EpTrackMaster> GetAllTracksWithEpDetails(Guid epId)
        {
            EPQueriesCommands EpCQ = new EPQueriesCommands();

            var result = EpCQ.GetAllTracksWithEpDetails(epId);

            //Result could be null or a list consists of Tracks with store submission report and all.
            return result;
        }

        public bool IsEpFull(Guid epId)
        {
            EPQueriesCommands EpCQ = new EPQueriesCommands();
            if (EpCQ.EPEmptiness(EpCQ.GetEpById(epId)) == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsEpExpired(Guid epId)
        {
            logic = new GeneralLogics();
            EPQueriesCommands EpCQ = new EPQueriesCommands();
            if (EpCQ.GetEpById(epId).PurchaseRecord.Usage_Exp_Date < logic.CurrentIndianTime())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<EpTrackMaster> GetAllEpsWithTracks()
        {
            EPQueriesCommands epCQ = new EPQueriesCommands();
            return epCQ.GetAllEpsWithTrackDetail();
        }

        public int UpdateStoreSubmissionStatusForEpTrack(Guid epId, Guid trackId, int statusCode)
        {
            EPQueriesCommands epCQ = new EPQueriesCommands();
            if (epCQ.UpdateStoreSubmissionStatus(epId, trackId, statusCode) == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
