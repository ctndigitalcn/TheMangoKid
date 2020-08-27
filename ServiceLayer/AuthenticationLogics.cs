using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServiceLayer
{
    public partial class BusinessLogics
    {
        GeneralLogics logic;

        //Sign Up process
        public int SignUp(string firstName,string lastName, string email, string mobile, string address, string password)
        {
            Account acc = new Account();
            AuthQueriesCommands auth = new AuthQueriesCommands();

            if (auth.IsAccountExist(email))
            {
                //Duplicate record found
                return 0;
            }
            else
            {
                logic = new GeneralLogics();

                Guid NewAccountId = logic.CreateUniqueId();
                acc.Id = NewAccountId;
                acc.Email = email.ToLower().Trim();
                acc.Account_Role = auth.GetNormalUserRoleId();
                acc.Password = password;
                acc.Account_Status = 1;
                acc.Account_Creation_Date = logic.CurrentIndianTime();

                acc.UserDetail.User_Account_Id = NewAccountId;
                acc.UserDetail.User_Address = address;
                acc.UserDetail.User_Mobile_Number = mobile;
                acc.UserDetail.User_First_Name = firstName;
                acc.UserDetail.User_Last_Name = lastName;

                var result = auth.Register(acc);
                if (result == 0)
                {
                    //Exception has been thrown
                    return 2;
                }
                else
                {
                    //saved successfully
                    return 1;
                }
                
            }
        }

        //Login process
        public Account Login(string email, string password)
        {
            AuthQueriesCommands auth = new AuthQueriesCommands();

            var result = auth.LogInAccount(email.ToLower().Trim(), password);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public Account FindAccountByEmail(string email)
        {
            AuthQueriesCommands auth = new AuthQueriesCommands();

            return auth.GetAccountByEmail(email);
        }

        public int ChangePassword(Account accountDetails)
        {
            AuthQueriesCommands auth = new AuthQueriesCommands();
            //0 for failure 1, for success
            return auth.ChangePassword(accountDetails);
        }

        public List<Account> GetAllAccounts()
        {
            AuthQueriesCommands authCQ = new AuthQueriesCommands();

            return authCQ.GetAllAccounts();
        }

        public int DeleteAccount(Guid id)
        {
            AuthQueriesCommands authCQ = new AuthQueriesCommands();

            var result = authCQ.GetAccountById(id);
            if (result != null)
            {
                var newResult = authCQ.DeleteAccount(result);
                if (newResult == 1)
                {
                    //Account active status has been changed successfully
                    return 1;
                }
                else
                {
                    //Exception has been thrown
                    return 2;
                }
            }
            else
            {
                //No account found with the account Id provided
                return 0;
            }
        }
        public int DeleteAccount(string email)
        {
            AuthQueriesCommands authCQ = new AuthQueriesCommands();

            var result = authCQ.GetAccountByEmail(email);
            if (result != null)
            {
                var newResult = authCQ.DeleteAccount(result);
                if (newResult == 1)
                {
                    //Account active status has been changed successfully
                    return 1;
                }
                else
                {
                    //Exception has been thrown
                    return 2;
                }
            }
            else
            {
                //No account found with the email provided
                return 0;
            }
        }
    }
}
