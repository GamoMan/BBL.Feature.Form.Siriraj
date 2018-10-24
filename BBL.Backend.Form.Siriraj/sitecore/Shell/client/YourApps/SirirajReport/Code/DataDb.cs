using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BBL.Backend.Form.Siriraj.Config;
using BBL.Backend.Form.Siriraj.Cryptography;
using DataAccess = BBL.Backend.Form.Siriraj.Db.DataAccess;

namespace BBL.Backend.Form.Siriraj.sitecore.Shell.client.YourApps.SirirajReport.Renderings
{
    public class DataDb : DataAccess
    {
        public string JsonData;

        public Object[] GetRegisterUsers(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            string store = "Select * From Personal where CreateDate between @FromDate and @ToDate Order By CreateDate";
            List<IDataParameter> parms = new List<IDataParameter>();
            parms.Add(new SqlParameter("@FromDate", FromDate + " 00:00:00"));
            parms.Add(new SqlParameter("@ToDate", ToDate + " 23:59:59"));

            ds = Connection.GetDataSet(store, parms, CommandType.Text);

            //ds = Connection.GetDataSet(store);

            DataTable dt = ds.Tables[0];

            var query = (from temp in dt.AsEnumerable()
                         select new
                         {
                             //ID = temp.Field<Guid>("ID"),
                             Data = Decryption.Decrypt(AppSettings.FormKey, temp.Field<String>("Data")),
                             CreateDate = temp.Field<DateTime>("CreateDate").ToString("dd/MM/yyyy HH:mm:ss")
                         });

            return query.ToArray();
        }
    }
}