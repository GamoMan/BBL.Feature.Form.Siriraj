using BBL.Backend.Form.Siriraj.Config;
using CoreDb;

namespace BBL.Backend.Form.Siriraj.Db
{
    public class DataAccess
    {
        public DataAccessLayer Connection;

        public DataAccess()
        {
            var connecStionstring = AppSettings.FormConnectionString;

            Connection = new DataBaseSql(connecStionstring);
        }
    }
}