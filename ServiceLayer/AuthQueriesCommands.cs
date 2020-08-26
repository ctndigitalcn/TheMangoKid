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

        public int? GetNormalUserRoleId()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var UserRole = db.Roles.Where(role => String.Equals(role.Role_Name, "User", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                return UserRole.Id;
            }
        }

        public Account Register(Account account)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Accounts.Add(account);
                    return account;

                }catch(Exception ex)
                {
                    return null;
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
    }
}
