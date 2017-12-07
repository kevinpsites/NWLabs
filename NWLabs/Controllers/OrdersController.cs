using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWLabs.Models;

namespace NWLabs.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private NWlabsContext db = new NWlabsContext();
        static Customer Customer = new Customer();

        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }

        //add an assay to an order
        public ActionResult AddOrder(int id)
        {
            Assay assay = new Assay();
            IEnumerable<Assay> eAssay;

            assay = db.Assays.Find(id);
            
            eAssay = HelperController.SetOrderAssayList(assay).AsEnumerable();

            return RedirectToAction("Order");
        }

        //remove an assay from an order
        public ActionResult RemoveOrder(int id)
        {
            Assay assay = new Assay();
            IEnumerable<Assay> eAssay;

            assay = db.Assays.Find(id);

            eAssay = HelperController.RemoveOrderAssayList(assay);

            return RedirectToAction("Order");
        }

        //view the order
        public ActionResult Order()
        {
           
            IEnumerable<Assay> eAssay;

            eAssay = HelperController.GetOrderAssayList();

            return View(eAssay);
        }

    }
}