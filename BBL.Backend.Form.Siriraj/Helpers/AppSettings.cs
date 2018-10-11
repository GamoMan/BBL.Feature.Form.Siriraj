using System;
using System.Web.Configuration;

namespace BBL.Backend.Form.Siriraj.Config
{
    public class AppSettings
    {
        public static bool HasKey { get; set; }

        //From KeyVualt
        public static string FormConnectionString { get; set; }

        public static string FormKey { get; set; }
    }
}