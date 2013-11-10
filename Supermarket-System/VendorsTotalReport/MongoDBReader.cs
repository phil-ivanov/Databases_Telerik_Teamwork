using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace VendorsTotalReport
{
    public class MongoDBReader
    {

        private readonly string mongoConnectionString = Settings.Default.MongoConnectionString;
        private readonly string databaseName = Settings.Default.MongoDatabaseName;
        private readonly string reportCollectionName = Settings.Default.MongoCollectionName;
        private readonly string vendorCollectionName = Settings.Default.MongoVendorCollectionName;

        public List<Report> GetProductReports()
        {
            var client = new MongoClient(mongoConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(databaseName);
            var productReports = database.GetCollection(reportCollectionName);

            List<Report> result = new List<Report>();
            var reportsLocal = productReports.FindAllAs<Report>();

            foreach (var report in reportsLocal)
            {
                result.Add(report);
            }

            return result;
        }

        public List<VendorExpense> GetVendorExpenses()
        {
            var client = new MongoClient(mongoConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(databaseName);
            var vendorExpenses = database.GetCollection(vendorCollectionName);

            DateTime current = DateTime.Now;

            List<VendorExpense> result = new List<VendorExpense>();
            var expensesLocal = vendorExpenses.FindAllAs<VendorExpense>();

            foreach (var expense in expensesLocal)
            {
                result.Add(expense);
            }

            return result;
        }
    }
}
