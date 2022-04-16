using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mongoose.Models;

namespace mongoose.Areas.Student_InternshipSection.Controllers
{
    public class Student_InternshipController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: Student_InternshipSection/Student_Internship
        public ActionResult Index()
        {
            var student_Internship = db.Student_Internship.Include(s => s.Instructor).Include(s => s.Internship).Include(s => s.Student);
            return View(student_Internship.ToList());
        }

        // GET: Student_InternshipSection/Student_Internship/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Internship student_Internship = db.Student_Internship.Find(id);
            if (student_Internship == null)
            {
                return HttpNotFound();
            }
            return View(student_Internship);
        }

        // GET: Student_InternshipSection/Student_Internship/Create
        public ActionResult Create()
        {
            ViewBag.InstructorId = new SelectList(db.Instructors, "InstructorId", "FirstName");
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName");
            return View();
        }

        // POST: Student_InternshipSection/Student_Internship/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentInternshipId,StudentId,InstructorId,InternshipId,Midterm,Final,Comments,Term,Semester")] Student_Internship student_Internship)
        {
            if (ModelState.IsValid)
            {
                db.Student_Internship.Add(student_Internship);
                db.SaveChanges();

                if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("ActiveInternships", "Instructors", new { area = "InstructorSection" });
                }
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.InstructorId = new SelectList(db.Instructors, "InstructorId", "FirstName", student_Internship.InstructorId);
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", student_Internship.InternshipId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", student_Internship.StudentId);
            return View(student_Internship);
        }

        // GET: Student_InternshipSection/Student_Internship/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Internship student_Internship = db.Student_Internship.Find(id);
            if (student_Internship == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "InstructorId", "FirstName", student_Internship.InstructorId);
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", student_Internship.InternshipId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", student_Internship.StudentId);
            return View(student_Internship);
        }

        // POST: Student_InternshipSection/Student_Internship/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentInternshipId,StudentId,InstructorId,InternshipId,Midterm,Final,Comments,Term,Semester")] Student_Internship student_Internship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student_Internship).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new {id = student_Internship.StudentInternshipId });
                //if (User.IsInRole("Employer"))
                //{
                //    return RedirectToAction("ActiveInternships", "Employers", new { area = "EmployerSection" });
                //}
                //if (User.IsInRole("Instructor"))
                //{
                //    return RedirectToAction("ActiveInternships", "Instructors", new { area = "InstructorSection" });
                //}
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "InstructorId", "FirstName", student_Internship.InstructorId);
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", student_Internship.InternshipId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", student_Internship.StudentId);
            return View(student_Internship);
        }

        // GET: Student_InternshipSection/Student_Internship/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Internship student_Internship = db.Student_Internship.Find(id);
            if (student_Internship == null)
            {
                return HttpNotFound();
            }
            return View(student_Internship);
        }

        // POST: Student_InternshipSection/Student_Internship/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student_Internship student_Internship = db.Student_Internship.Find(id);
            db.Student_Internship.Remove(student_Internship);
            db.SaveChanges();
            if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("ActiveInternships", "Instructors", new { area = "InstructorSection" });
            }
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
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
