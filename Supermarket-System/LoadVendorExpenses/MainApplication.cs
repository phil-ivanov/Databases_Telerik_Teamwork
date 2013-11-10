using System;

namespace LoadVendorExpenses
{
    class MainApplication
    {
        private static readonly string connectionString = Settings.Default.MongoConnectionString;
        private static readonly string xmlLocation = Settings.Default.XmlLocation;

        static void Main()
        {
            XmlDataTransferer transferManager = new XmlDataTransferer(xmlLocation);

            Console.WriteLine("Transfering vendor expenses to MS SQL ...");
            transferManager.TransferExpensesToMssql();
            Console.WriteLine("Vendor expenses transferred successfully!");
            Console.WriteLine();

            Console.WriteLine("Transfering vendor expenses to MongoDb ...");
            transferManager.TransferExpensesToMongoDb(connectionString);
            Console.WriteLine("Vendor expenses transferred successfully!");
        }
    }
}
