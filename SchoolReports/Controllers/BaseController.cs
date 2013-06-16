namespace SchoolReports.Controllers
{
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        private const string password = "015422805";

        private bool IsAuthenticated
        {
            get
            {
                if (Session["IsAuthenticated"] == null)
                {
                    Session["IsAuthenticated"] = false;
                }

                return (bool)Session["IsAuthenticated"];
            }
            set
            {
                Session["IsAuthenticated"] = value;
            }
        }

        public ActionResult RedirectToLoginIfNotAuthenticated(ActionResult pageToNavigateTo)
        {
#if !DEBUG
            if (!this.IsAuthenticated)
            {                
                return RedirectToAction("Login", "Report");
            }
#endif

            return pageToNavigateTo;
        }

        public ActionResult Authenticate(string typedPassword)
        {
            this.IsAuthenticated = (typedPassword == password);

#if !DEBUG
            if (!this.IsAuthenticated)
            {
                return RedirectToAction("Login", "Report");
            }
#endif

            return RedirectToAction("Index", "Report");
        }
        
        public ActionResult Logout()
        {
            this.IsAuthenticated = false;

            return RedirectToAction("Login", "Report");
        }
    }
}