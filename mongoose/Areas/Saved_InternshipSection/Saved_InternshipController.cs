using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mongoose.Models;

namespace mongoose.Areas.Saved_InternshipSection
{
    public class Saved_InternshipController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: Saved_InternshipSection/Saved_Internship
        public ActionResult Index()
        {
            var saved_Internship = db.Saved_Internship.Include(s => s.Internship).Include(s => s.Student);
            ViewBag.Developer = "MB";
            return View(saved_Internship.ToList());
        }

        // GET: Saved_InternshipSection/Saved_Internship/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saved_Internship saved_Internship = db.Saved_Internship.Find(id);
            if (saved_Internship == null)
            {
                return HttpNotFound();
            }
            ViewBag.Developer = "MB";
            return View(saved_Internship);
        }

        // GET: Saved_InternshipSection/Saved_Internship/Create
        public ActionResult Create(int? id)
        {

            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName");
            ViewBag.Developer = "MB";
            return View();
        }

        // POST: Saved_InternshipSection/Saved_Internship/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Saved_InternshipId,StudentId,InternshipId")] Saved_Internship saved_Internship)
        {
            if (ModelState.IsValid)
            {
                db.Saved_Internship.Add(saved_Internship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", saved_Internship.InternshipId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", saved_Internship.StudentId);
            return View(saved_Internship);
        }

        // GET: Saved_InternshipSection/Saved_Internship/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saved_Internship saved_Internship = db.Saved_Internship.Find(id);
            if (saved_Internship == null)
            {
                return HttpNotFound();
            }
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", saved_Internship.InternshipId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", saved_Internship.StudentId);
            ViewBag.Developer = "MB";
            return View(saved_Internship);
        }

        // POST: Saved_InternshipSection/Saved_Internship/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Saved_InternshipId,StudentId,InternshipId")] Saved_Internship saved_Internship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saved_Internship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", saved_Internship.InternshipId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", saved_Internship.StudentId);
            return View(saved_Internship);
        }

        // GET: Saved_InternshipSection/Saved_Internship/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saved_Internship saved_Internship = db.Saved_Internship.Find(id);
            if (saved_Internship == null)
            {
                return HttpNotFound();
            }
            ViewBag.Developer = "MB";
            return View(saved_Internship);
        }

        // POST: Saved_InternshipSection/Saved_Internship/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Saved_Internship saved_Internship = db.Saved_Internship.Find(id);
            db.Saved_Internship.Remove(saved_Internship);
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
