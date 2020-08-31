using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class BankController : Controller
    {
        BusinessLogics businessLogics;
        GeneralLogics logics;

        // GET: Bank
        [HttpGet]
        public ActionResult AddBankDetails()
        {
            if (Session["LoginEmail"] == null)
            {
                return RedirectToAction("Logout", "Authentication");
            }
            ViewBag.Title = "Add Bank Detail";
            return View("AddorEditBankDetails");
        }

        [HttpGet]
        public ActionResult EditBankDetails(string userEmail)
        {
            ViewBag.Title = "Edit Bank Detail";
            return View("AddorEditBankDetails");
        }

        [HttpPost]
        public ActionResult AddBankDetails(string payee_first_name, string payee_last_name, string payee_bank_name, string payee_bank_account, string payee_bank_ifsc, string payee_bank_branch)
        {
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            string email = Session["LoginEmail"].ToString();
            //string email = "koushik.official1999@gmail.com";
            var result = businessLogics.FindAccountByEmail(email);

            if (result != null)
            {
                List<string> inputDetails = new List<string> { payee_first_name, payee_last_name, payee_bank_name, email, payee_bank_account, payee_bank_ifsc, payee_bank_branch };

                if (logics.ContainsAnyNullorWhiteSpace(inputDetails))
                {
                    ViewBag.ErrorMsg = "No Field Should be left blank";
                }
                if (!logics.ContainsOnlyDigits(payee_bank_account))
                {
                    ViewBag.ErrorMsg = "Invalid bank account number";
                }
                if (payee_bank_ifsc.Length != 11 && !logics.ContainsOnlyAlphabets(payee_bank_ifsc.Substring(0, 4)))
                {
                    ViewBag.ErrorMsg = "Invalid bank IFSC number";
                }
                if (!logics.ContainsOnlyAlphabets(payee_first_name) && !logics.ContainsOnlyAlphabets(payee_last_name))
                {
                    ViewBag.ErrorMsg = "Invalid Payee name invalid";
                }

                var bankaccountCreationResult = businessLogics.AddBankDetails(result.Id, payee_first_name, payee_last_name, payee_bank_name, payee_bank_account, payee_bank_ifsc, payee_bank_branch);

                if (bankaccountCreationResult == 1)
                {
                    //Bank details created successfully
                    return RedirectToAction("Index", "UserProfile");

                }
                //else if (bankaccountCreationResult == 2)
                //{
                //    ViewBag.ErrorMsg = "Internal server error occured while inserting data to the database";
                //}
                else
                {
                    ViewBag.ErrorMsg = "Internal server error occured while inserting data to the database";
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Account information retreval failed";
            }

            return View();
        }

    }
}