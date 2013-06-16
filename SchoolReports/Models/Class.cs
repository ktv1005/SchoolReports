namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Class
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Location { get; set; }

        public string Name
        {
            get
            {
                return "L" + this.Level + " " + this.Location;
            }
        }
    }
}
