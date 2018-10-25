using BBL.Feature.Form.Siriraj.Config;
using BBL.Feature.Form.Siriraj.Cryptography;
using BBL.Feature.Form.Siriraj.Helpers;
using BBL.Feature.Form.Siriraj.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DataAccess = BBL.Feature.Form.Siriraj.Db.DataAccess;

namespace BBL.Feature.Form.Siriraj.Areas.FormSiriraj.Models
{
    public class SirirajDb : DataAccess
    {
        public string JsonData;

        public object Insert()
        {
            List<IDataParameter> parms = new List<IDataParameter>();
            //var storeName = "INSERT INTO [SIP] ([ID],[Data]) VALUES (NEWID(),@Data) ;SELECT SCOPE_IDENTITY();";
            var storeName = "spInserteBook";
            parms.Add(new SqlParameter("@Data", JsonData));
            //prset.Add(Db.CreateParameterDb("@Data", JsonData));

            object output = Db.FbExecuteNonQuery(storeName, parms, CommandType.StoredProcedure);

            return output;
        }

        public object getAllBranches()
        {
            var sql = "sp_SirirajBranch";
            DataSet ds = new DataSet();
            DataSet dsTemp = new DataSet();
            dsTemp = Db.GetDataSet(sql);

            DataTable dt = dsTemp.Tables[0];
            DataView view = new DataView(dt);
            var obj = dt.AsEnumerable()
            .GroupBy(r => r["Province"])
            .Select(g => new
            {
                Province = g.Key.ToString(),
                Data = g.Select(r => new
                {
                    BranchCode = r["BranchCode"].ToString(),
                    BranchName = r["BranchName"].ToString()
                }).ToArray()
            })
            .ToList();

            var json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public Object[] getDeleteRegister(string ID, string SecretCode)
        {
            DataSet ds = new DataSet();
            string myID = Hash.HashCitizenID(ID);

            List<IDataParameter> parms = new List<IDataParameter>();
            var sql = "SELECT * FROM Register WHERE CitizenID = @ID";
            parms.Add(new SqlParameter("@ID", myID));

            ds = Db.GetDataSet(sql, parms, CommandType.Text);

            DataTable dt = ds.Tables[0];

            var query = (from temp in dt.AsEnumerable()
                         select new
                         {
                             //Result = temp.Field<Int32>("Result"),
                             //StockCar = temp.Field<Int32>("StockCar"),
                             //StockCamera = temp.Field<Int32>("StockCamera"),
                             //StockRadio = temp.Field<Int32>("StockRadio"),
                             InvoiceID = temp.Field<Int32>("InvoiceID"),
                             //CitizenID = temp.Field<String>("CitizenID"),
                             Car = temp.Field<Int32>("Car"),
                             Camera = temp.Field<Int32>("Camera"),
                             Radio = temp.Field<Int32>("Radio"),
                             Receipt = temp.Field<Int32>("Receipt"),
                             DeliveryType = temp.Field<Int32>("DeliveryType"),
                             BranchCode = temp.Field<String>("BranchCode"),
                             BranchName = temp.Field<String>("BranchName"),
                             BranchProvince = temp.Field<String>("BranchProvince"),
                             Barcode = temp.Field<String>("Barcode"),
                             QRcode = temp.Field<String>("QRcode"),
                             Amount = temp.Field<Double>("Amount"),
                             Donate = temp.Field<Double>("Donate"),
                             CreateDate = temp.Field<DateTime>("CreateDate").ToString("yyyy/MM/dd HH:mm:ss")
                         });

            return query.ToArray();
        }

        public object DeleteRegister(string ID, string SecretCode)
        {
            string myID = Hash.HashCitizenID(ID);

            List<IDataParameter> parms = new List<IDataParameter>();

            var sql = "DELETE FROM Register WHERE CitizenID = @ID";
            parms.Add(new SqlParameter("@ID", myID));

            object output = Db.FbExecuteNonQuery(sql, parms, CommandType.Text);

            return output;
        }

        public Object[] getRegister(string ID)
        {
            DataSet ds = new DataSet();
            string myID = Hash.HashCitizenID(ID);

            List<IDataParameter> parms = new List<IDataParameter>();
            var sql = "SELECT * FROM Register WHERE CitizenID = @ID";
            parms.Add(new SqlParameter("@ID", myID));

            ds = Db.GetDataSet(sql, parms, CommandType.Text);

            DataTable dt = ds.Tables[0];

            var query = (from temp in dt.AsEnumerable()
                         select new
                         {
                             InvoiceID = temp.Field<Int32>("InvoiceID"),
                             //CitizenID = temp.Field<String>("CitizenID"),
                             Car = temp.Field<Int32>("Car"),
                             Camera = temp.Field<Int32>("Camera"),
                             Radio = temp.Field<Int32>("Radio"),
                             Receipt = temp.Field<Int32>("Receipt"),
                             DeliveryType = temp.Field<Int32>("DeliveryType"),
                             BranchCode = temp.Field<String>("BranchCode"),
                             BranchName = temp.Field<String>("BranchName"),
                             BranchProvince = temp.Field<String>("BranchProvince"),
                             Barcode = temp.Field<String>("Barcode"),
                             QRCode = temp.Field<String>("QRCode"),
                             Amount = temp.Field<Double>("Amount"),
                             Donate = temp.Field<Double>("Donate"),
                             CreateDate = temp.Field<DateTime>("CreateDate").ToString("yyyy/MM/dd HH:mm:ss")
                         });

            return query.ToArray();
        }

        public object SaveRegister(FormSiriraj.Models.SirirajModel model)
        {
            bool output = false;
            try
            {

                //Begin TranSaction
                Db.OpenFbData();
                Db.BeginTransaction();

                DataSet ds = new DataSet();
                List<IDataParameter> parms = new List<IDataParameter>();
                //var storeName = "INSERT INTO [SIP] ([ID],[Data]) VALUES (NEWID(),@Data) ;SELECT SCOPE_IDENTITY();";
                var storeName = "sp_RegisterUser";
                parms.Add(new SqlParameter("@CitizenID", model.CitizenID));
                parms.Add(new SqlParameter("@Car", model.Car));
                parms.Add(new SqlParameter("@Camera", model.Camera));
                parms.Add(new SqlParameter("@Radio", model.Radio));
                parms.Add(new SqlParameter("@Receipt", model.Receipt));
                parms.Add(new SqlParameter("@DeliveryType", model.DeliveryType));
                parms.Add(new SqlParameter("@BranchCode", model.BranchCode));
                parms.Add(new SqlParameter("@BranchName", model.BranchName));
                parms.Add(new SqlParameter("@BranchProvince", model.BranchProvince));
                parms.Add(new SqlParameter("@Barcode", model.Barcode));
                parms.Add(new SqlParameter("@QRcode", model.QRcode));
                parms.Add(new SqlParameter("@Amount", model.Amount));
                parms.Add(new SqlParameter("@Donate", model.Donate));
                parms.Add(new SqlParameter("@TaxID", model.TaxID));
                parms.Add(new SqlParameter("@Suffix", model.Suffix));

                ds = Db.GetDataSet(storeName, parms, CommandType.StoredProcedure);

                DataTable dt = ds.Tables[0];

                var query = (from temp in dt.AsEnumerable()
                             select new
                             {
                                 Result = temp.Field<Int32>("Result"),
                                 StockCar = temp.Field<Int32>("StockCar"),
                                 StockCamera = temp.Field<Int32>("StockCamera"),
                                 StockRadio = temp.Field<Int32>("StockRadio"),
                                 InvoiceID = temp.Field<Int32>("InvoiceID"),
                                 //CitizenID = temp.Field<String>("CitizenID"),
                                 Car = temp.Field<Int32>("Car"),
                                 Camera = temp.Field<Int32>("Camera"),
                                 Radio = temp.Field<Int32>("Radio"),
                                 Receipt = temp.Field<Int32>("Receipt"),
                                 DeliveryType = temp.Field<Int32>("DeliveryType"),
                                 BranchCode = temp.Field<String>("BranchCode"),
                                 BranchName = temp.Field<String>("BranchName"),
                                 BranchProvince = temp.Field<String>("BranchProvince"),
                                 Barcode = temp.Field<String>("Barcode"),
                                 QRcode = temp.Field<String>("QRcode"),
                                 Amount = temp.Field<Double>("Amount"),
                                 Donate = temp.Field<Double>("Donate"),

                                 CreateDate = temp.Field<DateTime>("CreateDate").ToString("yyyy/MM/dd HH:mm:ss")
                             });



                int CarStock = Convert.ToInt32(dt.Rows[0]["StockCar"]);
                int CameraStock = Convert.ToInt32(dt.Rows[0]["StockCamera"]);
                int RadioStock = Convert.ToInt32(dt.Rows[0]["StockRadio"]);

                //Set Strovk in Memory
                SetItemsStock(CarStock, CameraStock, RadioStock);



                //Save PerSonale ============================================================================

                //S
                model.personal.InvoiceID = Convert.ToInt32(dt.Rows[0]["InvoiceID"]);
                String JsonData = FromString.JsonToString(model.personal);


                List<IDataParameter> parmsData = new List<IDataParameter>();

                var jsonEncrypt = Encryption.Encrypt(AppSettings.FormKey, JsonData);

                //parmsData.Add(Db.CreateParameterDb("@Data", JsonData));
                parmsData.Add(new SqlParameter("@Data", jsonEncrypt));

                 Db.FbExecuteNonQuery("INSERT INTO [Personal]([Data],[CreateDate],[Status])VALUES(@Data, GETDATE(), 0)", parmsData, CommandType.Text );

                //=============================================================================================
            


                Db.CommitTransaction();

                Db.CloseFbData();

                //Refresh Stock
               

                return query.ToArray();
            }
            catch (System.Exception ex)
            {
                Db.RollBackTransaction();
                Db.CloseFbData();
                throw ex;
            }
        }

       

        public object AvailableItems()
        {

            if (StockItem.IsSyncdataBase()==false)
            {
                InitAvailableItems();
            }
 
            var curruntItem = new { Car = StockItem.HaveCar, Camera = StockItem.HaveCamera, Radio = StockItem.HaveRadio };
            return curruntItem;
        }


        public bool CancelOrder(string CitizenID)
        {
                string myID = Hash.HashCitizenID(CitizenID);

                List<IDataParameter> parms = new List<IDataParameter>();
                var sql = "DELETE FROM  [Register] WHERE CitizenID = @ID";
                parms.Add(new SqlParameter("@ID", myID));
                object itemcount = Db.FbExecuteNonQuery(sql, parms);

                int citiCount = Convert.ToInt32(itemcount);
                if (citiCount ==1)
                    return true;
                else
                    return false;
           

        }




        public void InitAvailableItems()
        {
            DataSet ds = new DataSet();

            ds = Db.GetDataSet("sp_AvailableItems");

            DataTable dt = ds.Tables[0];

            StockItem.CarStock = Convert.ToInt32(dt.Rows[0]["Car"]);
            StockItem.CameraStock = Convert.ToInt32(dt.Rows[0]["Camera"]);
            StockItem.RadioStock = Convert.ToInt32(dt.Rows[0]["Radio"]);

        }

        public void SetItemsStock(int car, int Camera ,int Radio)
        {
     
            StockItem.CarStock = Convert.ToInt32(car);
            StockItem.CameraStock = Convert.ToInt32(Camera);
            StockItem.RadioStock = Convert.ToInt32(Radio);

        }


    }
}