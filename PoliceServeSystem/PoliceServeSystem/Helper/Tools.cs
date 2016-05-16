using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using PoliceServeSystem.Models;

namespace PoliceServeSystem.Helper
{
    public class Tools
    {
        public static Users UserData()
        {
            Users serializeModel = new Users();
            HttpCookie cookieTicket = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            try
            {
                if (cookieTicket != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookieTicket.Value);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    if (authTicket != null) serializeModel = serializer.Deserialize<Users>(authTicket.UserData);
                }
                //IPrincipal currentUser = HttpContext.Current.User;
                //FormsAuthenticationTicket identity = ((FormsIdentity)currentUser.Identity).Ticket;
                //JavaScriptSerializer json = new JavaScriptSerializer();
                //obj = json.Deserialize<Net_GetUsers>(identity.UserData);
                return serializeModel;
            }
            catch (Exception)
            {
                return serializeModel;
            }
        }

        public static string GetUserId => UserData().Userid;

        public static int IsAgencyExpired => UserData().IsAgencyExpired;

        public static int? UserActive => UserData().UserActive;
    }
}