using NWLabs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NWLabs.Controllers
{
    public class LoginController : Controller
    {
        private NWlabsContext db = new NWlabsContext();
        static Customer Customer = new Customer();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, bool rememberMe = false)
        {
            String email = form["Email address"].ToString();
            String password = form["Password"].ToString();
            int sPassword;

            /* password = HelperController.GetHash(password);*/

            Customer.contact = db.Contacts.SqlQuery(
                "Select * " +
                "FROM Contacts " +
                "inner join Credentials ON " +
                "Credentials.CredID = Contacts.CredID " +
                "Where Credentials.UserName = '" + email + "' AND Credentials.Password = '" + password + "'"
                ).FirstOrDefault();

            Customer.credential = db.Credentials.Find(Customer.contact.CredID);
            Customer.role = db.Roles.Find(Customer.credential.RoleID);
            Customer.account = db.Accounts.Find(Customer.contact.AccountID);

           /* sPassword = db.Owners.SqlQuery(
                 "Select ownerID, ownerLastName, ownerFirstName, ownerEmail, ownerPassword " +
                "FROM Owner " +
                "Where Owner.ownerEmail = '" + email + "' AND Owner.ownerPassword = '" + password + "'"
                 ).Count(); */

            if ((Customer.contact != null) && (string.Equals(email, Customer.credential.UserName)) && (string.Equals(password,Customer.credential.Password)))
            {
                FormsAuthentication.SetAuthCookie(Customer.role.RoleDesc, rememberMe);
                HelperController.SetCustomer(Customer);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
           
            return RedirectToAction("Index");
        }

    }
}