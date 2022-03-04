using System.Web.Mvc;

namespace mongoose.Areas.Internship_MajorSection
{
    public class Internship_MajorSectionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Internship_MajorSection";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Internship_MajorSection_default",
                "Internship_MajorSection/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}