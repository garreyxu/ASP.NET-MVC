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

        public JsonResult LoginTemp(string loginId, string pwd)
        {
            if (loginId == null || loginId.Trim() == "")
            {
                ModelState.AddModelError("", "Enter Username");
                return Json("Enter Username", JsonRequestBehavior.AllowGet);
            }
            if (pwd == null || pwd.Trim() == "")
            {
                ModelState.AddModelError("", "Enter Password");
                return Json("Enter Password", JsonRequestBehavior.AllowGet);
            }

            //=================Form Authentication For User==========================
            Users user;
            NetGetUsersDal usersdalfile = new NetGetUsersDal();
            user = usersdalfile.GetUser(loginId, pwd);

            if (user == null)
            {
                return Json("Username or Password is invalid", JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string userJson = js.Serialize(user);
                FormsAuthenticationTicket userTicket = new FormsAuthenticationTicket(1,
                                                            loginId,
                                                            DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout),
                                                            true,
                                                            userJson);
                string encryptedTicket = FormsAuthentication.Encrypt(userTicket);

                HttpCookie ticketCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(ticketCookie);
            }
            catch (Exception)
            {
                return Json("Error in authentication", JsonRequestBehavior.AllowGet);
            }

            //if (Tools.GetUserId != 0)
            return Json(true, JsonRequestBehavior.AllowGet);
            //else
            //    return Json("Username or Password is invalid", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var userObj = new Users
            {
                Userid = Tools.GetUserId,
                NotificationType = model.NotificationType
            };

            UsersDal usersdalfile = new UsersDal();
            usersdalfile.UpdateNotificationType(userObj);

            userObj.LoginId = model.UserName;
            userObj.Password = model.Password;
            usersdalfile.Load(model.UserName, model.Password);

            //model.ModuleType = Tools.UserRole;

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
