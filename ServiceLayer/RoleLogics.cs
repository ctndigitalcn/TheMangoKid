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
        public int Addrole(string roleName)
        {
            RoleQueriesCommands roleCQ = new RoleQueriesCommands();
            

            if (roleCQ.IsRoleExists(roleName))
            {
                //Duplicate record found
                return 0;
            }
            else
            {
                Role role = new Role();
                role.Role_Name = roleName.ToLower();
                var result = roleCQ.AddRole(role);
                if (result == 0 )
                {
                    //Exception has been thrown
                    return 2;
                }
                else
                {
                    //saved Successfully
                    return 1;
                }
            }
        }

        public int EditRole(int id, string roleName)
        {
            RoleQueriesCommands roleCQ = new RoleQueriesCommands();
            if (roleCQ.IsRoleExists(id))
            {
                if (roleCQ.IsRoleExists(roleName.ToLower().Trim()))
                {
                    //same role exists in the database
                    return 3;
                }
                Role role = new Role();
                role.Id = id;
                role.Role_Name = roleName.ToLower().Trim();
                var result = roleCQ.EditRole(role);
                if (result == 0)
                {
                    //Exception Occured while changing database record
                    return 2;
                }
                else
                {
                    //Record changes successfully
                    return 1;
                }
            }
            else
            {
                //Record does not found to make changes
                return 0;
            }
        }

        public int DeleteRole(int id)
        {
            RoleQueriesCommands roleCQ = new RoleQueriesCommands();

            if (roleCQ.IsRoleExists(id))
            {
                Role role = roleCQ.GetRoleById(id);

                var result = roleCQ.DeleteRole(role);
                if (result == 1)
                {
                    //Successfully removed role and deactivated the accounts associated with
                    return 1;
                }
                else if(result == 2)
                {
                    //Internal error occured. Accounts could not be deactivated.
                    return 2;
                }
                else
                {
                    //Internal error occured. Role deletion unsuccessfull.
                    return 3;
                }
            }
            else
            {
                //Provided role doesn't exists.
                return 0;
            }
        }

        public List<Role> GetAllRoles()
        {
            RoleQueriesCommands RoleCQ = new RoleQueriesCommands();
            var result = RoleCQ.getRoles();
            return result;
        }

        public Role GetRole(int id)
        {
            RoleQueriesCommands RoleCQ = new RoleQueriesCommands();
            var result = RoleCQ.GetRoleById(id);
            return result;
        }
    }
}
