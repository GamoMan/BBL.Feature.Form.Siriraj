using System;
using System.Web.Configuration;

namespace BBL.Feature.Form.Siriraj.Config
{
    public class AppSettings
    {
        public static string SecretCode { get; set; }
        public static bool HasKey { get; set; }

        //From KeyVualt
        public static string FormConnectionString { get; set; }

        public static string FormKey { get; set; }
    }
}