using NWLabs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWLabs.Controllers
{
    public class HomeController : Controller
    {
        private NWlabsContext db = new NWlabsContext();
        static Customer Customer = new Customer();

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //view the catalog of the assays
        public ActionResult Catalog()
        {
            Catalog catalog = new Catalog();
            
            List<Assay_Tests> assayList = new List<Assay_Tests>();

            //take all assays and put them as a list.
            catalog.assay = db.Assays.ToList();

            //break down each assay to individual assay and tests
            foreach (var item in catalog.assay)
            {
                Assay_Tests atest = new Assay_Tests();
                atest.assay = db.Assays.Find(item.AssayID);
                assayList.Add(atest);
            }

            //for each assay find all the individual tests required
            foreach (var item in assayList)
            {
                item.test = db.Tests.SqlQuery("Select * " +
               "FROM Tests " +
               "INNER JOIN Assay_Tests ON " +
               "Assay_Tests.TestID = Tests.TestID " +
               "Inner Join Assays ON " +
               "Assays.AssayID = Assay_Tests.AssayID " +
               "Where Assays.AssayID = '" + item.assay.AssayID + "'").AsEnumerable();
            }

            //add all the test and assay information to the catalog model
            catalog.assay_tests = assayList.ToList();
       
            
            return View(catalog);
        }
    }
}