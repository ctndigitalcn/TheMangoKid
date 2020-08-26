using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApp.Controllers
{
    public class AuthenticationController : Controller
    {
        BusinessLogics businessLogics;
        GeneralLogics logics;

        // GET: Authentication
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(string first_name, string last_name, string mobile, string email, string address1, string address2, string pincode, string password, string con_password)
        {
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            List<string> inputValues = new List<string> { first_name, last_name, mobile, email, address1, address2, pincode, password, con_password };

            if (logics.ContainsAnyNullorWhiteSpace(inputValues))
            {
                ViewBag.ErrorMsg = "No Field Should be left blank";
            }
            else
            {
                if(!logics.ContainsOnlyDigits(mobile) && mobile.Length!=10 && pincode.Length!=6 && !logics.ContainsOnlyDigits(pincode))
                {
                    ViewBag.ErrorMsg = "Mobile or Pincode invalid ";
                }
                else if(!logics.ValidEmail(email))
                {
                    ViewBag.ErrorMsg = "Invalid Email provided";
                }
                else
                {
                    if(!logics.ContainsOnlyAlphabets(first_name) && !logics.ContainsOnlyAlphabets(last_name))
                    {
                        ViewBag.ErrorMsg = "First name and Last name is invalid";
                    }
                    else
                    {
                        if (password.Length < 6)
                        {
                            ViewBag.ErrorMsg = "Password length must be of minimum 6";
                        }
                        else
                        {
                            if (password != con_password)
                            {
                                ViewBag.ErrorMsg = "Password and Confirm paswword field must contain same value";
                            }
                            else
                            {
                                string Address = address1 + ", " + address2 + ", " + pincode;
                                var account = businessLogics.SignUp(first_name, last_name, email, mobile, Address, password);
                                if (account == null)
                                {
                                    ViewBag.ErrorMsg = "Error occoured while creating Account. This may be because of the provided email already exists or internal server error occured.";
                                }
                                else
                                {
                                    return RedirectToAction("Login", "Authentication");
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            if (logics.ValidEmail(email) && !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(password) && password.Length<6)
            {
                ViewBag.ErrorMsg = "Invalid Email input";
            }
            else
            {
                var result = businessLogics.Login(email, password);
                if (result != null)
                {
                    //FormsAuthentication.SetAuthCookie(result.Email.ToString(), false);
                    FormsAuthenticationTicket Authticket = new FormsAuthenticationTicket(
                                                                1,
                                                                result.Email.ToString() + ",",
                                                                DateTime.Now,
                                                                DateTime.Now.AddMinutes(120),
                                                                false,
                                                                result.UserDetail.User_First_Name.ToString() + 
                                                                ","+result.Role.Role_Name.ToString());
                    string hash = FormsAuthentication.Encrypt(Authticket);

                    HttpCookie Authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                    if (Authticket.IsPersistent)
                        Authcookie.Expires = Authticket.Expiration;

                    Response.Cookies.Add(Authcookie);

                    Session["LoginEmail"] = result.Email;

                    return RedirectToAction("Index", "UserProfile");
                }
                else
                {
                    ViewBag.ErrorMsg = "No account found with the credential provided";
                }
                
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult PasswordReset(string email, string oldPassword, string newPassword)
        {
            logics = new GeneralLogics();
            businessLogics = new BusinessLogics();

            var result = businessLogics.FindAccountByEmail(email);

            if (result != null)
            {
                if (result.Password == oldPassword)
                {
                    if (oldPassword == newPassword)
                    {
                        ViewBag.ErrorMsg = "Old Password can't be your new password";
                    }
                    else
                    {
                        result.Password = newPassword;
                        var newResult = businessLogics.ChangePassword(result);
                        if (newResult != null)
                        {
                            return RedirectToAction("Logout", "Authentication");
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Error occured while changing Password";
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Please enter your valid old Password";
                }
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}