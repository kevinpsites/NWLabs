using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWLabs.Models
{
    public class myOrders
    {
        public Order order { get; set; }
        public Status status { get; set; }
        public Order_Details order_Details { get; set; }
        public IEnumerable<Assay_Tests> assay_tests { get; set; }

    }
}