using System;

namespace VendorsTotalReport
{
    class MainApplication
    {
        private static readonly string connectionString = Settings.Default.SqLiteConnectionString;
        private static readonly string mongoConnectionString = Settings.Default.MongoConnectionString;
        private static readonly string databaseName = Settings.Default.MongoDatabaseName;
        private static readonly string collectionName = Settings.Default.MongoCollectionName;
        private static readonly string sqLiteTableName = Settings.Default.SqliteTableName;

        static void Main()
        {

            SqLiteManager sqliteManager = new SqLiteManager(connectionString);
            Console.WriteLine("Transfering product reports from MnogoDb to SQLite...");
            sqliteManager.TransferProductReportsFromMongoDb(mongoConnectionString, databaseName, collectionName, sqLiteTableName);
            Console.WriteLine("Product reports transferred successfully!");


            var mongodbReader = new MongoDBReader();
            var sqliteReader = new SQLiteReader();

            var expenses = mongodbReader.GetVendorExpenses();
            var reports = mongodbReader.GetProductReports();
            var taxes = sqliteReader.GetProductTaxes();

            Console.WriteLine("Generating total vendors report for the current month ...");
            ExelGenerator.GenerateExelVendorReport(expenses, reports, taxes);
            Console.WriteLine("Total vendors report generated succesfully!");
        }
    }
}
