using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mongoose.Areas.CourseSection.Controllers
{
    public class InstructorInternshipController : Controller
    {
        // GET: CourseSection/InstructorInternship
        public ActionResult Index()
        {
            return View();
        }

        // GET: CourseSection/InstructorInternship/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CourseSection/InstructorInternship/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseSection/InstructorInternship/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CourseSection/InstructorInternship/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CourseSection/InstructorInternship/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CourseSection/InstructorInternship/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CourseSection/InstructorInternship/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
