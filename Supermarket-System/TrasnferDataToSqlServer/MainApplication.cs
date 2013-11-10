using System;
using System.Linq;

namespace TrasnferDataToSqlServer
{
    class MainApplication
    {
        static void Main(string[] args)
        {
            MySqlDataTransferer mySqlTransferer = new MySqlDataTransferer();
            Console.WriteLine("Transfering data from MySQL...");
            mySqlTransferer.Transfer();
            Console.WriteLine("Data from MySQL transfered successfully!");
            Console.WriteLine();

            Console.WriteLine("Transfering data from ExcelDocuments...");
            ExcelDataTransferer excelTransferer = new ExcelDataTransferer();
            Console.WriteLine("Data from Excel documents transfered successfully!");
            excelTransferer.Transfer();
        }
    }
}
