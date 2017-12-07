using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWLabs.Controllers
{
    public class SeattleController : Controller
    {
        // GET: Seattle
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderForm()
        {
            return View();
        }

        public ActionResult SummaryReport()
        {
            return View();
        }
    }
}