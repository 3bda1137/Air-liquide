using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace MyProject.Filters
{
    public class LanguageFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //string lang = HttpRequestHelper.GetHeaderValue("lang");
            //lang = lang.ToLower();
            //if (string.IsNullOrEmpty(lang))
            //    lang = "ar";
            ////Thread.CurrentThread.CurrentCulture.Name = lang;
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
        }


    }
}
