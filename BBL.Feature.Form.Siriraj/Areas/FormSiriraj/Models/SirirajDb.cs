using BBL.Feature.Form.Siriraj.Config;
using BBL.Feature.Form.Siriraj.Helpers;
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

            ds.WriteXml(@"C:\temp\tem.xml"); //Remove
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


            //Save Personal

            string name = model.personal.Name;


            return query.ToArray();
        }



        public bool haveCitizenID(string CitizenID)
        {
            string myID = Hash.HashCitizenID(CitizenID);

            List<IDataParameter> parms = new List<IDataParameter>();
            var sql = "SELECT * FROM Register WHERE CitizenID = @ID";
            parms.Add(new SqlParameter("@ID", myID));
            object itemcount = Db.FbExecuteScalar ("sp_ExistsCitizenID", parms);

            int  citiCount= Convert.ToInt32(itemcount);
            if (citiCount > 0)
                return true;
            else
                return false;
        }


        public object AvailableItems()
        {
            DataSet ds = new DataSet();
          
            ds = Db.GetDataSet("sp_AvailableItems");
            DataTable dt = ds.Tables[0];

            var query = (from temp in dt.AsEnumerable()
                         select new
                         {
                             Car = temp.Field<Int32>("Car") > 0,
                             Camera = temp.Field<Int32>("Camera") > 0,
                             Radio = temp.Field<Int32>("Radio") > 0,
                         });
            return query.ToArray();
        }
    }
}