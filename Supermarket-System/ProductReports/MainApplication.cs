using System;
using System.Linq;
using MSSQLSupermarket.Data;

namespace ProductReports
{
    class MainApplication
    {
        private static readonly string connectionString = Settings.Default.MongoConnectionString;
        private static readonly string reportsDirectoryPath = Settings.Default.ReportsTargetDirectory;

        static void Main()
        {
            CreateAndSaveProductsReports();
        }

        private static void CreateAndSaveProductsReports()
        {
            ReportGenerator reportGenerator = new ReportGenerator();
            ReportManager reportManager = new ReportManager();

            using (var context = new SupermarketEntities())
            {
                var allProducts = context.Products.Select(x => x.ID);

                foreach (var id in allProducts)
                {
                    Report report = reportGenerator.Generate(id);
                    reportManager.SaveToFileSystemAsJson(report, reportsDirectoryPath);
                    reportManager.SaveToMongoDB(report, connectionString);

                    Console.WriteLine("Report for product with ID {0} created and saved!", id);
                }
            }
        }
    }
}
