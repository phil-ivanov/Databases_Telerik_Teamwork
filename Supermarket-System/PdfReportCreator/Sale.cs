using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfReport
{
    public class Sale
    {
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string Location { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }

        public string Mesure { get; set; }

        public Sale(string productName, int quantity, decimal unitPrice, string location, decimal sum, DateTime date, string mesure)
        {
            this.ProductName = productName;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.Location = location;
            this.Sum = sum;
            this.Date = date;
            this.Mesure = mesure;
        }
    }
}
