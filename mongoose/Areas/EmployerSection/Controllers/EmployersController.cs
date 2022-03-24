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
            ViewBag.UserId = userId;
            ViewBag.EditProfile = profileDetails;
            ViewBag.BusinessName = loggedIn.Name;

            ViewBag.InternshipCount = db.Internships.Where(i => i.Employer.Id == userId ).Count().ToString();// number of employers open internships

            var activeInternships = 

            ViewBag.ActiveIntershipCount = db.Student_Internship.Where(i => i.Internship.Employer.Id == userId).Count().ToString();
            return View();
            
        }
        public ActionResult ProfilePicture()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.User = userId;
            return View();
        }

        [HttpPost]
        public ActionResult ProfilePicture(HttpPostedFileBase file)
        {
            if (file != null)
            {
                //string path = path to profile folder + logged in users id;
              
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Images/"), User.Identity.GetUserId() + ".jpg"); //may need to change path for web server
                
                // file is uploaded 
                file.SaveAs(path);

                Console.WriteLine(path);


            }
            // after successfully uploading redirect the user
            return RedirectToAction("Home");
        }
        public ActionResult OpenInternships()
        {   
            var userId = User.Identity.GetUserId();
            ViewBag.User = userId;
            ViewBag.EmployerId = db.Employers.Where(e => e.Id == userId);
            var internships = db.Internships.Where(i => i.Employer.Id == userId);   //List of internships created by logged in employer m.b.
            ViewBag.Saved = db.Saved_Internship.ToList();

            return View(internships.ToList());
        }
        public ActionResult ActiveInternships()
        {
            var userId = User.Identity.GetUserId();
            var internships = db.Student_Internship.Where(i => i.Internship.Employer.Id == userId);   //List of "active" student_internships created by logged in employer
            return View(internships.ToList());
        }
        public ActionResult StudentSearch(string sortOrder, string searchString, int? majorId)
        {
            var Majors = db.Majors.Select(rr => new SelectListItem { Value = rr.MajorId.ToString(), Text = rr.Name }).ToList();
            Majors.Insert(0, (new SelectListItem { Text = "All Majors", Value = "0" }));
            ViewBag.Majors = Majors;

            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LocationSortParm = sortOrder == "Start" ? "location_desc" : "Location";
            ViewBag.EnrollmentSortParm = sortOrder == "Enrolled" ? "not_enrolled" : "Enrolled";
            var students = from s in db.Students
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(i => i.FirstName.Contains(searchString)
                                       || i.LastName.Contains(searchString));
            }

            if (majorId > 0)
            {
                var intMaj = db.Student_Major.Where(i => i.MajorId == majorId);
                var stuIds = intMaj.Select(x => x.StudentId).ToList();
                students = students.Where(i => stuIds.Contains(i.StudentId));

            }

            switch (sortOrder)
            {
                case "Enrolled":
                    students = students.Where(s => s.EnrollmentStatus == 0);
                    break;
                case "not_enrolled":
                    students = students.Where(s => s.EnrollmentStatus != 0);
                    break;
                case "Location":
                    students = students.OrderBy(s => s.State);
                    break;
                case "location_desc":
                    students = students.OrderByDescending(s => s.State);
                    break;
                case "Name":
                    students = students.OrderBy(s => s.LastName);
                    break;
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.GraduationDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.GraduationDate);
                    break;
                default:
                    
                    break;
            }

            
            return View(students);
        }

        public ActionResult StudentSaved(int? id)
        {
            ViewBag.InternshipId = id;
            var saved = db.Saved_Internship.Where(s => s.InternshipId == id).ToList();
            var student = saved.Select(x => x.Student).ToList();
            return View(student);
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
                employer.Id = User.Identity.GetUserId(); //binds employer with logged in user m.b.
                db.SaveChanges();
                
                

                return RedirectToAction("Home");
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
            var UserId = User.Identity.GetUserId();
            ViewBag.UserEmpId = db.Employers.FirstOrDefault(e => e.Id == UserId).EmployerId;
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
                return RedirectToAction("Home");
            }
            return View(employer);
        }
        [Authorize(Roles = "Instructor")]
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
