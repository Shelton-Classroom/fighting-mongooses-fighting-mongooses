using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mongoose.Models;

namespace mongoose.Areas.Internship_MajorSection.Controllers
{
    public class Internship_MajorController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: Internship_MajorSection/Internship_Major
        public ActionResult Index()
        {
            ViewBag.DepartmentId = new SelectList(db.Majors.OrderBy(d => d.MajorId), "Major", "MajorId");
            var internship_Major = db.Internship_Major.Include(i => i.Internship).Include(i => i.Major);
            return View(internship_Major.ToList());
        }

        // GET: Internship_MajorSection/Internship_Major/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Internship_Major internship_Major = db.Internship_Major.Find(id);
            if (internship_Major == null)
            {
                return HttpNotFound();
            }
            return View(internship_Major);
        }

        // GET: Internship_MajorSection/Internship_Major/Create
        public ActionResult Create(int? id)
        {
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name");
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name");
            ViewBag.instId = id;
            return View();
        }

        // POST: Internship_MajorSection/Internship_Major/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InternshipMajorId,MajorId,InternshipId")] Internship_Major internship_Major)
        {
            if (ModelState.IsValid)
            {
                db.Internship_Major.Add(internship_Major);
                db.SaveChanges();
                return RedirectToAction("OpenInternships", "Employers", new { area = "EmployerSection" });
            }

            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", internship_Major.InternshipId);
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name", internship_Major.MajorId);
            return View(internship_Major);
        }

        // GET: Internship_MajorSection/Internship_Major/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Internship_Major internship_Major = db.Internship_Major.Find(id);
            if (internship_Major == null)
            {
                return HttpNotFound();
            }
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", internship_Major.InternshipId);
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name", internship_Major.MajorId);
            return View(internship_Major);
        }

        // POST: Internship_MajorSection/Internship_Major/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InternshipMajorId,MajorId,InternshipId")] Internship_Major internship_Major)
        {
            if (ModelState.IsValid)
            {
                db.Entry(internship_Major).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", internship_Major.InternshipId);
            ViewBag.MajorId = new SelectList(db.Majors, "MajorId", "Name", internship_Major.MajorId);
            return View(internship_Major);
        }

        // GET: Internship_MajorSection/Internship_Major/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Internship_Major internship_Major = db.Internship_Major.Find(id);
            if (internship_Major == null)
            {
                return HttpNotFound();
            }
            return View(internship_Major);
        }

        // POST: Internship_MajorSection/Internship_Major/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Internship_Major internship_Major = db.Internship_Major.Find(id);
            db.Internship_Major.Remove(internship_Major);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _IndexByTag(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var course = db.Courses
                .Include(m => m.CourseId)
                .Where(m => m.Department.Length.Equals(id))
                .ToArray();
            return PartialView("_Index", id);
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
