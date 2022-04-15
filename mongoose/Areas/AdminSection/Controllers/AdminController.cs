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
using OfficeOpenXml;
namespace mongoose.Areas.AdminSection.Controllers
{
    public class AdminController : Controller
    {
        private InternshipEntities db = new InternshipEntities();
        // GET: AdminSection/Admin
        public ActionResult Home()
        {
            return View();
        }


        public ActionResult Upload()
        {
            
            return View();
        }



        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            Student_Course student_Course = new Student_Course();
                            student_Course.CourseId = Int32.Parse(workSheet.Cells[rowIterator, 1].Value.ToString()) ;
                            student_Course.StudentId = Int32.Parse(workSheet.Cells[rowIterator, 2].Value.ToString()) ;
                            var term = workSheet.Cells[rowIterator, 3].Value.ToString();
                            if(term == "0")
                            {
                                student_Course.SemesterCompleted = semester.Spring;
                            }
                            if (term == "1")
                            {
                                student_Course.SemesterCompleted = semester.Summer;
                            }
                            if (term == "2")
                            {
                                student_Course.SemesterCompleted = semester.Fall;
                            }
                            student_Course.Grade = workSheet.Cells[rowIterator, 4].Value.ToString();
                            student_Course.Term = Int32.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());

                            if (ModelState.IsValid)
                            {
                                db.Student_Course.Add(student_Course);
                                db.SaveChanges();   
                            }
                        }
                        ViewBag.Success = "Student Course Data Successfully added!";
                        return View();
                    }
                }
            }
            return View("Home");
        }
        // GET: AdminSection/Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminSection/Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminSection/Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminSection/Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminSection/Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminSection/Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminSection/Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
