﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mongoose.Models;
using Microsoft.AspNet.Identity;

namespace mongoose.Areas.StudentSection.Controllers
{
    public class StudentsController : Controller
    {
        private InternshipEntities db = new InternshipEntities();

        // GET: StudentSection/Students
        [Authorize(Roles = "Student")]
        public ActionResult Home()
        {
            var userId = User.Identity.GetUserId();
            var loggedIn = db.Students.FirstOrDefault(s => s.Id == userId);
            ViewBag.Name = loggedIn.FirstName;

            ViewBag.EditProfile = loggedIn.StudentId;
            ViewBag.UserId = userId;
            ViewBag.Developer = "MB";


            return View();

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
           

            ViewBag.EmployerList = new SelectList(db.Employers.OrderBy(e => e.Name), "EmployerId", "Name");
            var userId = User.Identity.GetUserId(); //gets logged in users id
            var studentId = db.Students.FirstOrDefault(s => s.Id == userId).StudentId; //gets logged in users studentId
            var studentSaved = db.Saved_Internship.Where(s => s.StudentId == studentId); //gets students saved internships
            ViewBag.Saved = studentSaved.Select(x => x.InternshipId).ToList(); // list of just internshipId's from above saved_interships, to display hearts in red in view
            

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
                                       || i.Name.Contains(searchString)) ;
            }
            if (majorId > 0)
            {
                var intMaj = db.Internship_Major.Where( i => i.MajorId == majorId);
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
        //public ActionResult StuMajor()
        //{
        //    var userId = User.Identity.GetUserId();
        //    ViewBag.StudentId = db.Students.Where(s => s.Id == userId);
        //    var studentMajors = db.Majors.Where(o => o.Student.Id == userId);   //Get to coursemajor by logged in student
        //    return View(Majors.ToList());
        //}
        public ActionResult ActiveInternships()   //This will be setup once instructor section has view for adding student to internship creating a student_intership
        {
            var userId = User.Identity.GetUserId();
            var internships = db.Student_Internship.Where(i => i.Student.Id == userId);   //List of internships student is assigned to
            ViewBag.Developer = "MB";
            return View(internships);
        }
        public ActionResult SavedInternships()
        {
            var userId = User.Identity.GetUserId(); //gets logged in users id
            var studentId = db.Students.FirstOrDefault(s => s.Id == userId).StudentId; //gets logged in users studentId
            var internships = db.Saved_Internship.Where(s => s.StudentId == studentId);
            ViewBag.Developer = "MB";
            return View(internships);
        }
        //public ActionResult ActiveInternships()   //This will be setup once instructor section has view for adding student to internship creating a student_intership
        //{
        //    var userId = User.Identity.GetUserId();
        //    var internships = db.Student_Internship.Where(i => i.Student.Id == userId);   //List of internships student is assigned to
        //    return View(internships);
        //}     
        
        //public ActionResult RecommendedInternshipss()   
        //{
        //    var userId = User.Identity.GetUserId();
        //    var studentmajor = db.Student_Major where (i => i.Student.Id == userId);   //List of internships student is assigned to
        //    var recommewndedinternships = db.Internshp
        //    return View(recommendedinternships);
        //}

        // GET: StudentSection/Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return HttpNotFound();
            }
            ViewBag.studentcourse = db.Student_Course.Where(s => s.StudentId == id).ToList();
            ViewBag.studentmajor = db.Student_Major.Where(s => s.StudentId == id).ToList();
            ViewBag.Developer = "MB";
            return View(Student);
        }
        public ActionResult MyAcademics()
        {
            var userId = User.Identity.GetUserId();
            var loggedIn = db.Students.FirstOrDefault(s => s.Id == userId);
            ViewBag.Name = loggedIn.FirstName;

            ViewBag.StudentId = loggedIn.StudentId;
            ViewBag.UserId = userId;
            ViewBag.studentcourse = db.Student_Course.Where(s => s.StudentId == loggedIn.StudentId).ToList();
            ViewBag.studentmajor = db.Student_Major.Where(s => s.StudentId == loggedIn.StudentId).ToList();
            ViewBag.Developer = "GG";
            return View();
        }
        // GET: StudentSection/Students/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: StudentSection/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,FirstName,LastName,GraduationDate,EnrollmentStatus,Email,Phone,Address1,Address2,City,State,Zipcode")] Student Student) //do not change m.b.
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(Student);
                Student.Id = User.Identity.GetUserId();
                db.SaveChanges();



                return RedirectToAction("Home");
            }

            return View(Student);
        }

        // GET: StudentSection/Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return HttpNotFound();
            }
           
            return View(Student);
        }

        // POST: StudentSection/Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,FirstName,LastName,GraduationDate,EnrollmentStatus,Email,Phone,Address1,Address2,City,State,Zipcode")] Student Student) //do not change m.b.
        {
            if (ModelState.IsValid)
            {
                db.Entry(Student).State = EntityState.Modified;
                Student.Id = User.Identity?.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(Student);
        }

        // GET: StudentSection/Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return HttpNotFound();
            }
            
            return View(Student);
        }

        // POST: StudentSection/Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student Student = db.Students.Find(id);
            db.Students.Remove(Student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void InternshipSave(int id)  //on heart click will create saved internship for that internship/student or remove if it exists, hopefully!
        {
            var intId = id;
            var userId = User.Identity.GetUserId();
            var student = db.Students.FirstOrDefault(s => s.Id == userId);
            var stuId = student.StudentId;
            var studentSaved = db.Saved_Internship.FirstOrDefault(s => s.StudentId == stuId & s.InternshipId == id); 
            if (studentSaved != null) 
            {
                
                    db.Saved_Internship.Remove(studentSaved);   
                    db.SaveChanges();
                
            } else
            {
                var saved_Internship = new Saved_Internship(); //new instance of saved_internship
                db.Saved_Internship.Add(saved_Internship); // add to database
                saved_Internship.InternshipId = intId;// add selected Internship Id
                saved_Internship.StudentId = stuId; // add add current student Id
                db.SaveChanges(); //saves to database
            }
            

           
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
