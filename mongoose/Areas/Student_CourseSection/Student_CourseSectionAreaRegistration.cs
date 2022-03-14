using System.Web.Mvc;

namespace mongoose.Areas.Student_CourseSection
{
    public class Student_CourseSectionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Student_CourseSection";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Student_CourseSection_default",
                "Student_CourseSection/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}