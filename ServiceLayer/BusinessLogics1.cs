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
        public Account SignUp(string firstName,string lastName, string email, string mobile, string address, string password)
        {
            Account acc = new Account();
            AuthQueriesCommands auth = new AuthQueriesCommands();

            if (auth.IsAccountExist(email))
            {
                return null;
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
                if (result == null)
                {
                    return null;
                }
                else
                {
                    return result;
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

        public Account ChangePassword(Account accountDetails)
        {
            AuthQueriesCommands auth = new AuthQueriesCommands();
            return auth.ChangePassword(accountDetails);
        }
    }
}
