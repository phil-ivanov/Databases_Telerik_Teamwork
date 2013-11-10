using System;

namespace LoadVendorExpenses
{
    public class MongoDbVendorExpense
    {
        public int ID { get; set; }

        public int VendorID { get; set; }

        public string VendorName { get; set; }

        public DateTime Month { get; set; }

        public decimal Expenses { get; set; }
    }
}
