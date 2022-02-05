using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mongoose.Models;

namespace mongoose.Areas.EmployerSection.Controllers
{
    public class InternshipsController : Controller
    {
        private InternshipAppEntities db = new InternshipAppEntities();

        // GET: EmployerSection/Internships
        public ActionResult Index()
        {
            var internships = db.Internships.Include(i => i.Employer);
            return View(internships.ToList());
        }

        // GET: EmployerSection/Internships/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Internship internship = db.Internships.Find(id);
            if (internship == null)
            {
                return HttpNotFound();
            }
            return View(internship);
        }

        // GET: EmployerSection/Internships/Create
        public ActionResult Create()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name");
            return View();
        }

        // POST: EmployerSection/Internships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InternshipId,EmployerId,Name,Description,Length,Rate,Location")] Internship internship)
        {
            if (ModelState.IsValid)
            {
                db.Internships.Add(internship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name", internship.EmployerId);
            return View(internship);
        }

        // GET: EmployerSection/Internships/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Internship internship = db.Internships.Find(id);
            if (internship == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name", internship.EmployerId);
            return View(internship);
        }

        // POST: EmployerSection/Internships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InternshipId,EmployerId,Name,Description,Length,Rate,Location")] Internship internship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(internship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name", internship.EmployerId);
            return View(internship);
        }

        // GET: EmployerSection/Internships/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Internship internship = db.Internships.Find(id);
            if (internship == null)
            {
                return HttpNotFound();
            }
            return View(internship);
        }

        // POST: EmployerSection/Internships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Internship internship = db.Internships.Find(id);
            db.Internships.Remove(internship);
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
