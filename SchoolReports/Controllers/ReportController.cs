namespace SchoolReports.Controllers
{
    using SchoolReports.Models;
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class ReportController : BaseController
    {
        private ReportContext db = new ReportContext();

        private int CurrentPeriodId
        {
            get
            {
                if (Session["CurrentPeriodId"] == null)
                {
                    Session["CurrentPeriodId"] = Period.GetCurrentPeriodId(db.Periods.ToList());
                }

                return (int)Session["CurrentPeriodId"];
            }
            set
            {
                Session["CurrentPeriodId"] = value;
            }
        }

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

        private int CurrentClassId
        {
            get
            {
                if (Session["CurrentClassId"] == null)
                {
                    Session["CurrentClassId"] = db.Classes.First().Id;
                }

                return (int)Session["CurrentClassId"];
            }
            set
            {
                Session["CurrentClassId"] = value;
            }
        }

        public ActionResult Index()
        {
            ViewBag.Periods = new SelectList(db.Periods.OrderBy(p => p.NumberGym), "Id", "Name", this.CurrentPeriodId);
            ViewBag.Years = new SelectList(Enumerable.Range(2010, 20).Select(x => new SchoolYear { Year = x }).ToList(), "Year", "Name", this.CurrentYear);
            ViewBag.Classes = new SelectList(db.Classes, "Id", "Name", this.CurrentClassId);

            ViewBag.Reports = this.db.Reports.ToList();

            var studentInClasses = db.StudentInClasses.Where(s => s.Year == this.CurrentYear && s.ClassId == this.CurrentClassId)
                .Include(s => s.Student)
                .Include(s => s.ReportResults)
                .Include(s => s.Class).ToList()
                .OrderBy(s => s.DisplayName);

            return this.RedirectToLoginIfNotAuthenticated(View(studentInClasses));
        }

        [HttpPost]
        public ActionResult Index(int currentPeriodId = 1, int currentYear = 2012, int currentClassId = 1)
        {
            this.CurrentPeriodId = currentPeriodId;
            this.CurrentYear = currentYear;
            this.CurrentClassId = currentClassId;

            return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string password)
        {
            return this.Authenticate(password);
        }
        
        public ActionResult Print(ReportTypes reportType = 0, int currentPeriodId = 1, int currentYear = 2012, int currentClassId = 0)
        {
            ViewBag.CurrentPeriod = db.Periods.Find(currentPeriodId);
            ViewBag.ReportType = reportType;

            var studentInClasses = db.StudentInClasses.Where(s => s.Year == currentYear)
                .Include(s => s.Student)
                .Include(s => s.ReportResults)
                .Include(s => s.Class).ToList()
                .Where(s => s.HasReportResultForPeriod(currentPeriodId, reportType));

            if (currentClassId != 0)
            {
                studentInClasses = studentInClasses.Where(s => s.ClassId == currentClassId);
            }

            studentInClasses = studentInClasses.OrderBy(s => s.DisplayName);

            return this.RedirectToLoginIfNotAuthenticated(View(studentInClasses));
        }

        public ActionResult Create(int studentInClassId = 0, ReportTypes reportType = 0, int currentPeriodId = 1)
        {
            StudentInClass currentStudent = db.StudentInClasses.Single(s => s.Id == studentInClassId);
            if (currentStudent.HasReportResultForPeriod(currentPeriodId, reportType))
            {
                throw new InvalidOperationException("Deze leerling heeft al een rapport voor deze periode!");
            }

            ViewBag.CurrentStudent = currentStudent;
            ViewBag.CurrentPeriod = db.Periods.Single(p => p.Id == currentPeriodId);

            ReportResult reportResult = currentStudent.CreateReportResultForPeriod(currentPeriodId, reportType, this.db);

            return this.RedirectToLoginIfNotAuthenticated(View(reportResult));
        }

        [HttpPost]
        public ActionResult Create(ReportResult reportResult)
        {
            if (ModelState.IsValid)
            {
                db.ReportResults.Add(reportResult);
                db.SaveChanges();

                return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
            }

            return this.RedirectToLoginIfNotAuthenticated(View(reportResult));
        }

        public ActionResult Edit(int id = 0)
        {
            ReportResult reportResult = db.ReportResults.Find(id);
            if (reportResult == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentStudent = db.StudentInClasses.Single(s => s.Id == reportResult.StudentInClassId);
            ViewBag.CurrentPeriod = db.Periods.Single(p => p.Id == reportResult.PeriodId);

            return this.RedirectToLoginIfNotAuthenticated(View(reportResult));
        }

        [HttpPost]
        public ActionResult Edit(ReportResult reportResult)
        {
            if (ModelState.IsValid)
            {
                foreach (TestResultGroup testResultGroup in reportResult.TestResultGroups)
                {
                    foreach (TestResult testResult in testResultGroup.TestResults)
                    {
                        if (testResult.Answers != null)
                        {
                            foreach (Answer answer in testResult.Answers)
                            {
                                db.Entry(answer).State = EntityState.Modified;
                            }
                        }

                        db.Entry(testResult).State = EntityState.Modified;
                    }

                    db.Entry(testResultGroup).State = EntityState.Modified;
                }

                db.Entry(reportResult).State = EntityState.Modified;
                db.SaveChanges();

                return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
            }

            return this.RedirectToLoginIfNotAuthenticated(View(reportResult));
        }

        public ActionResult Delete(int id = 0)
        {
            ReportResult reportResult = db.ReportResults.Find(id);
            if (reportResult == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentStudent = db.StudentInClasses.Single(s => s.Id == reportResult.StudentInClassId);
            ViewBag.CurrentPeriod = db.Periods.Single(p => p.Id == reportResult.PeriodId);

            return this.RedirectToLoginIfNotAuthenticated(View(reportResult));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportResult reportResult = db.ReportResults.Find(id);

            for (int i = reportResult.TestResultGroups.Count - 1; i >= 0; i--)
            {
                TestResultGroup testResultGroup = reportResult.TestResultGroups.ElementAt(i);
                for (int j = testResultGroup.TestResults.Count - 1; j >= 0; j--)
                {
                    TestResult testResult = testResultGroup.TestResults.ElementAt(j);
                    if (testResult.Answers != null)
                    {
                        for (int k = testResult.Answers.Count - 1; k >= 0; k--)
                        {
                            db.Answers.Remove(testResult.Answers.ElementAt(k));
                        }
                    }

                    db.TestResults.Remove(testResult);
                }

                db.TestResultGroups.Remove(testResultGroup);
            }

            db.ReportResults.Remove(reportResult);
            db.SaveChanges();

            return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}