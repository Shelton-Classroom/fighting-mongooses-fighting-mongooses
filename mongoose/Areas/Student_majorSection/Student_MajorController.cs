using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mongoose.Models;
using Microsoft.AspNet.Identity; //This is needed when using User.Identity.GetUserId


namespace mongoose.Areas.Student_majorSection
{
    public class Student_MajorController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: Student_majorSection/Student_Major
        public ActionResult Index()
        {
            var student_Major = db.Student_Major.Include(s => s.Major).Include(s => s.Student);
            ViewBag.Developer = "MB";
            return View(student_Major.ToList());
        }

        // GET: Student_majorSection/Student_Major/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Major student_Major = db.Student_Major.Find(id);
            if (student_Major == null)
            {
                return HttpNotFound();
            }
            ViewBag.Developer = "MB";
            return View(student_Major);
        }

        // GET: Student_majorSection/Student_Major/Create
        public ActionResult Create()
        {
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "LastName");

            ViewBag.Developer = "MB";
            return View();
        }

        // POST: Student_majorSection/Student_Major/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentMajorId,MajorId,StudentId")] Student_Major student_Major)
        {
            if (ModelState.IsValid)
            {
                db.Student_Major.Add(student_Major);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name", student_Major.MajorId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", student_Major.StudentId);
            return View(student_Major);
        }

        // GET: Student_majorSection/Student_Major/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Major student_Major = db.Student_Major.Find(id);
            if (student_Major == null)
            {
                return HttpNotFound();
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name", student_Major.MajorId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", student_Major.StudentId);
            //var userId = User.Identity.GetUserId();
            //var loggedIn = db.Students.FirstOrDefault(s => s.Id == userId);
            //ViewBag.studentmajor = db.Student_Major.Where(s => s.StudentId == loggedIn.StudentId).ToList(); Getting user ID is not working for the bridge table at this juncture
            ViewBag.Developer = "MB";
            return View(student_Major);
        }

        // POST: Student_majorSection/Student_Major/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentMajorId,MajorId,StudentId")] Student_Major student_Major)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student_Major).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name", student_Major.MajorId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", student_Major.StudentId);
            return View(student_Major);
        }

        // GET: Student_majorSection/Student_Major/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Major student_Major = db.Student_Major.Find(id);
            if (student_Major == null)
            {
                return HttpNotFound();
            }
            ViewBag.Developer = "MB";
            return View(student_Major);
        }

        // POST: Student_majorSection/Student_Major/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student_Major student_Major = db.Student_Major.Find(id);
            db.Student_Major.Remove(student_Major);
            db.SaveChanges();
            ViewBag.Developer = "MB";
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
