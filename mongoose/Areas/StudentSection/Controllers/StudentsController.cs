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

namespace mongoose.Areas.StudentSection.Controllers
{
    public class StudentsController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: StudentSection/Students
        [Authorize(Roles = "Student")]
        public ActionResult Home()
        {
            var userId = User.Identity.GetUserId();
            var loggedIn = db.Students.FirstOrDefault(e => e.Id == userId);

            var profileDetails = loggedIn.StudentId;

            ViewBag.LoggedIn = loggedIn.FirstName;
            ViewBag.EditProfile = profileDetails;

            //var internships = db.Internships.Where(i => i.Student.Id == userId);   //List of internships created by logged in Student
            return View();

        }

        public ActionResult OpenInternships()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.StudentId = db.Students.Where(e => e.Id == userId);
            //var internships = db.Internships.Where(i => i.Student.Id == userId);   //studentId is not a property in the internship table MB
            var internships = db.Internships.ToList(); //List of all internships MB
            return View(internships);
        }
        public ActionResult ActiveInternships()
        {
            var userId = User.Identity.GetUserId();
           // var internships = db.Student_Internship.Where(i => i.Internship.Student.Id == userId);   //List of internships created by logged in Student
            return View();
        }

        // GET: StudentSection/Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return HttpNotFound();
            }
            return View(Student);
        }

        // GET: StudentSection/Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentSection/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,Name,ContactName,Phone,Email,Address1,Address2,City,State,Zipcode")] Student Student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(Student);
                Student.Id = User.Identity.GetUserId();
                db.SaveChanges();



                return RedirectToAction("Index");
            }

            return View(Student);
        }

        // GET: StudentSection/Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return HttpNotFound();
            }
            return View(Student);
        }

        // POST: StudentSection/Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,Name,ContactName,Phone,Email,Address1,Address2,City,State,Zipcode")] Student Student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Student).State = EntityState.Modified;
                Student.Id = User.Identity?.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Student);
        }

        // GET: StudentSection/Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return HttpNotFound();
            }
            return View(Student);
        }

        // POST: StudentSection/Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student Student = db.Students.Find(id);
            db.Students.Remove(Student);
            db.SaveChanges();
            return RedirectToAction("Index");
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
