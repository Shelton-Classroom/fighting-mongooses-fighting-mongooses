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

namespace mongoose.Areas.ApplicationSection.Controllers
{
    public class ApplicationsController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: ApplicationSection/Applications
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.Student).Include(a => a.Internship);
            return View(applications.ToList());
            ViewBag.Developer = "MB";
        }

        // GET: ApplicationSection/Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: ApplicationSection/Applications/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var loggedIn = User.Identity.GetUserId();
            ViewBag.InternshipId = id;
            ViewBag.InternshipTitle = db.Internships.FirstOrDefault(i => i.InternshipId == id).Name;
            ViewBag.Employer = db.Internships.FirstOrDefault(i => i.InternshipId == id).Employer.Name;
            ViewBag.StudentId = db.Students.FirstOrDefault(s => s.Id == loggedIn).StudentId;
            ViewBag.CurrentDate = DateTime.Now;
            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName");
            //ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name");
            ViewBag.Developer = "MB";
            return View();
        }

        // POST: ApplicationSection/Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationId,Resume,InternshipId,StudentId,ApplicationDate")] Application application, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                if (file != null)
                {
                    
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Resumes/"), application.ApplicationId + ".docx");
                    file.SaveAs(path);
                    ViewBag.Success = "Application Sent!";
                }
                return RedirectToAction("Home", "Students", new {area = "StudentSection"});
            }

            
            return View(application);
        }

        // GET: ApplicationSection/Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", application.StudentId);
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", application.InternshipId);
            return View(application);
        }

        // POST: ApplicationSection/Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationId,Resume,InternshipId,StudentId,ApplicationDate")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", application.StudentId);
            ViewBag.InternshipId = new SelectList(db.Internships, "InternshipId", "Name", application.InternshipId);
            return View(application);
        }

        // GET: ApplicationSection/Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: ApplicationSection/Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Resumes/"), id + ".docx");
            if (System.IO.File.Exists(path)){ //removes resume file (if present) on application deletion
                System.IO.File.Delete(path);
            }
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Applicant", "Employers", new { area = "EmployerSection" });
            }
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("ActiveInternships", "Students", new { area = "StudentSection" });
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
