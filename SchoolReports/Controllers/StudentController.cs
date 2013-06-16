using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolReports.Models;

namespace SchoolReports.Controllers
{
    public class StudentController : BaseController
    {
        private ReportContext db = new ReportContext();

        public ActionResult Index()
        {
            return this.RedirectToLoginIfNotAuthenticated(View(db.Students.ToList()));
        }

        public ActionResult Create()
        {
            return this.RedirectToLoginIfNotAuthenticated(View());
        }

        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
            }

            return this.RedirectToLoginIfNotAuthenticated(View(student));
        }

        public ActionResult Edit(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return this.RedirectToLoginIfNotAuthenticated(View(student));
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
            }

            return this.RedirectToLoginIfNotAuthenticated(View(student));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}