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
            UserDetail ud = new UserDetail();

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

                ud.User_Account_Id = NewAccountId;
                ud.User_Address = address;
                ud.User_Mobile_Number = mobile;
                ud.User_First_Name = firstName;
                ud.User_Last_Name = lastName;

                var result = auth.Register(acc);
                
                if (result == 0)
                {
                    //Account creation failed
                    return 2;
                }
                else
                {
                    //Account successfully created
                    result = auth.Register(ud);
                    if (result == 1)
                    {
                        //User details and account creation successfull
                        return 1;
                    }
                    else
                    {
                        //Remove recently created account as there is a problem while creating the user details for the same.
                        result = auth.RemoveAccountPermanantly(acc);
                        if (result == 1)
                        {
                            //Account deleted parmanantly
                            return 4;
                        }
                        else
                        {
                            //Account removal failed and user details could not be created
                            return 3;
                        }
                    }
                    
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
        
        public bool IsUserAdmin(string email)
        {
            AuthQueriesCommands authCQ = new AuthQueriesCommands();
            return authCQ.IsSuperAdmin(email);
        }
    }
}
