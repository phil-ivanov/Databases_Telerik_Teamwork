using System;
using System.Collections.Generic;
using System.Linq;
using MSSQLSupermarket.Data;

namespace PdfReport
{
    public static class MSSQLReportDataRetriever
    {
        public static List<Sale> GetSalesData()
        {
            SupermarketEntities context = new SupermarketEntities();

            var query = (from s in context.SalesReports
                         join c in context.Products on s.ProductID equals c.ID
                         join e in context.Measures on c.MeasureID equals e.ID
                         orderby s.Date
                         select new { ProductName = c.ProductName, Quantity = s.Quantity, UnitPrice = s.UnitPrice, Location = s.Location, Sum = s.Sum, Date = s.Date, Mesure = e.MeasureName }).ToList();

            List<Sale> sales = new List<Sale>();

            foreach (var item in query)
            {
                Sale current = new Sale(item.ProductName, item.Quantity, item.UnitPrice, item.Location, item.Sum, item.Date, item.Mesure);
                sales.Add(current);
            }

            return sales;
        }

    }
}
