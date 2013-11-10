using System;
using System.Data;
using System.IO;
using System.Linq;
using Ionic.Zip;
using MSSQLSupermarket.Data;

namespace TrasnferDataToSqlServer
{
    public class ExcelDataTransferer
    {
        private readonly string zipName = Settings.Default.ZipName;
        private readonly string zipLocation = Settings.Default.ZipLocation;
        private readonly string extractFolderName = Settings.Default.ExtractFolderName;

        public void Transfer()
        {
            using (var context = new SupermarketEntities())
            {
                try
                {
                    ZipFile zipToExtract = ZipFile.Read(zipLocation + zipName + ".zip");
                    Directory.CreateDirectory(extractFolderName);
                    zipToExtract.ExtractAll(extractFolderName);

                    string[] allReportDirs = Directory.GetDirectories(extractFolderName);

                    foreach (var folder in allReportDirs)
                    {
                        string[] allReports = Directory.GetFiles(folder);
                        string dateString = GetFolderName(folder);
                        DateTime date = DateTime.Parse(dateString);

                        foreach (var report in allReports)
                        {
                            DataTable excelTable = ExcelToTableData.GetTable(report, "Sales");
                            string location = excelTable.Rows[0][0].ToString();

                            for (int i = 2; i < excelTable.Rows.Count - 2; i++)
                            {
                                var currentRow = excelTable.Rows[i];

                                SalesReport currentReport = new SalesReport();
                                currentReport.ProductID = int.Parse(currentRow[0].ToString());
                                currentReport.Quantity = int.Parse(currentRow[1].ToString());
                                currentReport.UnitPrice = decimal.Parse(currentRow[2].ToString());
                                currentReport.Sum = decimal.Parse(currentRow[3].ToString());
                                currentReport.Date = date;
                                currentReport.Location = location;
                                context.SalesReports.Add(currentReport);
                            }
                        }
                    }

                    context.SaveChanges();
                    RemoveExtractFolder(extractFolderName);
                }
                catch (Exception)
                {
                    RemoveExtractFolder(extractFolderName);
                }
            }
        }

        private void RemoveExtractFolder(string zipExtractFolder)
        {
            DirectoryInfo extractFolder = new DirectoryInfo(zipExtractFolder);
            extractFolder.EmptyDirectory();
            Directory.Delete(zipExtractFolder);
        }

        private string GetFolderName(string path)
        {
            int lastSlashIndex = path.LastIndexOf("\\");
            string folderName = path.Substring(lastSlashIndex + 1);

            return folderName;
        }
    }
}
