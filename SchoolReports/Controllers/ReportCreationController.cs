namespace SchoolReports.Controllers
{
    using SchoolReports.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;

    public class ReportCreationController : BaseController
    {
        private ReportContext db = new ReportContext();

        public ActionResult Index()
        {
            ViewBag.Periods = db.Periods.OrderBy(p => p.NumberGym).ToList();
            ViewBag.Levels = Enumerable.Range(1, 6);

            return this.RedirectToLoginIfNotAuthenticated(View(db.Reports.ToList()));
        }

        public ActionResult Create(int level, ReportTypes reportType, int currentPeriodId)
        {
            Report existingReport = db.Reports.SingleOrDefault(r => r.Level == level &&
                                                                    r.ReportType == reportType &&
                                                                    r.PeriodId == currentPeriodId);
            if (existingReport != null)
            {
                throw new InvalidOperationException("Dit leerjaar heeft al een rapport voor deze periode!");
            }

            Report report = Report.CreateReport(level, reportType, currentPeriodId, this.db);

            return this.RedirectToLoginIfNotAuthenticated(View(report));
        }

        [HttpPost]
        public ActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();

                return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
            }

            return this.RedirectToLoginIfNotAuthenticated(View(report));
        }

        public ActionResult Edit(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }

            return this.RedirectToLoginIfNotAuthenticated(View(report));
        }

        [HttpPost]
        public ActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                foreach (var testGroup in report.TestGroups)
                {
                    foreach (var test in testGroup.Tests)
                    {
                        foreach (var question in test.Questions)
                        {
                            db.Entry(question).State = EntityState.Modified;
                        }

                        db.Entry(test).State = EntityState.Modified;
                    }

                    db.Entry(testGroup).State = EntityState.Modified;
                }

                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();

                return this.RedirectToLoginIfNotAuthenticated(RedirectToAction("Index"));
            }

            return this.RedirectToLoginIfNotAuthenticated(View(report));
        }

        public ActionResult Delete(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }

            return this.RedirectToLoginIfNotAuthenticated(View(report));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);

            for (int i = report.TestGroups.Count - 1; i >= 0; i--)
            {
                var testGroup = report.TestGroups.ElementAt(i);
                for (int j = testGroup.Tests.Count - 1; j >= 0; j--)
                {
                    var test = testGroup.Tests.ElementAt(j);
                    for (int k = test.Questions.Count - 1; k >= 0; k--)
                    {
                        db.Questions.Remove(test.Questions.ElementAt(k));
                    }

                    db.Tests.Remove(test);
                }

                db.TestGroups.Remove(testGroup);
            }

            db.Reports.Remove(report);
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