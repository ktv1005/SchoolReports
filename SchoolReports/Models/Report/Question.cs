namespace SchoolReports.Models
{
    using System.Collections.Generic;

    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }
        
        public Answer CreateAnswers(ReportContext reportContext)
        {
            Answer answer = reportContext.Answers.Create();

            answer.QuestionText = this.Text;

            return answer;
        }
        
        public static List<Question> CreateQuestionsForSwim(ReportContext reportContext)
        {
            Question question1 = reportContext.Questions.Create();
            question1.Text = "ik kan dribbelen met de bal";

            Question question2 = reportContext.Questions.Create();
            question2.Text = "ik kan scoren in de ring";

            Question question3 = reportContext.Questions.Create();
            question3.Text = "ik kan de bal afpakken";

            return new List<Question> { question1, question2, question3 };
        }
        
        public static List<Question> CreateQuestionsForGymSkill(ReportContext reportContext)
        {
            Question question1 = reportContext.Questions.Create();
            question1.Text = string.Empty;

            Question question2 = reportContext.Questions.Create();
            question2.Text = string.Empty;

            Question question3 = reportContext.Questions.Create();
            question3.Text = string.Empty;

            return new List<Question> { question1, question2, question3 };
        }

        public static List<Question> CreateQuestionsForGymAttitude(ReportContext reportContext)
        {
            Question question1 = reportContext.Questions.Create();
            question1.Text = "in orde zijn met turnkledij, attesten en briefwisseling";

            Question question2 = reportContext.Questions.Create();
            question2.Text = "afspraken, werkgewoonten en spelregels naleven";

            Question question3 = reportContext.Questions.Create();
            question3.Text = "in teamverband werken en spelen";

            return new List<Question> { question1, question2, question3 };
        }
    }
}
