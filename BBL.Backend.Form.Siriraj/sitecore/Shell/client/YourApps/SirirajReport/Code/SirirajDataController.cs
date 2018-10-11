using bbl.AzureUtility.Helpers;
using bbl.BBLSecurity.Cryptography;
using BBL.Backend.Form.Siriraj.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BBL.Backend.Form.Siriraj.sitecore.Shell.client.YourApps.SirirajReport.Renderings
{
    public class SirirajDataController : Controller
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
            var webappKey = "";
            if (AppSettings.HasKey == false)
            {
                webappKey = "Landlord#1";
                //AppSettings.FormConnectionString = ConfigurationManager.ConnectionStrings["ReportConnectionString"].ConnectionString;
                AppSettings.FormConnectionString = "Data Source = (local); Initial Catalog = Siriraj; Integrated Security = False; User ID = sa; Password = P@ssw0rd";
                AppSettings.FormKey = "x8r9ho0GGR";// x8r9ho0GGR";

                AppSettings.HasKey = true;
            }

            //if (AppSettings.HasKey == false)
            //{
            //    var akvHelper = new AkvHelper();
            //    var webappKey = akvHelper.GetSecret("WEBAPPKey").Value;
            //    // var webappConnectionString = akvHelper.GetSecret("ReportConnectionString").Value;
            //    //var aes = new AES(webappKey);
            //    // AppSettings.FormConnectionString = aes.Decrypt(webappConnectionString);
            //    AppSettings.FormConnectionString = ConfigurationManager.ConnectionStrings["ReportConnectionString"].ConnectionString;
            //    AppSettings.FormKey = akvHelper.GetSecret("WEBFORMKey").Value;

            //    AppSettings.HasKey = true;
            //}
        }
    }
}