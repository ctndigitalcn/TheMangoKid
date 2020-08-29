using DataAccess;
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
        public int AddBankDetails(Guid accountId, string payee_first_name, string payee_last_name, string payee_bank_name, string payee_bank_account, string payee_bank_ifsc, string payee_bank_branch)
        {
            BankQueriesCommands BankCQ = new BankQueriesCommands();
            BankDetail bd = new BankDetail();

            bd.Id = logic.CreateUniqueId();
            bd.Account_Id = accountId;
            bd.PayeeFirstName = payee_first_name;
            bd.PayeeLastName = payee_last_name;
            bd.PayeeBankName = payee_bank_name;
            bd.PayeeBankAccountNumber = payee_bank_account;
            bd.PayeeBankIfscNumber = payee_bank_ifsc;
            bd.PayeeBankBranch = payee_bank_branch;
            bd.Detail_Submitted_At = logic.CurrentIndianTime();

            var result = BankCQ.AddBankDetails(bd);

            if (result == 1)
            {
                //Operation completed successfully
                return 1;
            }
            else
            {
                //Error occured while adding the data in the database
                return 0;
            }
        }

        public bool IsBankDetailsExistsOf(string userEmail)
        {
            BankQueriesCommands BankCQ = new BankQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            return BankCQ.IsBankDetailsExistsOf(AuthCQ.GetAccountByEmail(userEmail)); 
        }

        public int EditBankDetails(string userEmail, string payee_first_name, string payee_last_name, string payee_bank_name, string payee_bank_account, string payee_bank_ifsc, string payee_bank_branch)
        {
            //Code to change the bank details submitted by user
            BankQueriesCommands BankCQ = new BankQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            var account = AuthCQ.GetAccountByEmail(userEmail);

            if (account != null)
            {
                if (IsBankDetailsExistsOf(userEmail))
                {
                    var bankDetails = BankCQ.GetBankDetailOf(account);
                    if (bankDetails != null)
                    {
                        bankDetails.Account_Id = account.Id;
                        bankDetails.PayeeBankAccountNumber = payee_bank_account;
                        bankDetails.PayeeBankName = payee_bank_name;
                        bankDetails.PayeeBankIfscNumber = payee_bank_ifsc;
                        bankDetails.PayeeBankBranch = payee_bank_branch;
                        bankDetails.PayeeFirstName = payee_first_name;
                        bankDetails.PayeeLastName = payee_last_name;
                        bankDetails.Detail_Submitted_At = logic.CurrentIndianTime();

                        var result = BankCQ.EditBankDetails(bankDetails);

                        if (result == 1)
                        {
                            //Changes done successfully
                            return 1;
                        }
                        else
                        {
                            //Internal error occured while changing bank detais
                            return 4;
                        }
                    }
                    else
                    {
                        //No bank details is associated with the account
                        return 3;
                    }
                    
                }
                else
                {
                    //No bank details found for this account
                    return 2;
                }
            }
            else
            {
                //No account found with this email id
                return 0;
            }
        }

        public BankDetail GetBankDetailOf(string userEmail)
        {
            BankQueriesCommands BankCQ = new BankQueriesCommands();
            AuthQueriesCommands AuthCQ = new AuthQueriesCommands();

            var accountObject = AuthCQ.GetAccountByEmail(userEmail);

            if (accountObject != null)
            {
                //Account found now returning the bank details
                return BankCQ.GetBankDetailOf(accountObject);
            }
            else
            {
                //No Account found with this email
                return null;
            }
            
        }
    }
}
