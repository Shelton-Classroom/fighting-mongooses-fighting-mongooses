using System.Web.Mvc;

namespace mongoose.Areas.Student_InternshipSection
{
    public class Student_InternshipSectionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Student_InternshipSection";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Student_InternshipSection_default",
                "Student_InternshipSection/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}