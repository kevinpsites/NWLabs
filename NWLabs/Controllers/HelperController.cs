using NWLabs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWLabs.Controllers
{
    public class HelperController : Controller
    {
        private NWlabsContext db = new NWlabsContext();
        static Customer Customer = new Customer();
        public static List<Assay> OrderAssayList = new List<Assay>();
        
        
        public static string GetName()
        {
            string name;
            if (Customer != null)
            {
                name = Customer.contact.ContactFirstName + " " + Customer.contact.ContactLastName;
                return name;
            }
            else
            {
                name = null;
                return name;
            }
        }

        public static Customer GetCustomer()
        {
            return Customer;
        }

        public static Customer SetCustomer(Customer contact)
        {
            Customer = contact;
            return Customer;
        }

        public static string GetHash(string password)
        {
            password = password.ToLower();
            int sPassword = password.GetHashCode();
            password = sPassword.ToString();

            return password;
        }

        public static List<Assay> GetOrderAssayList()
        {
           return OrderAssayList;
        }

        public static List<Assay> SetOrderAssayList(Assay assay)
        {
            OrderAssayList.Add(assay);
            return OrderAssayList;
        }

    }
}