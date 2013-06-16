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
    public class StudentInClassController : BaseController
    {
        private ReportContext db = new ReportContext();

        private int CurrentYear
        {
            get
            {
                if (Session["CurrentYear"] == null)
                {
                    Session["CurrentYear"] = SchoolYear.GetCurrentSchoolYear();
                }

                return (int)Session["CurrentYear"];
            }
            set
            {
                Session["CurrentYear"] = value;
            }
        }

        public ActionResult Index()
        {
            ViewBag.Years = new SelectList(Enumerable.Range(2010, 20).Select(x => new SchoolYear { Year = x }).ToList(), "Year", "Name", this.CurrentYear);
            ViewBag.Classes = db.Classes.ToList().OrderBy(c => c.Name);
            ViewBag.CanAddStudents = db.Students.Where(s => s.StudentInClasses.Count(sc => sc.Year == this.CurrentYear) == 0).Count() != 0;

            var studentInClasses = db.StudentInClasses.Where(s => s.Year == this.CurrentYear)
                .Include(s => s.Student)
                .Include(s => s.Class).ToList()
                .OrderBy(s => s.DisplayName);

            return this.RedirectToLoginIfNotAuthenticated(View(studentInClasses));
        }

        [HttpPost]
        public ActionResult Index(int currentYear = 2012)
        {
            this.CurrentYear = currentYear;

            return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
        }


        public ActionResult FromFile(int currentYear)
        {
            return this.RedirectToLoginIfNotAuthenticated(View());
        }

        [HttpPost]
        public ActionResult FromFile(int currentYear, string fileContent)
        {
            if (this.db.StudentInClasses.Any(s => s.Year == currentYear))
            {
                throw new InvalidOperationException("Er bestaan al koppelingen voor leerlingen van dit jaar!");
            }

            List<StudentInClass> studentsInClass = new List<StudentInClass>();
            var classes = db.Classes.ToList();

            foreach (string line in fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] lineContent = line.Split('\t');

                string lastName = lineContent[0];
                string firstName = lineContent[1];
                string className = lineContent[2];

                Student student = this.db.Students.SingleOrDefault(s =>
                    s.LastName == lastName &&
                    s.FirstName == firstName);

                if (student == null)
                {
                    student = this.db.Students.Create();

                    student.LastName = lastName;
                    student.FirstName = firstName;

                    db.Students.Add(student);
                }
                
                var studentInClass = db.StudentInClasses.Create();

                studentInClass.Class = classes.Single(c => c.Name == className);
                studentInClass.ClassId = studentInClass.Class.Id;
                studentInClass.Year = currentYear;
                studentInClass.Student = student;
                studentInClass.StudentId = student.Id;

                db.StudentInClasses.Add(studentInClass);

                studentsInClass.Add(studentInClass);
            }

            db.SaveChanges();

            return this.RedirectToLoginIfNotAuthenticated(View(studentsInClass));
        }

        public ActionResult Create(int currentClassId, int currentYear)
        {
            var students = db.Students.Where(s => s.StudentInClasses.Count(sc => sc.Year == currentYear) == 0).ToList();
            if (students.Count == 0)
            {
                throw new InvalidOperationException("Alle leerlingen hebben al een klas voor dit schooljaar!");
            }

            ViewBag.Students = new SelectList(students.OrderBy(s => s.FullName), "Id", "FullName");
            ViewBag.CurrentSchoolYear = new SchoolYear() { Year = currentYear };

            StudentInClass studentInClass = db.StudentInClasses.Create();

            studentInClass.Class = db.Classes.Single(c => c.Id == currentClassId);
            studentInClass.ClassId = currentClassId;
            studentInClass.Year = currentYear;

            return this.RedirectToLoginIfNotAuthenticated(View(studentInClass));
        }

        [HttpPost]
        public ActionResult Create(StudentInClass studentinclass)
        {
            if (ModelState.IsValid)
            {
                db.StudentInClasses.Add(studentinclass);
                db.SaveChanges();
                return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
            }

            return this.RedirectToLoginIfNotAuthenticated(View(studentinclass));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}