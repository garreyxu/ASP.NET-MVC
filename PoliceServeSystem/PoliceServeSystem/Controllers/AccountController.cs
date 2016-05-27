using System;
using System.Web;
using System.Web.Mvc;
using PoliceServeSystem.DAL;
using PoliceServeSystem.Models;
using System.Web.Script.Serialization;
using System.Web.Security;
using PoliceServeSystem.Helper;

namespace PoliceServeSystem.Controllers
{
    public class AccountController : Controller
    {
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (model.UserName == null || model.UserName.Trim() == "")
            {
                ModelState.AddModelError("", "Enter Username");
                return View(model);
            }
            if (model.Password == null || model.Password.Trim() == "")
            {
                ModelState.AddModelError("", "Enter Password");
                return View(model);
            }

            UsersDal usersdalfile = new UsersDal();
            var user = usersdalfile.Load(model.UserName, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is invalid");
                return View(model);
            }

            try
            {
                if (Tools.GetUserId != "0")
                {
                    if (Tools.IsAgencyExpired == 1)
                    {
                        ModelState.AddModelError("",
                            "Your licence is about to expire, please contact Palatine administrator 800-610-7897.");
                        return View(model);
                    }
                    else if (Tools.IsAgencyExpired == 2)
                    {
                        ModelState.AddModelError("",
                            "Your licence is about to expire, please contact Palatine administrator 800-610-7897.");
                        return View(model);
                    }
                    if (Tools.UserActive == 0)
                    {
                        ModelState.AddModelError("", "Inactive Login.");
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            return RedirectToAction("Index", "Served");
        }

        public JsonResult GetNotificationType()
        {
            var user = new Users {Userid = Tools.GetUserId};
            return Json(user, JsonRequestBehavior.AllowGet);
        }
    }
}
