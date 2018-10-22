

using System.Web.Mvc;

namespace BBL.Feature.Form.Siriraj.Areas.FormSiriraj
{
    public class FormSirirajAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FormSiriraj";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FormSiriraj_default",
                "FormSiriraj/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}