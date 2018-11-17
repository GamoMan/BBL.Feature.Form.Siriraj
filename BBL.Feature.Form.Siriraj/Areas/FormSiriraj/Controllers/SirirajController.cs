#define DEV
using bbl.AzureUtility.Helpers;
using bbl.BBLSecurity.Cryptography;
using BBL.Feature.Form.Siriraj.Areas.FormSiriraj.Models;

//using QRCoder;
using BBL.Feature.Form.Siriraj.Config;
using BBL.Feature.Form.Siriraj.Cryptography;
using BBL.Feature.Form.Siriraj.Helpers;
using BBL.Feature.Form.Siriraj.Json;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace BBL.Feature.Form.Siriraj.Areas.FormSiriraj.Controllers
{
    //public class Crc16
    //{
    //    private static ushort[] CrcTable = {
    //    0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7,
    //    0x8108, 0x9129, 0xa14a, 0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef,
    //    0x1231, 0x0210, 0x3273, 0x2252, 0x52b5, 0x4294, 0x72f7, 0x62d6,
    //    0x9339, 0x8318, 0xb37b, 0xa35a, 0xd3bd, 0xc39c, 0xf3ff, 0xe3de,
    //    0x2462, 0x3443, 0x0420, 0x1401, 0x64e6, 0x74c7, 0x44a4, 0x5485,
    //    0xa56a, 0xb54b, 0x8528, 0x9509, 0xe5ee, 0xf5cf, 0xc5ac, 0xd58d,
    //    0x3653, 0x2672, 0x1611, 0x0630, 0x76d7, 0x66f6, 0x5695, 0x46b4,
    //    0xb75b, 0xa77a, 0x9719, 0x8738, 0xf7df, 0xe7fe, 0xd79d, 0xc7bc,
    //    0x48c4, 0x58e5, 0x6886, 0x78a7, 0x0840, 0x1861, 0x2802, 0x3823,
    //    0xc9cc, 0xd9ed, 0xe98e, 0xf9af, 0x8948, 0x9969, 0xa90a, 0xb92b,
    //    0x5af5, 0x4ad4, 0x7ab7, 0x6a96, 0x1a71, 0x0a50, 0x3a33, 0x2a12,
    //    0xdbfd, 0xcbdc, 0xfbbf, 0xeb9e, 0x9b79, 0x8b58, 0xbb3b, 0xab1a,
    //    0x6ca6, 0x7c87, 0x4ce4, 0x5cc5, 0x2c22, 0x3c03, 0x0c60, 0x1c41,
    //    0xedae, 0xfd8f, 0xcdec, 0xddcd, 0xad2a, 0xbd0b, 0x8d68, 0x9d49,
    //    0x7e97, 0x6eb6, 0x5ed5, 0x4ef4, 0x3e13, 0x2e32, 0x1e51, 0x0e70,
    //    0xff9f, 0xefbe, 0xdfdd, 0xcffc, 0xbf1b, 0xaf3a, 0x9f59, 0x8f78,
    //    0x9188, 0x81a9, 0xb1ca, 0xa1eb, 0xd10c, 0xc12d, 0xf14e, 0xe16f,
    //    0x1080, 0x00a1, 0x30c2, 0x20e3, 0x5004, 0x4025, 0x7046, 0x6067,
    //    0x83b9, 0x9398, 0xa3fb, 0xb3da, 0xc33d, 0xd31c, 0xe37f, 0xf35e,
    //    0x02b1, 0x1290, 0x22f3, 0x32d2, 0x4235, 0x5214, 0x6277, 0x7256,
    //    0xb5ea, 0xa5cb, 0x95a8, 0x8589, 0xf56e, 0xe54f, 0xd52c, 0xc50d,
    //    0x34e2, 0x24c3, 0x14a0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
    //    0xa7db, 0xb7fa, 0x8799, 0x97b8, 0xe75f, 0xf77e, 0xc71d, 0xd73c,
    //    0x26d3, 0x36f2, 0x0691, 0x16b0, 0x6657, 0x7676, 0x4615, 0x5634,
    //    0xd94c, 0xc96d, 0xf90e, 0xe92f, 0x99c8, 0x89e9, 0xb98a, 0xa9ab,
    //    0x5844, 0x4865, 0x7806, 0x6827, 0x18c0, 0x08e1, 0x3882, 0x28a3,
    //    0xcb7d, 0xdb5c, 0xeb3f, 0xfb1e, 0x8bf9, 0x9bd8, 0xabbb, 0xbb9a,
    //    0x4a75, 0x5a54, 0x6a37, 0x7a16, 0x0af1, 0x1ad0, 0x2ab3, 0x3a92,
    //    0xfd2e, 0xed0f, 0xdd6c, 0xcd4d, 0xbdaa, 0xad8b, 0x9de8, 0x8dc9,
    //    0x7c26, 0x6c07, 0x5c64, 0x4c45, 0x3ca2, 0x2c83, 0x1ce0, 0x0cc1,
    //    0xef1f, 0xff3e, 0xcf5d, 0xdf7c, 0xaf9b, 0xbfba, 0x8fd9, 0x9ff8,
    //    0x6e17, 0x7e36, 0x4e55, 0x5e74, 0x2e93, 0x3eb2, 0x0ed1, 0x1ef0 };

    //    public static UInt16 ComputeCrc(byte[] data)
    //    {
    //        ushort crc = 0xFFFF;

    //        foreach (byte datum in data)
    //        {
    //            crc = (ushort)(((crc << 8) & 0xff00) ^ CrcTable[((crc >> 8) & 0xff) ^ datum]);
    //            //crc = (ushort)((crc >> 8) ^ CrcTable[(crc ^ datum) & 0xFF]);
    //        }

    //        return (ushort)(crc & 0xffff);
    //    }
    //}

    public class SirirajController : SitecoreController
    {
        // GET: FormSiriraj/Siriraj
        public override ActionResult Index()
        {
            Item contentItem = null;
            var database = Context.Database;
            if (database != null)
                if (!string.IsNullOrEmpty(
                    RenderingContext.Current.Rendering.DataSource))
                    contentItem = database.GetItem(new ID(
                        RenderingContext.Current.Rendering.DataSource));
            return View(contentItem);
        }

        public ActionResult Delete()
        {
            Item contentItem = null;
            var database = Context.Database;
            if (database != null)
                if (!string.IsNullOrEmpty(
                    RenderingContext.Current.Rendering.DataSource))
                    contentItem = database.GetItem(new ID(
                        RenderingContext.Current.Rendering.DataSource));
            AppSettings.SecretCode = contentItem.Fields["SecretCode"].ToString();
            return View(contentItem);
        }

        public ActionResult Reprint()
        {
            Item contentItem = null;
            var database = Context.Database;
            if (database != null)
                if (!string.IsNullOrEmpty(
                    RenderingContext.Current.Rendering.DataSource))
                    contentItem = database.GetItem(new ID(
                        RenderingContext.Current.Rendering.DataSource));

            return View(contentItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OutputCache(Duration = 604800, VaryByParam = "none", Location = OutputCacheLocation.Any)]
        public async Task<ActionResult> getAllBranches()
        {
            try
            {
                Init();

                SirirajDb db = new SirirajDb();

                Object data = db.getAllBranches();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> getDeleteRegister(string ID, string SecretCode)
        {
            Object data = "";
            try
            {
                Init();
                if (SecretCode == AppSettings.SecretCode)
                {
                    SirirajDb db = new SirirajDb();

                    data = db.getDeleteRegister(ID, SecretCode);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteData(string ID, string SecretCode)
        {
            Object data = "";
            try
            {
                Init();
                if (SecretCode == AppSettings.SecretCode)
                {
                    SirirajDb db = new SirirajDb();

                    data = db.DeleteRegister(ID, SecretCode);
                    db.InitAvailableItems();
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> getRegister(FormSiriraj.Models.SirirajModel model)

        {
            try
            {
                Init();

                SirirajDb db = new SirirajDb();

                Object data = db.getRegister(model.CitizenID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public string getHash(string ID)
        //{
        //    return Hash.HashCitizenID(ID);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public string getBarcode(string TaxID,string Suffix,string Ref1,string Ref2, float Amount)
        //{
        //    return BOTBarcode.genBarcode(TaxID, Suffix, Ref1, Ref2, Amount);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> haveCitizenID(FormSiriraj.Models.SirirajModel model)
        //{
        //    try
        //    {
        //        Init();

        //        SirirajDb db = new SirirajDb();
        //        Object data = db.haveCitizenID(model.CitizenID);

        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        var resultFail = new { Success = "false", e.Message };

        //        return Json(resultFail, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRegister(FormSiriraj.Models.SirirajModel model)
        {
            try
            {
                Init();

                model.ClearCitizenID = model.CitizenID;
                model.CitizenID = Hash.HashCitizenID(model.CitizenID);
                //var modeljson = FromString.JsonToString(model);

                //SirirajDb db = new SirirajDb { JsonData = modeljson };
                SirirajDb db = new SirirajDb();
                Object data = db.SaveRegister(model);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SaveData(FormSiriraj.Models.SirirajModel model)
        //{
        //    try
        //    {
        //        Init();

        //        var modeljson = FromString.JsonToString(model);

        //        var jsonEncrypt = Encryption.Encrypt(AppSettings.FormKey, modeljson);

        //        var SirirajDb = new SirirajDb { JsonData = jsonEncrypt };
        //        //var eBookDb = new eBookDb { JsonData = modeljson };

        //        SirirajDb.Insert();

        //        //byte[] encryptedBytes = System.Convert.FromBase64String(model.id);
        //        //var id = Encoding.UTF8.GetString(encryptedBytes);

        //        //MediaItem mediaItem = Sitecore.Context.Database.Items.GetItem(id);

        //        //var theURL = Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem);

        //        var success = new { Success = "true" };

        //        return Json(success, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        var resultFail = new { Success = "false", e.Message };

        //        return Json(resultFail, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AvailableItems()
        {
            try
            {
                Object data = "";
                Init();

                SirirajDb db = new SirirajDb();

                data = db.AvailableItems();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }

            //Object data = "";
            //Init();

            //SirirajDb db = new SirirajDb();

            //data = db.AvailableItems();

            //return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetAvailableItems()
        {
            try
            {
                Object data = "";
                Init();

                SirirajDb db = new SirirajDb();

                db.InitAvailableItems();

                var success = new { Success = "true" };

                return Json(success, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelOrder(string CitizenID)
        {
            try
            {
                Object data = "";
                Init();

                SirirajDb db = new SirirajDb();

                bool cancelResult = db.CancelOrder(CitizenID);

                if (cancelResult == false)
                {
                    throw new Exception("Unable to cancel order");
                }
                else
                {
                    //Sys Stock To Mem


                    db.InitAvailableItems();

                }

                var success = new { Success = "true" };

                return Json(success, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var resultFail = new { Success = "false", e.Message };

                return Json(resultFail, JsonRequestBehavior.AllowGet);
            }
        }



        private void Init()
        {
#if DEV
            var webappKey = "";
            if (AppSettings.HasKey == false)
            {
                webappKey = "Landlord#1";
                //AppSettings.FormConnectionString = ConfigurationManager.ConnectionStrings["ReportConnectionString"].ConnectionString;
                AppSettings.FormConnectionString = "Data Source = (local); MultipleActiveResultSets=true; Initial Catalog = Siriraj; Integrated Security = False; User ID = sa; Password = P@ssw0rd";
                AppSettings.FormKey = "x8r9ho0GGR";// x8r9ho0GGR";

                AppSettings.HasKey = true;
            }
#else
            if (AppSettings.HasKey == false)
            {
                var akvHelper = new AkvHelper();
                var webappKey = akvHelper.GetSecret("WEBAPPKey").Value;
                var webappConnectionString = akvHelper.GetSecret("SirirajConnectionString").Value;
                var aes = new AES(webappKey);
                AppSettings.FormConnectionString = aes.Decrypt(webappConnectionString);
                AppSettings.FormKey = akvHelper.GetSecret("WEBFORMKey").Value;

                AppSettings.HasKey = true;
            }
#endif
        }
    }
}