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
            

            return View();

        }



        public ActionResult ProfilePicture() 
        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
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
            var userId = User.Identity.GetUserId(); //gets logged in users id
            var studentId = db.Students.FirstOrDefault(s => s.Id == userId).StudentId; //gets logged in users studentId
            var studentSaved = db.Saved_Internship.Where(s => s.StudentId == studentId); //gets students saved internships
            ViewBag.Saved = studentSaved.Select(x => x.InternshipId).ToList(); // list of just internshipId's from above saved_interships, to display hearts in red in view
          
            var internships = db.Internships.ToList(); //List of all internships MB
            return View(internships);
        }
        public ActionResult ActiveInternships()   //This will be setup once instructor section has view for adding student to internship creating a student_intership
        {
            var userId = User.Identity.GetUserId();
            var internships = db.Student_Internship.Where(i => i.Student.Id == userId);   //List of internships student is assigned to
            return View(internships);
        }
        public ActionResult SavedInternships()
        {
            var userId = User.Identity.GetUserId(); //gets logged in users id
            var studentId = db.Students.FirstOrDefault(s => s.Id == userId).StudentId; //gets logged in users studentId
            var internships = db.Saved_Internship.Where(s => s.StudentId == studentId);

            return View(internships);
        }

       
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
            return View(Student);
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
