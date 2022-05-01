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

namespace mongoose.Areas.InstructorSection.Controllers
{
    public class InstructorsController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: InstructorSection/Instructors
        public ActionResult Index()
        {
            ViewBag.Developer = "MB";
            return View(db.Instructors.ToList());
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult Home()
        {
            var userId = User.Identity.GetUserId();
            if (db.Instructors.Any(i => i.Id == userId))
            {
                var loggedIn = db.Instructors.FirstOrDefault(e => e.Id == userId);
                var profileDetails = loggedIn.InstructorId;
                ViewBag.InternshipCount = db.Student_Internship.Where(i => i.InstructorId == loggedIn.InstructorId).Count().ToString();
                ViewBag.LoggedIn = loggedIn.FirstName;
                ViewBag.EditProfile = profileDetails;
                ViewBag.UserId = userId;
                ViewBag.Developer = "MB";
                return View();
            }else
            {
                return View("Create");
            }
            
        }

        public ActionResult ProfilePicture()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
            ViewBag.Developer = "MB";
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
        public ActionResult OpenInternships(string sortOrder, string searchString, int? majorId)
        {
            var Majors = db.Majors.Select(rr => new SelectListItem { Value = rr.MajorId.ToString(), Text = rr.Name }).ToList();
            Majors.Insert(0, (new SelectListItem { Text = "All Majors", Value = "0" }));
            ViewBag.Majors = Majors;


            //ViewBag.EmployerList = new SelectList(db.Employers.OrderBy(e => e.Name), "EmployerId", "Name");
            //var userId = User.Identity.GetUserId(); //gets logged in users id
            //var studentId = db.Students.FirstOrDefault(s => s.Id == userId).StudentId; //gets logged in users studentId
            //var studentSaved = db.Saved_Internship.Where(s => s.StudentId == studentId); //gets students saved internships
            //ViewBag.Saved = studentSaved.Select(x => x.InternshipId).ToList(); // list of just internshipId's from above saved_interships, to display hearts in red in view


            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.StartDateSortParm = sortOrder == "Start" ? "start_desc" : "Start";
            ViewBag.EmployerNameSortParm = sortOrder == "EmployerName" ? "employerName_desc" : "EmployerName";
            ViewBag.LocationSortParm = sortOrder == "Location" ? "location_desc" : "Location";
            ViewBag.PaidSortParm = sortOrder == "Paid" ? "not_paid" : "Paid";
            var internships = from i in db.Internships
                              select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                internships = internships.Where(i => i.Description.Contains(searchString)
                                       || i.Name.Contains(searchString));
            }
            if (majorId > 0)
            {
                var intMaj = db.Internship_Major.Where(i => i.MajorId == majorId);
                var intIds = intMaj.Select(x => x.InternshipId).ToList();
                internships = internships.Where(i => intIds.Contains(i.InternshipId));

            }
            switch (sortOrder)
            {
                case "Paid":
                    internships = internships.Where(i => i.Paid == 0);
                    break;
                case "not_paid":
                    internships = internships.Where(i => i.Paid != 0);
                    break;
                case "Location":
                    internships = internships.OrderBy(i => i.Location);
                    break;
                case "location_desc":
                    internships = internships.OrderByDescending(i => i.Location);
                    break;
                case "Name":
                    internships = internships.OrderBy(i => i.Name);
                    break;
                case "name_desc":
                    internships = internships.OrderByDescending(i => i.Name);
                    break;
                case "EmployerName":
                    internships = internships.OrderBy(i => i.Employer.Name);
                    break;
                case "employerName_desc":
                    internships = internships.OrderByDescending(i => i.Employer.Name);
                    break;
                case "Date":
                    internships = internships.OrderBy(i => i.PostDate);
                    break;
                case "date_desc":
                    internships = internships.OrderByDescending(i => i.PostDate);
                    break;
                case "Start":
                    internships = internships.OrderBy(i => i.StartDate);
                    break;
                case "start_desc":
                    internships = internships.OrderByDescending(i => i.StartDate);
                    break;
                default:
                    internships = internships.OrderByDescending(i => i.PostDate);
                    break;
            }
            ViewBag.Developer = "MB";
            return View(internships.ToList());
        }
        public ActionResult ActiveInternships(string sortOrder, string searchString, int? instructorId)
        {
            

            
           
            var userid = User.Identity.GetUserId();
            var intructorid = db.Instructors.FirstOrDefault(i => i.Id == userid).InstructorId;
            ///*var internships = db.Student_Internship.Where(i => i.Instructor.Id == userid);*/   //list of internships instructor is assigned to mb
            //ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewBag.StartDatesortParm = sortOrder == "Start" ? "start_desc" : "Start";
            ViewBag.EmployerNameSortParm = sortOrder == "EmployerName" ? "employerName_desc" : "EmployerName";
            //ViewBag.LocationSortParm = sortOrder == "Location" ? "location_desc" : "Location";
            //ViewBag.PaidSortParm = sortOrder == "Paid" ? "not_paid" : "Paid";

            var internships = from i in db.Student_Internship
                              select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                internships = db.Student_Internship.Where(i => i.Student.LastName.Contains(searchString) || i.Student.FirstName.Contains(searchString));
            }
            if (instructorId > 0)
            {
                internships = db.Student_Internship.Where(i => i.InstructorId == instructorId);
            }
            switch (sortOrder)
            {
            //    case "Paid":
            //        internships = internships.Where(i => i.Paid == 0);
            //        break;
            //    case "not_paid":
            //        internships = internships.Where(i => i.Paid != 0);
            //        break;
            //    case "Location":
            //        internships = internships.OrderBy(i => i.Location);
            //        break;
            //    case "location_desc":
            //        internships = internships.OrderByDescending(i => i.Location);
            //        break;
            //    case "Name":
            //        internships = internships.OrderBy(i => i.Name);
            //        break;
            //    case "name_desc":
            //        internships = internships.OrderByDescending(i => i.PostDate);
            //        break;
                case "EmployerName":
                    internships = internships.OrderBy(i => i.Internship.Employer.Name);
                   break;
                case "employerName_desc":
                    internships = internships.OrderByDescending(i => i.Internship.Employer.Name);
                    break;
                default:
                    internships = internships.OrderByDescending(i => i.StudentInternshipId);
                    break;
            }
       
            ViewBag.InstructorList = new SelectList(db.Instructors.OrderBy(i => i.LastName), "InstructorId", "LastName");
            ViewBag.Developer = "MB";
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

            ViewBag.Developer = "MB";
            return View(students);
        }
        public ActionResult Classes()
        {
            var classes = db.Courses.ToList();//List of all courses MB
            return View(classes);
        }
        public ActionResult Majors()
        {
            var majors = db.Majors.ToList();//List of all majors MB
            return View(majors);
        }

        // GET: InstructorSection/Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Developer = "MB";
            return View(instructor);
        }

        // GET: InstructorSection/Instructors/Create
        public ActionResult Create()
        {
            ViewBag.Developer = "MB";
            return View();
        }

        // POST: InstructorSection/Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructorId,FirstName,LastName,Email,Phone")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                instructor.Id = User.Identity.GetUserId();
                db.SaveChanges();
                if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("Home");
                }
                return RedirectToAction("Index");
            }

            return View(instructor);
        }

        // GET: InstructorSection/Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: InstructorSection/Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstructorId,FirstName,LastName,Email,Phone")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                instructor.Id = User.Identity.GetUserId();
                db.SaveChanges();
                if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("Home");
                }
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        // GET: InstructorSection/Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: InstructorSection/Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _IndexByName(string parm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var internships = from i in db.Student_Internship select i;
            internships = internships.Where(i => i.Student.LastName.Contains(parm) || i.Internship.Employer.Name.Contains(parm));
            return PartialView("_Index", internships);
        }

        //Alex had to comment out this code because app would not build, there is already an activeInternship controller up above and there must be a return 

        //public ActionResult ActiveInternships(string sortOrder, int pageNumber=1, int pageSize=1)
        //{
        //    ViewBag.InternshipSortParam = String.IsNullOrEmpty(sortOrder) ? "intern_desc" : "";
        //    int ExcludeRecords = (pageSize * pageNumber) - pageSize;

        //    var Internships = from i in db.Internship_Major.Include(m => m.Internship).Include(m => m.InternshipId)
        //                      select i;

        //    switch(sortOrder)
        //    {
        //        case "intern_desc":
        //            Internships = Internships.OrderByDescending(m => m.Internship);
        //            break;
        //        default:
        //            Internships = Internships.OrderBy(m => m.Internship);
        //            break;
                    
        //    }

        //    Internships = Internships
        //        .Skip(ExcludeRecords)
        //        .Take(pageSize);

        //    var result = new PagedResult<Major>
        //    {
        //        pageNumber = pageNumber,
        //        pageSize = pageSize,
        //        TotalItems = db.Internships.Count()

        //    };
            
        //}
        
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
