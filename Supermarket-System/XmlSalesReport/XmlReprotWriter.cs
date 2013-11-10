using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using MSSQLSupermarket.Data;

namespace XmlSalesReport
{
    public class XmlReporter
    {
        

        public void CreateXmlByVendors(string path)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            using (SupermarketEntities db = new SupermarketEntities())
            {
                var vendors = (from v in db.Vendors
                               join p in db.Products on v.ID equals p.VendorID
                               join s in db.SalesReports on p.ID equals s.ProductID
                               select v.VendorName).Distinct();


                var salesEntry = new XElement("sales");

                foreach (var companyName in vendors)
                {
                    var compayEntry = new XElement("sale");
                    compayEntry.SetAttributeValue("vendor", companyName);

                    var salesByDay = (from v in db.Vendors
                                      join p in db.Products on v.ID equals p.VendorID
                                      join s in db.SalesReports on p.ID equals s.ProductID
                                      group s by s.Date into y
                                      select new VendorReport
                                      {
                                          Date = y.Key,
                                          Sum = y.Sum(x => x.Sum)
                                      });

                    foreach (VendorReport dailyReport in salesByDay)
                    {
                        string dateFormated = dailyReport.Date.ToString("dd-MMM-yyyy");
                        AddDalyEarnings(compayEntry, dateFormated, dailyReport.Sum);
                    }

                    salesEntry.Add(compayEntry);
                }

                salesEntry.Save(path);
            }
        }

        private static void AddDalyEarnings(XElement compayEntry, string monthStr, decimal earnings)
        {
            XElement dataSumNode = new XElement("summary");
            dataSumNode.SetAttributeValue("date", monthStr);
            dataSumNode.SetAttributeValue("tatal-sum", earnings);
            compayEntry.Add(dataSumNode);
        }
    }
}