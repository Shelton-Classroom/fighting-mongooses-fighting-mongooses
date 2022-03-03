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

namespace mongoose.Areas.EmployerSection.Controllers
{
    public class EmployersController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: EmployerSection/Employers
        [Authorize(Roles = "Employer")]
        public ActionResult Home()
        {   var userId = User.Identity.GetUserId();
            var loggedIn = db.Employers.FirstOrDefault(e => e.Id == userId);

            var profileDetails = loggedIn.EmployerId;

            ViewBag.LoggedIn = loggedIn.ContactName;
            ViewBag.EditProfile = profileDetails;

            var internships = db.Internships.Where(i => i.Employer.Id == userId );   //List of internships created by logged in employer
            return View(internships.ToList());
            
        }

        public ActionResult OpenInternships()
        {
            var userId = User.Identity.GetUserId();
            var internships = db.Internships.Where(i => i.Employer.Id == userId);   //List of internships created by logged in employer
            return View(internships.ToList());
        }
        public ActionResult ActiveInternships()
        {
            var userId = User.Identity.GetUserId();
            var internships = db.Student_Internship.Where(i => i.Internship.Employer.Id == userId);   //List of internships created by logged in employer
            return View(internships.ToList());
        }

        // GET: EmployerSection/Employers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // GET: EmployerSection/Employers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployerSection/Employers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployerId,Name,ContactName,Phone,Email,Address1,Address2,City,State,Zipcode")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                db.Employers.Add(employer);
                employer.Id = User.Identity.GetUserId();
                db.SaveChanges();
                
                

                return RedirectToAction("Index");
            }

            return View(employer);
        }

        // GET: EmployerSection/Employers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: EmployerSection/Employers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployerId,Name,ContactName,Phone,Email,Address1,Address2,City,State,Zipcode")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employer).State = EntityState.Modified;
                employer.Id = User.Identity?.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employer);
        }

        // GET: EmployerSection/Employers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: EmployerSection/Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employer employer = db.Employers.Find(id);
            db.Employers.Remove(employer);
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
