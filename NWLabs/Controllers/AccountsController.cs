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
    [Authorize]
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

        //view account of customer
        public ActionResult myAccount()
        {
            myAccount myAccount = new myAccount();

            List<Order> orderList = new List<Order>();

            List<Orders> AccountOrders = new List<Orders>();
            
            
            //find account based on id stored at login
            myAccount.account = db.Accounts.Find(HelperController.GetCustomer().account.AccountID);

            //find all the orders for account
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

        //view all account orders
        public ActionResult myOrders(int id)
        {
            myOrders myOrder = new myOrders();

            //find order from ID
            myOrder.order = db.Orders.Find(id);
            myOrder.status = db.Status.Find(myOrder.order.OrderStatusID);

            //variables used
            List<Assay> myDBAssayList = new List<Assay>();
            List<Assay_Tests> orderAssayList = new List<Assay_Tests>();
            List<TestsStatus> myTestsStatus = new List<TestsStatus>();
            decimal? materialCost = new decimal?();
            decimal? quantity = new decimal?();
            decimal? totalMaterialCost = new decimal?();
            decimal? empWage = new decimal?();
            decimal? time = new decimal?();
            decimal? assayTotal = new decimal?();
            List<decimal?> assayTotalList = new List<decimal?>();
            decimal? orderTotal = new decimal?();
            orderTotal = 0;

            //find each assay in each order
            foreach (var AssayorderDetails in myOrder.order.Order_Details)
            {
                assayTotalList.Clear();
                foreach (var testOrderDetails in AssayorderDetails.Test_Order_Details)
                {
                    //find all tests for each assay

                    //get material cost for each test - could be multiple
                    totalMaterialCost = 0;
                    foreach (var testMaterials in testOrderDetails.Test.Test_Materials)
                    {
                        materialCost = testMaterials.Material.MaterialCost;
                        quantity = (decimal)testMaterials.Quantity;

                        totalMaterialCost += materialCost * quantity;
                    }

                    //find employee wage
                    empWage = 0;
                    foreach (var empTest in testOrderDetails.Employee_Tests)
                    {
                        empWage += empTest.Employee.Title.Wage;
                       
                    }
                    
                    //find time for each test
                    time = ((decimal)testOrderDetails.EndTime.Value.Hour) - ((decimal)testOrderDetails.StartTime.Value.Hour);

                    //add them upp and store them
                    assayTotal = (time * empWage.GetValueOrDefault(0m)) + totalMaterialCost;
                    assayTotalList.Add(assayTotal);
                }
               
                //make a total amount for the order by summing assays
                foreach (var assay in assayTotalList)
                {
                    orderTotal += assay;
                }
            }

            //invice total based on sum of order totals
            foreach (var item in myOrder.order.Invoices)
            {
                item.InvoiceTotal = orderTotal;
            }
            
            return View(myOrder);
        }
    }
}
