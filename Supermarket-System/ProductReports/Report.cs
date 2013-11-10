using System;
using System.Runtime.Serialization;

namespace ProductReports
{
    [DataContract]
    public class Report
    {
        public Report(int productId, string productName, string vendorName, int totalQuantitySold, decimal totalIncomes)
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.VendorName = vendorName;
            this.TotalQuantitySold = totalQuantitySold;
            this.TotalIncomes = totalIncomes;
        }

        [DataMember]
        public int ProductId { get; private set; }
        
        [DataMember]
        public string ProductName { get; private set; }

        [DataMember]
        public string VendorName { get; private set; }

        [DataMember]
        public int TotalQuantitySold { get; private set; }

        [DataMember]
        public decimal TotalIncomes { get; private set; }
    }
}
