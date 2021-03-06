﻿using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class RoleQueriesCommands
    {
        public bool IsRoleExists(string RoleName)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.Roles.Any(role => role.Role_Name.ToLower() == RoleName.ToLower());
            }
        }

        public bool IsRoleExists(int roleId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Roles.Any(role => role.Id == roleId);
            }
        }

        public int AddRole(Role roleObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Roles.Add(roleObject);
                    db.SaveChanges();
                    return 1;
                }catch
                {
                    return 0;
                }
            }
        }

        public int EditRole(Role roleObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Entry(roleObject).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }catch(Exception ex)
                {
                    string error = ex.ToString();
                    return 0;
                }
                
            }
        }

        public int DeleteRole(Role roleObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    //Role is being removed
                    db.Entry(roleObject).State = EntityState.Deleted;
                    db.SaveChanges();

                    try
                    {
                        //Accounts with the roles removed recently will be de activated
                        var result = db.Accounts.Where(account => account.Account_Role == roleObject.Id).ToList();
                        if (result.Count == 0)
                        {
                            return 1;
                        }
                        else
                        {
                            foreach (var item in result)
                            {
                                item.Account_Status = 0;
                                db.SaveChanges();
                            }
                            return 1;
                        }
                    }catch
                    {
                        return 2;
                    }
                    
                }
                catch(Exception ex)
                {
                    string error = ex.ToString();
                    //Error occured while removing data from role table
                    return 0;
                }
            }
        }

        public List<Role> getRoles()
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.Roles.ToList();
            }
        }

        public Role GetRoleById(int id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Roles.Find(id);
            }
        }
    }
}
