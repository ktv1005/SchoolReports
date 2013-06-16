namespace SchoolReports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Period
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int NumberSwim { get; set; }
        public int NumberGym { get; set; }

        public int GetNumberForReportType(ReportTypes reportType)
        {
            switch (reportType)
            {
                case ReportTypes.Swim:
                    return this.NumberSwim;
                case ReportTypes.Gym:
                    return this.NumberGym;
                default:
                    return 0;
            }
        }

        public static int GetCurrentPeriodId(IList<Period> periods)
        {
            int currentMonth = DateTime.Now.Month;

            switch (currentMonth)
            {
                case 1:
                case 2:
                case 3:
                    return periods.Single(p => p.NumberGym == 3).Id;
                case 4:
                case 5:
                case 6:
                    return periods.Single(p => p.NumberGym == 4).Id;
                case 7:
                case 8:
                case 9:
                    return periods.Single(p => p.NumberGym == 1).Id;
                case 10:
                case 11:
                case 12:
                    return periods.Single(p => p.NumberGym == 2).Id;
                default:
                    return 0;
            }
        }
    }
}
