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
            NetGetUsersDal usersdalfile = new NetGetUsersDal();
            var user = usersdalfile.GetUser(loginId, pwd);

            if (user.Userid == null)
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

        #region UserRole Check
        //AccessController.CurrentQuickEntryUser = userObj;
        //switch (Tools.UserRole.ToUpper())
        //{
        //    case "ADMIN":
        //        AccessController.CurrentUserRole = UserRole.Admin;
        //        Session["CurrentUserRole"] = UserRole.Admin;
        //        break;
        //    case "ADMINISTRATOR":
        //        AccessController.CurrentUserRole = UserRole.Admin;
        //        Session["CurrentUserRole"] = UserRole.Admin;
        //        break;
        //    case "PROSECUTOR":
        //        AccessController.CurrentUserRole = UserRole.Prosecutor;
        //        Session["CurrentUserRole"] = UserRole.Prosecutor;
        //        AccessController.CurrentSearchwarrantProsecutor = userObj;
        //        break;
        //    case "CLERK":
        //        AccessController.CurrentUserRole = UserRole.Clerk;
        //        Session["CurrentUserRole"] = UserRole.Clerk;
        //        break;
        //    case "JUDGE":
        //        AccessController.CurrentUserRole = UserRole.Judge;
        //        Session["CurrentUserRole"] = UserRole.Judge;
        //        AccessController.CurrentSearchwarrantJudge = userObj;
        //        break;
        //    case "JUDICIAL CLERK":
        //        AccessController.CurrentUserRole = UserRole.JudicialClerk;
        //        Session["CurrentUserRole"] = UserRole.JudicialClerk;
        //        break;
        //    case "MASTER":
        //        AccessController.CurrentUserRole = UserRole.Master;
        //        Session["CurrentUserRole"] = UserRole.Master;
        //        break;
        //    case "SUPERVISOR":
        //        AccessController.CurrentUserRole = UserRole.Supervisor;
        //        Session["CurrentUserRole"] = UserRole.Supervisor;
        //        break;
        //    case "JAILER":
        //        AccessController.CurrentUserRole = UserRole.Supervisor;
        //        Session["CurrentUserRole"] = UserRole.Supervisor;
        //        break;
        //    case "OUTSIDE AGENCY":
        //        AccessController.CurrentUserRole = UserRole.OutsideAgency;
        //        Session["CurrentUserRole"] = UserRole.OutsideAgency;
        //        break;
        //    case "PROSECUTING ATTORNEY":
        //        AccessController.CurrentUserRole = UserRole.ProsecutingAttorney;
        //        Session["CurrentUserRole"] = UserRole.ProsecutingAttorney;
        //        break;
        //    default:
        //        AccessController.CurrentUserRole = UserRole.None;
        //        Session["CurrentUserRole"] = UserRole.None;
        //        break;
        //}
        #endregion

        //try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = new AddressConfigurationBase_DAL().GetCountyState();
        //        if (dt.Rows.Count > 0)
        //        {
        //            Session["County"] = dt.Rows[0]["County"].ToString();
        //            Session["State"] = dt.Rows[0]["State"].ToString();
        //        }
        //        else
        //        {
        //            Session["County"] = ConfigurationSettingsReader.CurrentCounty.ToString();
        //            Session["State"] = ConfigurationSettingsReader.CurrentState.ToString();
        //        }

        //    }
        //    catch
        //    {
        //        Session["County"] = ConfigurationSettingsReader.CurrentCounty.ToString();
        //        Session["State"] = ConfigurationSettingsReader.CurrentState.ToString();
        //    }

        //Session["Role"] = Tools.UserRole;
        //Session["UserID"] = AccessController.CurrentQuickEntryUser.UserID;
        //Session["LoginID"] = AccessController.CurrentQuickEntryUser.LoginID;

        //if (Tools.UserRole == "CDCS" || Tools.UserRole == "Judge")
        //{
        //    Judge judge = new Judge();
        //    Judge_DAL judgedalfile = new Judge_DAL();
        //    judge = judgedalfile.AuthenticateJudge(model.UserName, CommonRoutines.encriptString(model.Password));
        //    if (judge.JudgeNO == "" || judge.Password != CommonRoutines.encriptString(model.Password))
        //    {
        //        ModelState.AddModelError("", "Invalid User.");
        //        return View(model);
        //    }
        //    else
        //    {
        //        AccessController.CurrentJudge = judge;
        //        AccessController.CurrentUserRole = UserRole.Judge;

        //        switch (judge.UserRole.ToUpper())
        //        {
        //            case "CLERK":
        //                AccessController.CurrentUserRole = UserRole.Clerk;
        //                Session["CurrentUserRole"] = UserRole.Clerk;
        //                break;
        //            case "JUDICIAL CLERK":
        //                AccessController.CurrentUserRole = UserRole.JudicialClerk;
        //                Session["CurrentUserRole"] = UserRole.JudicialClerk;
        //                break;
        //            case "JUDGE":
        //                AccessController.CurrentUserRole = UserRole.Judge;
        //                Session["CurrentUserRole"] = UserRole.Judge;
        //                AccessController.CurrentSearchwarrantJudge = userObj;
        //                break;
        //            case "ADMIN":
        //                AccessController.CurrentUserRole = UserRole.Admin;
        //                Session["CurrentUserRole"] = UserRole.Admin;

        //                break;
        //            case "MASTER":
        //                AccessController.CurrentUserRole = UserRole.Master;
        //                Session["CurrentUserRole"] = UserRole.Master;
        //                break;
        //            case "JAILER":
        //                AccessController.CurrentUserRole = UserRole.Supervisor;
        //                Session["CurrentUserRole"] = UserRole.Supervisor;

        //                break;
        //            case "SUPERVISOR":
        //                AccessController.CurrentUserRole = UserRole.Supervisor;
        //                Session["CurrentUserRole"] = UserRole.Supervisor;

        //                break;
        //            case "PROSECUTOR":
        //                AccessController.CurrentUserRole = UserRole.Prosecutor;
        //                Session["CurrentUserRole"] = UserRole.Prosecutor;
        //                AccessController.CurrentSearchwarrantProsecutor = userObj;
        //                break;
        //            case "OUTSIDE AGENCY":
        //                AccessController.CurrentUserRole = UserRole.OutsideAgency;
        //                Session["CurrentUserRole"] = UserRole.OutsideAgency;
        //                break;
        //            case "PROSECUTING ATTORNEY":
        //                AccessController.CurrentUserRole = UserRole.ProsecutingAttorney;
        //                Session["CurrentUserRole"] = UserRole.ProsecutingAttorney;
        //                break;
        //            default:
        //                AccessController.CurrentUserRole = UserRole.None;
        //                Session["CurrentUserRole"] = UserRole.None;

        //                break;
        //        }

        //        SaveLoggInfo(AccessController.CurrentQuickEntryUser.FirstName + " " + AccessController.CurrentQuickEntryUser.LastName + " with Login ID: " + AccessController.CurrentQuickEntryUser.LoginID + " is logged in into the system as " + AccessController.CurrentUserRole + " at " + DateTime.Now.ToString() + " in " + model.ModuleType, Convert.ToInt16(LoggerBase.LoggerType.Edit));

        //        if (Tools.UserRole.ToUpper() == "MASTER" || Tools.UserRole.ToUpper() == "CLERK" || Tools.UserRole.ToUpper() == "JUDICIAL CLERK" || Tools.UserRole.ToUpper() == "JUDGE" || Tools.UserRole.ToUpper() == "JAILER" || Tools.UserRole.ToUpper() == "SUPERVISOR")
        //        {
        //            if (CheckFirstlogin() == true)
        //            {
        //                return RedirectToAction("~/ChangePassword");
        //            }
        //            if (Tools.UserRole == "CDCS")
        //            {
        //                return RedirectToAction("~/FirstHearing/Default");
        //            }
        //            else
        //            {
        //                if (Tools.UserRole.ToUpper() != "JAILER")
        //                {
        //                    //return RedirectToAction("../Warrant/Warrants");
        //                    return RedirectToAction("ViewList", "Warrant", new { CaseNumber = "" });
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Invalid Login.");
        //                    return View(model);
        //                }
        //            }
        //            Session["Role"] = Tools.UserRole;
        //            Session["UserID"] = AccessController.CurrentQuickEntryUser.UserID;
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Invalid Login.");
        //            return View(model);
        //        }
        //    }

        //}
        //else if (Tools.UserRole == "Quick Entry" || Tools.UserRole == "Search Warrant" || Tools.UserRole == "Return Search Warrant")    //Select Module
        //{
        //    SaveLoggInfo(AccessController.CurrentQuickEntryUser.FirstName + " " + AccessController.CurrentQuickEntryUser.LastName + " with LoginID " + AccessController.CurrentQuickEntryUser.LoginID + " is logged in into the system as " + AccessController.CurrentUserRole + " at " + DateTime.Now.ToString() + " in " + model.ModuleType, Convert.ToInt16(LoggerBase.LoggerType.Edit));
        //    Session["LoginuserId"] = AccessController.CurrentQuickEntryUser.UserID;                                                                                                                      //Module Type
        //    if (Tools.UserRole.ToUpper() != "SUPERVISOR")
        //    {
        //        if (CheckFirstlogin() == true)
        //        {
        //            return RedirectToAction("~/ChangePassword");
        //        }
        //        if (Tools.UserRole == "Quick Entry")
        //        {   //Select Module
        //            if (Tools.UserRole.ToUpper() == "MASTER" || Tools.UserRole.ToUpper() == "CLERK" || Tools.UserRole.ToUpper() == "JUDICIAL CLERK" || Tools.UserRole.ToUpper() == "JUDGE" || Tools.UserRole.ToUpper() == "JAILER" || Tools.UserRole.ToUpper() == "SUPERVISOR" || Tools.UserRole.ToUpper() == "PROSECUTOR" || (Tools.UserRole.ToUpper() == "OUTSIDE AGENCY"))
        //            {
        //                return RedirectToAction("~/QuickEntry/QuickEntry");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Invalid Login.");
        //                return View(model);
        //            }
        //        }
        //        else if (Tools.UserRole == "Search Warrant")    //Select Module
        //        {
        //            if (Tools.UserRole.ToUpper() == "PROSECUTOR" || (Tools.UserRole.ToUpper() == "OUTSIDE AGENCY"))
        //            {
        //                return RedirectToAction("~/SearchWarrant/SearchWarrant");
        //            }
        //            else if (Tools.UserRole.ToUpper() == "MASTER" || Tools.UserRole.ToUpper() == "JUDGE")
        //            {
        //                return RedirectToAction("~/SearchWarrant/JudgeSearchWarrant");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Invalid Login.");
        //                return View(model);
        //            }
        //        }
        //        else
        //        {
        //            if (Tools.UserRole.ToUpper() != "")
        //            {
        //                string ReturnSearchWarrantMode = "0";
        //                if (Tools.UserRole.ToUpper() == "PROSECUTOR")
        //                {
        //                    ReturnSearchWarrantMode = "1";
        //                    return RedirectToAction("OpenLoginWindow('./SearchWarrant/ReturnSearchWarrant.aspx?ReturnSearchWarrantMode=" + ReturnSearchWarrantMode + "');");
        //                }
        //                else if (Tools.UserRole.ToUpper() == "MASTER" || Tools.UserRole.ToUpper() == "JUDGE" || Tools.UserRole.ToUpper() == "JUDICIAL CLERK")
        //                {
        //                    ReturnSearchWarrantMode = "2";
        //                    //ActiveDeActiveSection("OpenLoginWindow('./SearchWarrant/JudgeSearchWarrant.aspx');");
        //                    //lblError.Text = "please login as officer";
        //                    return RedirectToAction("OpenLoginWindow('./SearchWarrant/ReturnSearchWarrant.aspx?ReturnSearchWarrantMode=" + ReturnSearchWarrantMode + "');");
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Invalid Login.");
        //                    return View(model);
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Invalid Login.");
        //                return View(model);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ActiveDeActiveApproved();
        //    }
        //}
        //else if (Tools.UserRole == "Jail")      //Select Module
        //{
        //    SaveLoggInfo(AccessController.CurrentQuickEntryUser.FirstName + " " + AccessController.CurrentQuickEntryUser.LastName + " with LoginID " + AccessController.CurrentQuickEntryUser.LoginID + " is logged in into the system as " + AccessController.CurrentUserRole + " at " + DateTime.Now.ToString() + " in " + model.ModuleType, Convert.ToInt16(LoggerBase.LoggerType.Edit));
        //    //JailUsersBase jailUser = new JailUsersBase();                                                                                                                                             //Select Module
        //    //jailUser.LoadJailusers(txtUserID.Text, CommonRoutines.encriptString(txtPassword.Text));
        //    if (Tools.GetLoginID != "")
        //    {
        //        //AccessController.CurrentJailUser = jailUser;

        //        //Response.Redirect(ConfigurationSettingsReader.ApplicationVirtualRoot + "Jail/Default.aspx");
        //        if (Tools.UserRole.ToUpper() == "MASTER" || Tools.UserRole.ToUpper() == "JAILER" || Tools.UserRole.ToUpper() == "SUPERVISOR" || Tools.UserRole.ToUpper() == "PROSECUTOR" || (Tools.UserRole.ToUpper() == "OUTSIDE AGENCY"))
        //        {
        //            if (CheckFirstlogin() == true)
        //            {
        //                return RedirectToAction("~/ChangePassword");
        //            }
        //            return RedirectToAction("OpenLoginWindow('./Jail/Default.aspx');");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Invalid Login.");
        //            return View(model);
        //        }


        //    }



        //}
        //else
        //{

        //if (CheckFirstlogin() == true)
        //                {
        //                    return RedirectToAction("~/ChangePassword");
        //                }
        //                AccessController.CurrentUserModuleUser = userObj;
        //                Session["UserRole"] = UserRole.Admin;
        //                SaveLoggInfo(AccessController.CurrentQuickEntryUser.FirstName + " " + AccessController.CurrentQuickEntryUser.LastName + " with LoginID " + AccessController.CurrentQuickEntryUser.LoginID + " is logged in into the system as " + AccessController.CurrentUserRole + " at " + DateTime.Now.ToString() + " in " + model.ModuleType, Convert.ToInt16(LoggerBase.LoggerType.Edit));
        //                //Select Module
        //                //if (Session["CurrentUserRole"].ToString().ToUpper() == "ADMIN")
        //                //{
        //                return RedirectToAction("~/Admin/Admin");
        //                //}
        //                //else {
        //                //    lblError.Text = "Invalid Login --> Insufficient Access.";
        //                //}
        //            }
        //            return RedirectToAction("ViewList", "Warrant", new { CaseNumber = "" });
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Username Or Password is incorrect");
        //            return View(model);
        //        }

        //    }

        #region SaveLoggInfo
        //private void SaveLoggInfo(string Description, int LogType)
        //    {
        //        LoggerBase OBjLoggBase = new LoggerBase();
        //        Logger_DAL OBjLogdalfile = new Logger_DAL();
        //        OBjLoggBase.LogType = LogType;
        //        OBjLoggBase.LogDescription = Description;
        //        OBjLoggBase.LogDate = DateTime.Now.ToString();
        //        OBjLoggBase.UserID = Tools.GetLoginID;
        //        OBjLogdalfile.Save(OBjLoggBase);
        //    }
        #endregion

        public JsonResult GetNotificationType()
        {
            var user = new Users {Userid = Tools.GetUserId};
            return Json(user, JsonRequestBehavior.AllowGet);
        }

/*
        private bool CheckFirstlogin()
        {
            bool chkfirstlogin = false;
            //if (Session["CurrentUserRole"].ToString() != UserRole.Admin.ToString())
            //{

            //    try
            //    {

            //        DataSet ds = new DataSet();//check user login as first time
            //        ds = UsersBase.CheckFirsttimelogin(txtUserID.Text, CommonRoutines.encriptString(ViewState["Password"].ToString()));
            //        if (ds != null)
            //        {
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ChangePassword", "Openchangepassword();", true);
            //                lblError.Text = "";
            //                chkfirstlogin = true;
            //            }
            //            else
            //            {


            //            }
            //        }
            //    }
            //    catch { }

            //}
            return chkfirstlogin;
        }
*/

/*
        private void ActiveDeActiveApproved()
        {
            // ClientScript.RegisterStartupScript(this.GetType(),"arrnage", MakeCarRentalPopUpMsg);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CaseApproved", MakeCarRentalPopUpMsg, true);
        }
*/
    }
}
