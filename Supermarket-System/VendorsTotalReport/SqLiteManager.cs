using System;
using System.Data.SQLite;
using System.Linq;
using MongoDB.Driver;
using SQLiteSupermarket.Data;

namespace VendorsTotalReport
{
    public class SqLiteManager
    {
        private string connectionString;


        public SqLiteManager(string connectionString)
        {
            this.ConnectionString = connectionString;
        }


        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid conneection string! It cannot be null!");
                }
                else if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
                {
                    throw new ArgumentException("Invalid conneection string! It cannot be empty or contain only white spaces!");
                }

                this.connectionString = value;
            }
        }

        public void TransferProductReportsFromMongoDb(string mongoConnectionString, string databaseName, string collectionName, string tableName)
        {
            if (mongoConnectionString == null)
            {
                throw new ArgumentNullException("Invalid conneection string! It cannot be null!");
            }
            else if (string.IsNullOrWhiteSpace(mongoConnectionString) || mongoConnectionString == string.Empty)
            {
                throw new ArgumentException("Invalid conneection string! It cannot be empty or contain only white spaces!");
            }

            if (databaseName == null)
            {
                throw new ArgumentNullException("Invalid database name! It cannot be null!");
            }
            else if (string.IsNullOrWhiteSpace(databaseName) || databaseName == string.Empty)
            {
                throw new ArgumentException("Invalid database name! It cannot be empty or contain only white spaces!");
            }

            if (collectionName == null)
            {
                throw new ArgumentNullException("Invalid collection name! It cannot be null!");
            }
            else if (string.IsNullOrWhiteSpace(collectionName) || collectionName == string.Empty)
            {
                throw new ArgumentException("Invalid conneection string! It cannot be empty or contain only white spaces!");
            }

            if (tableName == null)
            {
                throw new ArgumentNullException("Invalid table name! It cannot be null!");
            }
            else if (string.IsNullOrWhiteSpace(tableName) || tableName == string.Empty)
            {
                throw new ArgumentException("Invalid table string! It cannot be empty or contain only white spaces!");
            }

            var client = new MongoClient(mongoConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(databaseName);
            var collection = database.GetCollection(collectionName);

            var reports = collection.FindAllAs<Report>();

            using (var sqLiteContext = new SQLiteEntities())
            {
                 string sql = "INSERT INTO " + tableName + "(ProductID, ProductName, VendorName, TotalQuantitySold, TotalIncomes) "
                    + " VALUES({0}, {1}, {2}, {3}, {4})";

                foreach (var report in reports)
                {
                    sqLiteContext.Database.ExecuteSqlCommand(sql,
                        report.ProductId,
                        report.ProductName,
                        report.VendorName,
                        report.TotalQuantitySold,
                        report.TotalIncomes);
                }

                sqLiteContext.SaveChanges();
            }
        }
    }
}
