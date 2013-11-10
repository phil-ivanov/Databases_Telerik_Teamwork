using System;

namespace XmlSalesReport
{
    class MainApplication
    {
        private static readonly string path = Settings.Default.DestinationPath + Settings.Default.FileName;

        public static void Main()
        {
            XmlReporter xmlReporter = new XmlReporter();

            Console.WriteLine("Generating XML report by vendors ...");
            xmlReporter.CreateXmlByVendors(path);
            Console.WriteLine("XML report by vendors generated successfully!");
        }
    }
}
