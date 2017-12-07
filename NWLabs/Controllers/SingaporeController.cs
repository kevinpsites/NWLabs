using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWLabs.Controllers
{
    public class SingaporeController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        // record info from order recieved.
        public ActionResult Recieving()
        {

            return View();
        }
        // virew receiving info for the order
        public ActionResult RecievingInformation()
        {

            return View();
        }


    }

   
    
       
    

}