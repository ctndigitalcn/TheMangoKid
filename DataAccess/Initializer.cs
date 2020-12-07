using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            var roles = new List<Role>
            {
            new Role{Role_Name="superadmin"},
            new Role{Role_Name="admin"},
            new Role{Role_Name="user"}
            };

            roles.ForEach(s => context.Roles.Add(s));
            context.SaveChanges();
        }
    }
}
