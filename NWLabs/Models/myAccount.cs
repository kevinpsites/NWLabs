using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWLabs.Models
{
    public class myAccount
    {
        public Account account { get; set; }
        public IEnumerable<Orders> orders { get; set; }
        
    }
}