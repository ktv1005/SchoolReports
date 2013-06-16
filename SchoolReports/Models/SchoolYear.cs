namespace SchoolReports.Models
{
    using System;

    public class SchoolYear
    {
        #region Properties

        public int Year
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return this.Year + " - " + (this.Year + 1);
            }
        }

        public static int GetCurrentSchoolYear()
        {
            int currentYear = DateTime.UtcNow.Year;

            if (DateTime.UtcNow.Month <= 8)
            {
                currentYear = currentYear - 1;
            }

            return currentYear;
        }

        #endregion Properties
    }
}