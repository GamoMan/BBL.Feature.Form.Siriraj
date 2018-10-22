using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BBL.Feature.Form.Siriraj.Areas.FormSiriraj.Models
{

   

    public class StockItem
    {
        static int _CarItem = -99;
        static int _CameraItem = -99;
        static int _RadioItem = -99;

        public static bool IsSyncdataBase()
        {
            if (_CarItem == -99 || _CameraItem == -99 || _RadioItem == -99)
            {
                return false;
            }

            return true;

        }
        public static int CarStock
        {
           
            set
            {
                _CarItem = Convert.ToInt32 ( value);
            }
        }
        public static int CameraStock {
          
            set
            {
                _CameraItem = Convert.ToInt32(value);
            }
        }
        public static int RadioStock
        {
          
            set
            {
                _RadioItem = Convert.ToInt32(value);
            }
        }


        public static bool HaveCar
        {
            get { return _CarItem > 0; }
        }

        public static bool HaveCamera
        {
            get { return _CameraItem > 0; }
        }


        public static bool HaveRadio
        {
            get { return _RadioItem > 0; }
        }




    }
}