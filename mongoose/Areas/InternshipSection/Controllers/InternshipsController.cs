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
            ViewBag.Developer = "MB";
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
            ViewBag.Developer = "MB";
            ViewBag.EmpId = internship.Employer.Id;
            if (User.IsInRole("Student"))
            {
                var userId = User.Identity.GetUserId();
                var studentId = db.Students.FirstOrDefault(s => s.Id == userId).StudentId;
                var StudentApplications = db.Applications.Where(a => a.StudentId == studentId).ToList().FirstOrDefault(sa => sa.InternshipId == id);
                if (StudentApplications != null){
                    ViewBag.Match = id;
                } else
                {
                    ViewBag.Match = 0;
                }

            }
            return View(internship);
        }



    

        // GET: InternshipSection/Internships/Create
        public ActionResult Create()
        {
            ViewBag.EmployerId = new SelectList(db.Employers, "EmployerId", "Name");
            /*ViewBag.Majors = db.Majors.Select(rr => new SelectListItem { Value = rr.MajorId.ToString(), Text = rr.Name }).ToList();*/ //SelectList of majors so employers can add major(s) to internship on creation
            ViewBag.Majors = db.Majors.ToList();
            ViewBag.Developer = "MB";
            return View();
        }

        // POST: InternshipSection/Internships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InternshipId,Name,Description,Length,Rate,Location,StartDate,Paid")] Internship internship,FormCollection form) 
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();  //logged in users id
                var user = db.Employers.FirstOrDefault(e => e.Id == userId); //employer that matches logged in user
                db.Internships.Add(internship);
                internship.EmployerId = user.EmployerId; //assigns logged in employer
                internship.PostDate = DateTime.Now; // assigns current date to post date
                db.SaveChanges();

                string Majors = form["majors"]; // string of major selected major id's
                if (Majors != null)
                {
                    string[] split = Majors.Split(','); // splits string into string array of Id's
                    int[] testMajor = Array.ConvertAll(split, s => int.Parse(s)); //converts string array to int
                    for (int i = 0; i < testMajor.Length; i++)
                    {
                        var internship_major = new Internship_Major(); //new instance of internship_major
                        db.Internship_Major.Add(internship_major); // add to database
                        internship_major.MajorId = testMajor[i];// add selected majorId
                        internship_major.InternshipId = internship.InternshipId; // add newly created internship id
                    }
                    db.SaveChanges(); //saves to database
                }





                if (User.IsInRole("Employer"))
                {
                    return RedirectToAction("OpenInternships", "Employers", new { area = "EmployerSection" });
                }
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index");
                }
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
            ViewBag.Developer = "MB";
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
                if (User.IsInRole("Employer"))
                {
                    return RedirectToAction("OpenInternships", "Employers", new { area = "EmployerSection" });
                }
                return RedirectToAction("Index");
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
            ViewBag.Developer = "MB";
            return View(internship);
        }

        // POST: InternshipSection/Internships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {   
            var intMaj = db.Internship_Major.Where(i => i.InternshipId == id); //list of all internshipMajors that are tied to internship mb
            var stuInt = db.Student_Internship.Where(i => i.InternshipId == id);
            var saveInt =db.Saved_Internship.Where(i => i.InternshipId == id);
            foreach (var i in intMaj) //loops through list above
            {
                Internship_Major deleteMajor = db.Internship_Major.Find(i.InternshipMajorId); //selects internship major mb
                db.Internship_Major.Remove(deleteMajor);//deletes the internship major tied to internship being deleted!     
            }
            foreach (var i in stuInt) // same for student internships mb
            {
                Student_Internship deleteStuInt = db.Student_Internship.Find(i.StudentInternshipId);
                db.Student_Internship.Remove(deleteStuInt);
            }
            foreach (var i in saveInt) // same for saved internships mb
            {
                Saved_Internship deleteSaveInt = db.Saved_Internship.Find(i.Saved_InternshipId);
                db.Saved_Internship.Remove(deleteSaveInt);
            }
            Internship internship = db.Internships.Find(id);
            db.Internships.Remove(internship);
            db.SaveChanges();
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("OpenInternships", "Employers", new { area = "EmployerSection" });
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
