using System.Web.Script.Serialization;

namespace BBL.Backend.Form.Siriraj.Json
{
    public class FromString
    {
        public static string JsonToString(object obj)
        {
            var js = new JavaScriptSerializer();

            var jsonText = js.Serialize(obj);

            return jsonText;
        }
    }
}