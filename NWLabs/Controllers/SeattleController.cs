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

        //return order form for order
        public ActionResult OrderForm()
        {
            return View();
        }

        //view summary report for order
        public ActionResult SummaryReport()
        {
            return View();
        }
    }
}