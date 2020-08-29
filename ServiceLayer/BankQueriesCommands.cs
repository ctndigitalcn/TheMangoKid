using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class BankQueriesCommands
    {
        public bool IsBankDetailsExistsOf(Account accountObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.BankDetails.Any(rec => rec.Account_Id == accountObject.Id);
            }
        }

        public int AddBankDetails(BankDetail bankDetailObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.BankDetails.Add(bankDetailObject);
                    db.SaveChanges();
                    //Operation completed successfully
                    return 1;
                }
                catch
                {
                    //Internal error occured while inserting the user bank details
                    return 0;
                }
            }
        }

        public int EditBankDetails(BankDetail bankDetailObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                    try
                    {
                        db.Entry(bankDetailObject).State = EntityState.Modified;
                        db.SaveChanges();
                        //Operation completed successfully
                        return 1;
                    }
                    catch
                    {
                        //Internal error occured while saving changes to the bank account details
                        return 0;
                    }
            }
        }

        public BankDetail GetBankDetailOf(Account accountObject)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.BankDetails.Where(rec => rec.Account_Id == accountObject.Id).SingleOrDefault();
            }
        }
    }
}
