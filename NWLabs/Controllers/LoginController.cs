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
            //collect information submitted by user
            String email = form["Email address"].ToString();
            String password = form["Password"].ToString();
            int sPassword;

            //hash password
            /* password = HelperController.GetHash(password);*/

            //find the credentials of the person if they exist
            Customer.credential = db.Credentials.SqlQuery(
                "Select * " +
                "FROM Credentials " +
                "Where Credentials.UserName = '" + email + "' AND Credentials.Password = '" + password + "'"
                ).FirstOrDefault();

            //depending on the role of the person logging in it will return their information whether customer or employee
            if (Customer.credential.RoleID == 7)
            {
                //find personal information
                Customer.contact = db.Contacts.SqlQuery(
                "Select * " +
                "FROM Contacts " +
                "inner join Credentials ON " +
                "Credentials.CredID = Contacts.CredID " +
                "Where Credentials.UserName = '" + email + "' AND Credentials.Password = '" + password + "'"
                ).FirstOrDefault();

                //gather remaining info needed for person
                Customer.credential = db.Credentials.Find(Customer.contact.CredID);
                Customer.role = db.Roles.Find(Customer.credential.RoleID);
                Customer.account = db.Accounts.Find(Customer.contact.AccountID);

                //check credentials and authenticate and set role typr for layout page
                if ((Customer.contact != null) && (string.Equals(email, Customer.credential.UserName)) && (string.Equals(password, Customer.credential.Password)))
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
            else if (Customer.credential.RoleID == 5)
            {
                //find employee
                Customer.employee = db.Employees.SqlQuery(
                "Select * " +
                "FROM Employees " +
                "inner join Credentials ON " +
                "Credentials.CredID = Employees.CredID " +
                "Where Credentials.UserName = '" + email + "' AND Credentials.Password = '" + password + "'"
                ).FirstOrDefault();

                //gather remaining info needed for person
                Customer.credential = db.Credentials.Find(Customer.employee.CredID);
                Customer.role = db.Roles.Find(Customer.credential.RoleID);

                //check credentials and authenticate and set role typr for layout page
                if ((Customer.employee != null) && (string.Equals(email, Customer.credential.UserName)) && (string.Equals(password, Customer.credential.Password)))
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

            //return login view if wrong
                return View();
            
        }

        //log user out and clear session
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        //register new user
        public ActionResult Register()
        {
           
            return RedirectToAction("Index");
        }

    }
}