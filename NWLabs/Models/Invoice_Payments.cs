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
    
    public partial class Invoice_Payments
    {
        public int InvoicePaymentID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> PaymentID { get; set; }
        public Nullable<decimal> AmountApplied { get; set; }
    
        public virtual Invoice Invoice { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
