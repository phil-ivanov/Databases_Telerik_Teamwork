using System;

namespace PdfReport
{
    public class MainApplication
    {
        private static readonly string directory = Settings.Default.TargetDirectory;

        public static void Main()
        {
            Console.WriteLine("Generating PDF sales report ...");
            PdfReportCreator.GenerateSalesReport(directory);
            Console.WriteLine("Pdf sales report generated successfully!");
        }
    }
}
