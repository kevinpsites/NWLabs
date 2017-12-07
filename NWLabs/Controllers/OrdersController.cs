using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWLabs.Models;

namespace NWLabs.Controllers
{
    public class OrdersController : Controller
    {
        private NWlabsContext db = new NWlabsContext();
        static Customer Customer = new Customer();

        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult AddOrder(int id)
        {
            Assay assay = new Assay();
            IEnumerable<Assay> eAssay;

            assay = db.Assays.Find(id);


            eAssay = HelperController.SetOrderAssayList(assay).AsEnumerable();



            return View(eAssay);
        }
    }
}