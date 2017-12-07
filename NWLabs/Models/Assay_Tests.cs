using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWLabs.Models
{
    public class Assay_Tests
    {
        public Assay assay { get; set; }
        public IEnumerable<TestsStatus> testsStatus { get; set; }
        public IEnumerable<Test> test { get; set; }
        public Status status { get; set; }
    }
}