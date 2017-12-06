using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWLabs.Models
{
    public class Catalog
    {
        public IEnumerable<Assay> assay { get; set; }
        public IEnumerable<Assay_Tests> assay_tests { get; set; }
    }
}