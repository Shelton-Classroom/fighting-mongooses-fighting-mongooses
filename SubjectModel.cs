using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MvcApplication.Models
{
    public class SubjectModel
    {
        public SubjectModel()
        {
            SubjectList = new List<SelectListItem>();
        }

        [DisplayName("Subjects")]
        public List<SelecListItem> SubjectList
        {
            get;
            set;
        }
    }
}
