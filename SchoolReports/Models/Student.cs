namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Student
    {
        public int Id { get; set; }

        [DisplayName("Achternaam")]
        public string LastName { get; set; }

        [DisplayName("Voornaam")]
        public string FirstName { get; set; }

        public string FullName
        {
            get
            {
                return this.LastName + " " + this.FirstName;
            }
        }

        public virtual ICollection<StudentInClass> StudentInClasses { get; set; }
    }
}
