namespace BBL.Feature.Form.Siriraj.Areas.FormSiriraj.Models
{
    public static class Items
    {
        
        
        public static bool haveCar()
        {
            return true;
        }

        public static bool haveCamera()
        {
            return true;
        }

        public static bool haveRadio()
        {
            return true;
        }


         public static void   iteminei()
            {

            SirirajDb db = new SirirajDb();

            var data = db.AvailableItems();

           

        }


    }
}