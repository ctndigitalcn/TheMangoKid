using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //Role initializing
            var roles = new List<Role>
            {
            new Role{Role_Name="superadmin"},
            new Role{Role_Name="admin"},
            new Role{Role_Name="user"}
            };

            roles.ForEach(s => context.Roles.Add(s));
            context.SaveChanges();

            //First SuperAdmin Account Initializing
            if(context.Accounts.Any(account => account.Email == "superadmin@site.com"))
            {
                Account account = context.Accounts.Where(acc => acc.Email == "superadmin@site.com").FirstOrDefault();
                account.Account_Role = context.Roles.Where(role => role.Role_Name == "superadmin").FirstOrDefault().Id;
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                Account account = new Account();
                var newAccountId = Guid.NewGuid();
                account.Id = newAccountId;
                account.Email = "superadmin@site.com";
                account.Password = "Super@123";
                account.Account_Creation_Date = DateTime.Now;
                account.Account_Status = 1;
                account.Account_Role = context.Roles.Where(role => role.Role_Name == "superadmin").FirstOrDefault().Id;

                UserDetail ud = new UserDetail();
                ud.User_Account_Id = newAccountId;
                ud.User_First_Name = "Koushik";
                ud.User_Last_Name = "Saha";
                ud.User_Mobile_Number = "+917699978887";
                ud.User_Address = "N/A";

                context.Accounts.Add(account);
                context.SaveChanges();

                context.UserDetails.Add(ud);
                context.SaveChanges();
            }
        }
    }
}
