using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class SiteRoleManagementController : Controller
    {
        BusinessLogics businessLogics;
        GeneralLogics logics;

        // GET: SiteRoleManagement
        public ActionResult Index()
        {
            businessLogics = new BusinessLogics();
            ViewBag.Roles = businessLogics.GetAllRoles();
            return View();
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            ViewBag.Title = "Add Role";
            return View("AddorEditRole");
        }

        [HttpPost]
        public ActionResult AddRole(string roleName)
        {
            logics = new GeneralLogics();

            if (logics.ContainsOnlyAlphabets(roleName))
            {
                businessLogics = new BusinessLogics();
                var result = businessLogics.Addrole(roleName);
                if (result == 1)
                {
                    return RedirectToAction("Index", "SiteRoleManagement");
                }else if (result == 0)
                {
                    ViewBag.ErrorMsg = "Role already exists.";
                }
                else
                {
                    ViewBag.ErrorMsg = "Internal Error occured while adding new role";
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Invalid Role name entered";
            }
            return View("AddorEditRole");
        }

        [HttpGet]
        public ActionResult EditRole(int? id)
        {
            businessLogics = new BusinessLogics();
            if (id != null)
            {
                ViewBag.Title = "Edit Role";
                var result = businessLogics.GetRole((Int32)id);

                if (result != null)
                {
                    ViewBag.RoleId = result.Id;
                    ViewBag.RoleName = result.Role_Name;
                }
                else
                {
                    ViewBag.ErrorMsg = "No role found with the Id provided";
                }
            }
            else
            {
                ViewBag.Title = "Url Error";
            }
            

            return View("AddorEditRole");
        }
        
        [HttpPost]
        public ActionResult EditRole(int roleId, string roleName)
        {
            logics = new GeneralLogics();
            if (logics.ContainsOnlyAlphabets(roleName))
            {
                businessLogics = new BusinessLogics();
                var result = businessLogics.EditRole(roleId, roleName);
                if (result == 1)
                {
                    return RedirectToAction("Index", "SiteRoleManagement");
                }
                else if (result == 0)
                {
                    ViewBag.ErrorMsg = "No role found with the associated Id.";
                }
                else if (result == 3)
                {
                    ViewBag.ErrorMsg = "Role already exists";
                }
                else
                {
                    ViewBag.ErrorMsg = "Internal Error occured while modifying role.";
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Invalid Role name entered";
            }
            return RedirectToAction("Index", "SiteRoleManagement");
        }

        [HttpGet]
        public ActionResult DeleteRole(int roleId)
        {
            businessLogics = new BusinessLogics();

            var result = businessLogics.DeleteRole(roleId);

            if (result == 1)
            {
                return RedirectToAction("Index", "SiteRoleManagement");

            } else if (result == 0)
            {
                ViewBag.ErrorMsg = "Role Doesn't Exists";
            }
            else if (result == 2)
            {
                ViewBag.ErrorMsg = "Associated accounts with the role is still active";
            }
            else
            {
                ViewBag.ErrorMsg = "Internal Error occured while deleting the role";
            }
            return RedirectToAction("Index", "SiteRoleManagement");
        }
    }
}