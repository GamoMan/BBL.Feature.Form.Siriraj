using BBL.Feature.Form.Siriraj.Config;
using CoreDb;

namespace BBL.Feature.Form.Siriraj.Db
{
    public class DataAccess
    {
        public DataAccessLayer Db;

        public DataAccess()
        {
            var connecStionstring = AppSettings.FormConnectionString;

            Db = new DataBaseSql(connecStionstring);
        }
    }
}