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

namespace mongoose.Areas.InternshipSection.Controllers
{
    public class InternshipsController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: InternshipSection/Internships
        public ActionResult Index()
        {
            var internships = db.Internships.Include(i => i.Employer);
            return View(internships.ToList());
        }

        // GET: InternshipSection/Internships/Details/5
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

        // GET: InternshipSection/Internships/Create
        public ActionResult Create()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name");
            ViewBag.Majors = db.Majors.Select(rr => new SelectListItem { Value = rr.MajorId.ToString(), Text = rr.Name }).ToList(); //SelectList of majors so employers can add major(s) to internship on creation
            return View();
        }

        // POST: InternshipSection/Internships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InternshipId,Name,Description,Length,Rate,Location,StartDate,Paid")] Internship internship,int majors) 
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();  //logged in users id
                var user = db.Employers.FirstOrDefault(e => e.Id == userId); //employer that matches logged in user
                db.Internships.Add(internship);
                internship.EmployerId = user.EmployerId; //assigns logged in employer
                internship.PostDate = DateTime.Now; // assigns current date to post date
                db.SaveChanges();
       
                var internship_major = new Internship_Major(); //new instance of internship_major
                db.Internship_Major.Add(internship_major); // add to database
                internship_major.MajorId = majors;// add selected majorId
                internship_major.InternshipId = internship.InternshipId; // add newly created internship id
                db.SaveChanges(); //saves to database
                
    
                return RedirectToAction("OpenInternships", "Employers", new {area= "EmployerSection"});
            }

            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name", internship.EmployerId);
            return View(internship);
        }

        // GET: InternshipSection/Internships/Edit/5
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

        // POST: InternshipSection/Internships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InternshipId,EmployerId,Name,Description,Length,Rate,Location,StartDate,PostDate,Paid")] Internship internship)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(internship).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("OpenInternships", "Employers", new { area = "EmployerSection" });
            }
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name", internship.EmployerId);
            return View(internship);
        }

        // GET: InternshipSection/Internships/Delete/5
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

        // POST: InternshipSection/Internships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {   
            var intMajId = db.Internship_Major.Where(i => i.InternshipId == id); //list of all internshipMajors that are tied to internship
            foreach (var i in intMajId) //loops through list above
            {
                Internship_Major deleteMajor = db.Internship_Major.Find(i.InternshipMajorId); //selects internship major 
                db.Internship_Major.Remove(deleteMajor);//deletes each internship major tied to internship!     
            }
            Internship internship = db.Internships.Find(id);
            db.Internships.Remove(internship);
            db.SaveChanges();
            return RedirectToAction("OpenInternships", "Employers", new { area = "EmployerSection" });
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
