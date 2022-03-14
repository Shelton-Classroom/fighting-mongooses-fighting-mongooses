using System.Web.Mvc;

namespace mongoose.Areas.Saved_InternshipSection
{
    public class Saved_InternshipSectionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Saved_InternshipSection";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Saved_InternshipSection_default",
                "Saved_InternshipSection/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}