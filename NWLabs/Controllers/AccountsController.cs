using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NWLabs.Models;

namespace NWLabs.Controllers
{
    public class AccountsController : Controller
    {
        private NWlabsContext db = new NWlabsContext();

        // GET: Accounts
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountID,CompanyName,Address,Address2,City,StateOrProvince,Country,Zip,Balance")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,CompanyName,Address,Address2,City,StateOrProvince,Country,Zip,Balance")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult myAccount()
        {
            myAccount myAccount = new myAccount();

            List<Order> orderList = new List<Order>();

            List<Orders> AccountOrders = new List<Orders>();
            
            
            
            myAccount.account = db.Accounts.Find(HelperController.GetCustomer().account.AccountID);

            if (myAccount.account != null)
            {

               orderList = db.Database.SqlQuery<Order>(
                   "SELECT * " +
                   "FROM Orders " +
                   "Where Orders.AccountID = '" + myAccount.account.AccountID + "'").ToList();
                foreach (var orderListItem in orderList)
                {
                    Orders orders = new Orders();
                    orders.order = db.Orders.Find(orderListItem.OrderID);
                    orders.status = db.Status.Find(orderListItem.OrderStatusID);
                    AccountOrders.Add(orders);
                }

                myAccount.orders = AccountOrders.AsEnumerable();

                return View(myAccount);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }

        public ActionResult myOrders(int id)
        {
            myOrders myOrder = new myOrders();

            //find order from ID
            myOrder.order = db.Orders.Find(id);
            myOrder.status = db.Status.Find(myOrder.order.OrderStatusID);

            List<Assay> myDBAssayList = new List<Assay>();
            List<Assay_Tests> orderAssayList = new List<Assay_Tests>();
            List<TestsStatus> myTestsStatus = new List<TestsStatus>();

            //find all the assays that are associated with that order
            myDBAssayList = db.Database.SqlQuery<Assay>(
                "Select * " +
                "From Assays " +
                "inner join Order_Details on " +
                "Assays.AssayID = Order_Details.AssayID " +
                "Where Order_Details.OrderID = '" + id + "'"
                ).ToList();
            
            //create the individual assay details for every array found and the status of each assay
            foreach (var item in myDBAssayList)
            {
                Assay_Tests atest = new Assay_Tests();
                atest.assay = db.Assays.Find(item.AssayID);
                int statusnumber =  db.Database.SqlQuery<int>(
                    "Select OrderDetailStatus " +
                    "FROM Order_Details " +
                    "Where OrderID = '" + myOrder.order.OrderID + "' AND AssayID = '" + atest.assay.AssayID + "'"
                    ).FirstOrDefault();
                
                atest.status = db.Status.Find(statusnumber);
               
                orderAssayList.Add(atest);
            }

            myOrder.assay_tests = orderAssayList.ToList();

            //add test details for every individual assay
            foreach (var item in myOrder.assay_tests)
            {
                List<Test> DBTest = new List<Test>();
                
                DBTest = db.Tests.SqlQuery("Select * " +
               "FROM Tests " +
               "INNER JOIN Assay_Tests ON " +
               "Assay_Tests.TestID = Tests.TestID " +
               "Inner Join Assays ON " +
               "Assays.AssayID = Assay_Tests.AssayID " +
               "Where Assays.AssayID = '" + item.assay.AssayID + "'").ToList();

                foreach (var test in DBTest)
                {
                    TestsStatus myTests = new TestsStatus();
                    myTests.test = db.Tests.Find(test.TestID);
                    int testStatusID = db.Database.SqlQuery<int>(
                        "Select TestStatusID " +
                        "From Test_Order_Details " +
                        "inner join Order_Details ON " +
                        "Order_Details.OrderDetailID = Test_Order_Details.OrderDetailID " +
                        "Where Order_Details.OrderID = '" + myOrder.order.OrderID + "' AND " +
                        "Order_Details.AssayID = '" + item.assay.AssayID + "' AND " +
                        "Test_Order_Details.TestID = '" + myTests.test.TestID + "'"
                        ).FirstOrDefault();

                    myTests.status = db.Status.Find(testStatusID);

                    myTestsStatus.Add(myTests);
                }

                item.testsStatus = myTestsStatus.AsEnumerable();
            }

            

            
            return View(myOrder);
        }
    }
}
