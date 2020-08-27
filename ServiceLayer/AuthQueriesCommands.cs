using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AuthQueriesCommands
    {
        public bool IsAccountExist(string email)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                if(db.Accounts.Any(account=>String.Equals(account.Email, email, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Account GetAccountByEmail(string email)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Accounts.Where(account => account.Email == email.ToLower().Trim()).FirstOrDefault();
            }
        }

        public Account GetAccountById(Guid id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Accounts.Find(id);
            }
        }

        public int? GetNormalUserRoleId()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var UserRole = db.Roles.Where(role => String.Equals(role.Role_Name, "User", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                return UserRole.Id;
            }
        }

        public int Register(Account account)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return 1;

                }catch
                {
                    return 0;
                }
            }
        }

        public Account LogInAccount(string email, string password)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Accounts.Where(account =>account.Email==email && account.Password == password).SingleOrDefault();
            }
        }

        public int ChangePassword(Account accountDetails)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var result = accountDetails;
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public List<Account> GetAllAccounts()
        {
            using(DatabaseContext db=new DatabaseContext())
            {
                return db.Accounts.ToList();
            }
        }

        public int DeleteAccount(Account account)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    account.Account_Status = 0;
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
