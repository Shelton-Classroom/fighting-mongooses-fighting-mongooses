using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mongoose.Models;
using Microsoft.AspNet.Identity;

namespace mongoose.Areas.InstructorSection.Controllers
{
    public class InstructorsController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: InstructorSection/Instructors
        [Authorize(Roles = "Instructor")]
        public ActionResult Home()
        {
            var userId = User.Identity.GetUserId();
            var loggedIn = db.Instructors.FirstOrDefault(e => e.Id == userId);

            var profileDetails = loggedIn.InstructorId;

            ViewBag.LoggedIn = loggedIn.FirstName;
            ViewBag.EditProfile = profileDetails;
            ViewBag.UserId = userId;
            return View();
        }

        public ActionResult ProfilePicture()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
            return View();
        }
        [HttpPost]
        public ActionResult ProfilePicture(HttpPostedFileBase file)
        {
            if (file != null)
            {
                //string path = path to profile folder + logged in users id;

                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Images/"), User.Identity.GetUserId() + ".jpg"); //may need to change path for web server

                // file is uploaded 
                file.SaveAs(path);

                Console.WriteLine(path);


            }
            // after successfully uploading redirect the user
            return RedirectToAction("Home");

        }
        public ActionResult OpenInternships()
        {
            var internships = db.Internships.ToList(); //List of all internships MB
            return View(internships);
        }
        public ActionResult ActiveInternships()
        {
            var userId = User.Identity.GetUserId();
            var intructorId = User.Identity.GetUserId();
            var internships = db.Student_Internship.Where(i => i.Instructor.Id == userId);   //List of internships Instructor is assigned to mb
            return View(internships);
        }
        public ActionResult Classes()
        {
            var classes = db.Courses.ToList();//List of all courses MB
            return View(classes);
        }
        public ActionResult Majors()
        {
            var majors = db.Majors.ToList();//List of all majors MB
            return View(majors);
        }

        // GET: InstructorSection/Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: InstructorSection/Instructors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InstructorSection/Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructorId,FirstName,LastName,Email,Phone")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                instructor.Id = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            return View(instructor);
        }

        // GET: InstructorSection/Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: InstructorSection/Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstructorId,FirstName,LastName,Email,Phone")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                instructor.Id = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(instructor);
        }

        // GET: InstructorSection/Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: InstructorSection/Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _IndexByName(string parm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var internships = from i in db.Student_Internship select i;
            internships = internships.Where(i => i.Student.LastName.Contains(parm) || i.Internship.Employer.Name.Contains(parm));
            return PartialView("_Index", internships);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
