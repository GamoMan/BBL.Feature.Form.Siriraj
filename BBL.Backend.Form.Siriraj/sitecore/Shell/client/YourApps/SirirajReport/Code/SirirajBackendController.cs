#define DEV
using bbl.AzureUtility.Helpers;
using bbl.BBLSecurity.Cryptography;
using BBL.Backend.Form.Siriraj.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;

namespace BBL.Backend.Form.Siriraj.sitecore.Shell.client.YourApps.SirirajReport.Renderings
{
    public class SirirajBackendController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetUserData(string FromDate, string ToDate)
        {
            Init();

            DataDb UserData = new DataDb();

            Object[] data = UserData.GetRegisterUsers(FromDate, ToDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private void Init()
        {
#if DEV
            var webappKey = "";
            if (AppSettings.HasKey == false)
            {
                webappKey = "Landlord#1";
                //AppSettings.FormConnectionString = ConfigurationManager.ConnectionStrings["ReportConnectionString"].ConnectionString;
                AppSettings.FormConnectionString = "Data Source = (local); Initial Catalog = Siriraj; Integrated Security = False; User ID = sa; Password = P@ssw0rd";
                AppSettings.FormKey = "x8r9ho0GGR";// x8r9ho0GGR";

                AppSettings.HasKey = true;
            }
#else
            if (AppSettings.HasKey == false)
            {
                var akvHelper = new AkvHelper();
                var webappKey = akvHelper.GetSecret("WEBAPPKey").Value;
                AppSettings.FormConnectionString = ConfigurationManager.ConnectionStrings["SirirajConnectionString"].ConnectionString;
                AppSettings.FormKey = akvHelper.GetSecret("WEBFORMKey").Value;

                AppSettings.HasKey = true;
            }
#endif        
        }
    }
}
