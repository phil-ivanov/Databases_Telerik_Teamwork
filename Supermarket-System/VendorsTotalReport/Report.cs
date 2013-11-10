using System;
using MongoDB.Bson.Serialization.Attributes;

namespace VendorsTotalReport
{
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

        [BsonId]
        public object ObjectId { get; set; }

        public int ProductId { get; private set; }
        
        public string ProductName { get; private set; }

        public string VendorName { get; private set; }

        public int TotalQuantitySold { get; private set; }

        public decimal TotalIncomes { get; private set; }
    }
}
