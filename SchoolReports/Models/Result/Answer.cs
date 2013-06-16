namespace SchoolReports.Models
{
    using System.ComponentModel;

    public class Answer
    {
        public int Id { get; set; }

        public bool Result { get; set; }
        public string QuestionText { get; set; }
        
        [DisplayName("Opmerking")]
        public string RemarkText { get; set; }

        public static string GetImageForResult(bool result)
        {
            if (result)
            {
                return "../Images/Print/cb_checked.png";
            }

            return "../Images/Print/cb_notchecked.png";
        }
    }
}
