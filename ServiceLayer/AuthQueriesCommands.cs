using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                if(db.Accounts.Any(account=>account.Email==email.ToLower()))
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
                var UserRole = db.Roles.Where(role => role.Role_Name == "user").SingleOrDefault();
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
        public int Register(UserDetail userDetailObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.UserDetails.Add(userDetailObject);
                    db.SaveChanges();
                    return 1;

                }
                catch
                {
                    return 0;
                }
            }
        }

        public Account LogInAccount(string email, string password)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Accounts.Where(account =>account.Email==email && account.Password == password && account.Account_Status==1).Include(rec=>rec.UserDetail).SingleOrDefault();
            }
        }

        public int ChangePassword(Account accountObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Entry(accountObject).State = EntityState.Modified;
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

        public int DeleteAccount(Account accountObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    accountObject.Account_Status = 0;
                    db.Entry(accountObject).State = EntityState.Modified;
                    db.SaveChanges();
                    //Successfully deactivated Account
                    return 1;
                }
                catch
                {
                    //Error occured while deactivating account
                    return 0;
                }
            }
        }

        public int RemoveAccountPermanantly(Account accountObject)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Accounts.Remove(accountObject);
                    db.SaveChanges();
                    //Permanantly removed Account
                    return 1;
                }
                catch
                {
                    //Account removal failed
                    return 0;
                }
            }
        }

        public UserDetail GetUserDetailsOf(Account accountObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.UserDetails.Find(accountObject.Id);
            }
        }

        public bool IsSuperAdmin(string email)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.Accounts.Any(account => account.Email == email.Trim().ToLower() && account.Role.Role_Name == "superadmin");
            }
        }
    }
}
