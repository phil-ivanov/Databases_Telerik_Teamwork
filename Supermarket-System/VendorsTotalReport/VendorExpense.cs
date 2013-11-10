using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace VendorsTotalReport
{
    public class VendorExpense
    {
        public long ID { get; set; }

        public long VendorID { get; set; }

        public string VendorName { get; set; }

        public DateTime Month { get; set; }

        public decimal Expenses { get; set; }

        [BsonId]
        public object ObjectId { get; set; }

        public VendorExpense(long id, long vendorId, string vendorName, DateTime month, decimal expenses)
        {
            this.ID = id;
            this.VendorID = vendorId;
            this.VendorName = vendorName;
            this.Month = month;
            this.Expenses = expenses;
        }
    }
}
