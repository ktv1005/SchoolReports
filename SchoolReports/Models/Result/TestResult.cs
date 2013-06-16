namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;

    public class TestResult
    {
        public int Id { get; set; }

        public string Subject { get; set; }
        public string Theme { get; set; }

        public bool ShowWasAbsent { get; set; }

        public bool WasAbsent { get; set; }

        [DisplayName("Algemene opmerking")]
        public string RemarkText { get; set; }

        public string ResultText { get; set; }

        public Smiley ChosenSmiley { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public SelectList GetPossibleSmileys()
        {
            return new SelectList(SmileyOption.SmileyOptions(), "Smiley", "DisplayValue", this.ChosenSmiley);
        }

        public string GetSmileyImageFor(Smiley smiley)
        {
            string image = SmileyOption.GetImageForSmiley(smiley);

            if (this.ChosenSmiley == smiley)
            {
                return image;
            }

            return image.Insert(image.IndexOf("."), "_g");
        }

        public bool HasResult()
        {
            return this.WasAbsent || 
                   this.ChosenSmiley != SchoolReports.Models.Smiley.None ||
                   !string.IsNullOrEmpty(this.RemarkText) ||
                   !string.IsNullOrEmpty(this.ResultText) ||
                   string.IsNullOrEmpty(this.Subject);
        }
    }
}
