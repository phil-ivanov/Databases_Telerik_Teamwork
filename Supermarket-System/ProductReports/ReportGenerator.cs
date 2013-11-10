using System;
using System.Linq;
using MSSQLSupermarket.Data;

namespace ProductReports
{
    public class ReportGenerator
    {
        public Report Generate(int productId)
        {
            using (var context = new SupermarketEntities())
            {
                var product = context.Products.Include("Vendor").Where(x => x.ID == productId).FirstOrDefault();

                int totalQuantitySold = 0;
                decimal totalIncomes = 0M;

                try
                {
                    totalQuantitySold = context.SalesReports.Where(x => x.ProductID == productId).Sum(x => x.Quantity);
                    totalIncomes = context.SalesReports.Where(x => x.ProductID == productId).Sum(x => x.Sum);
                }
                catch (InvalidOperationException)
                {
                    // There aren't any sales for this report - so set the total incomes and quantity to zero
                    totalQuantitySold = 0;
                    totalIncomes = 0M;
                }

                Report report = new Report(product.ID, product.ProductName, product.Vendor.VendorName, totalQuantitySold, totalIncomes);

                return report;
            }
        }
    }
}
