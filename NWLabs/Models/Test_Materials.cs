//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NWLabs.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Test_Materials
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test_Materials()
        {
            this.Invoices = new HashSet<Invoice>();
        }
    
        public int TestMaterialID { get; set; }
        public Nullable<int> TestID { get; set; }
        public Nullable<int> MaterialID { get; set; }
        public int Quantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual Material Material { get; set; }
        public virtual Test Test { get; set; }
    }
}
