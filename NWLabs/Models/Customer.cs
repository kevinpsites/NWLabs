using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWLabs.Models
{
    public class Customer
    {
        public Account account { get; set; }
        public Contact contact { get; set; }
        public Credential credential { get; set; }
        public Role role { get; set; }
    }
}