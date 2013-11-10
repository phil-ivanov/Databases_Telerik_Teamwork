using System;
using System.Collections.Generic;
using System.Linq;
using MSSQLSupermarket.Data;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace PdfReport
{
    public static class PdfReportCreator
    {
        private static Phrase[] reportCellHeaders = { new Phrase("Product"), new Phrase("Quantity"), new Phrase("Unit Price"), new Phrase("Location"), new Phrase("Sum") };
        private static readonly int NUMBER_OF_ROWS = 5;

        public static void GenerateSalesReport(string directory)
        {
            List<Sale> sales = MSSQLReportDataRetriever.GetSalesData();

            byte[] pdfReport = GetPdfBitArray(sales);

            // Write out PDF from memory stream. 
            using (FileStream fs = File.Create(directory + @"\Sales-Report.pdf"))
            {
                fs.Write(pdfReport, 0, (int)pdfReport.Length);
            }
        }

        private static List<DateTime> GetDifferentDates(List<Sale> sales)
        {
            List<DateTime> dates = new List<DateTime>();

            foreach (var item in sales)
            {
                if (dates.Count == 0)
                {
                    dates.Add(item.Date);
                    continue;
                }
                else if (dates[dates.Count - 1] != item.Date)
                {
                    dates.Add(item.Date);
                }
            }

            return dates;
        }

        private static byte[] GetPdfBitArray(List<Sale> sales)
        {
            byte[] result;

            using (MemoryStream stream = new MemoryStream())
            {
                Document myDocument = new Document();
                PdfWriter myPDFWriter = PdfWriter.GetInstance(myDocument, stream);

                myDocument.Open();

                List<DateTime> dates = GetDifferentDates(sales);

                PdfPTable reportTable = BuildTable(sales, dates);

                myDocument.Add(reportTable);
                myDocument.Close();

                result = stream.ToArray();
            }

            return result;
        }

        private static PdfPTable BuildTable(List<Sale> sales, List<DateTime> dates)
        {

            PdfPTable table = new PdfPTable(NUMBER_OF_ROWS);
            PdfPCell header = new PdfPCell(new Phrase("Aggregated Sales Report"));
            header.Colspan = NUMBER_OF_ROWS;
            header.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(header);
            table.CompleteRow();

            int j = 0;

            foreach (var date in dates)
            {
                AddDateAndHeaders(ref table, date);
                decimal sum = 0;

                for (; j < sales.Count; ++j)
                {
                    if (sales[j].Date == date)
                    {
                        table.AddCell(new Phrase(sales[j].ProductName));
                        table.AddCell(new Phrase(sales[j].Quantity.ToString() + " " + sales[j].Mesure));
                        table.AddCell(new Phrase(string.Format("{0:f2}", sales[j].UnitPrice)));
                        table.AddCell(new Phrase(sales[j].Location));
                        table.AddCell(new Phrase(string.Format("{0:f2}", sales[j].Sum)));
                        sum += sales[j].Sum;
                    }
                    else
                    {
                        break;
                    }
                }

                AddSummary(ref table, date, sum);
                table.CompleteRow();
            }

            return table;
        }

        private static void AddSummary(ref PdfPTable table, DateTime date, decimal sum)
        {
            PdfPCell summaryDate = new PdfPCell(new Phrase("Total sum for " + GetDateString(date) + ":"));
            summaryDate.Colspan = NUMBER_OF_ROWS - 1;
            summaryDate.HorizontalAlignment = 2;

            table.AddCell(summaryDate);

            PdfPCell totalSold = new PdfPCell(new Phrase(sum.ToString()));
            table.AddCell(totalSold);

            table.CompleteRow();
        }

        private static void AddDateAndHeaders(ref PdfPTable table, DateTime date)
        {
            PdfPCell dateHeader = new PdfPCell(new Phrase("Date: " + GetDateString(date)));
            dateHeader.Colspan = NUMBER_OF_ROWS;
            dateHeader.HorizontalAlignment = 0;

            table.AddCell(dateHeader);
            table.CompleteRow();

            for (int i = 0; i < reportCellHeaders.Length; ++i)
            {
                table.AddCell(new PdfPCell(reportCellHeaders[i]));
            }

            table.CompleteRow();
        }

        private static string GetDateString(DateTime date)
        {
            string dateString = string.Format("{0}-{1}-{2}", date.Day, GetMonthAbbreviation(date.Month), date.Year);
            return dateString;
        }

        private static string GetMonthAbbreviation(int monthNumber)
        {
            string[] monthAbbreviations = { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            return monthAbbreviations[monthNumber - 1];
        }
    }
}