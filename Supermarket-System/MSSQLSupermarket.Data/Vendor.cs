//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSSQLSupermarket.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vendor
    {
        public Vendor()
        {
            this.Products = new HashSet<Product>();
            this.VendorExpenses = new HashSet<VendorExpens>();
        }
    
        public int ID { get; set; }
        public string VendorName { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<VendorExpens> VendorExpenses { get; set; }
    }
}
