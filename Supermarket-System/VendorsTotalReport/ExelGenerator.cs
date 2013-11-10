using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace VendorsTotalReport
{
    public static class ExelGenerator
    {
        private static readonly Dictionary<string, decimal> incomeDictonary = new Dictionary<string, decimal>();
        private static readonly Dictionary<string, decimal> expensesDictonary = new Dictionary<string, decimal>();
        private static readonly Dictionary<string, float> taxesDictonary = new Dictionary<string, float>();
        private static readonly string connectionString = Settings.Default.ExcelConnectionString;

        public static void GenerateExelVendorReport(List<VendorExpense> expenses, List<Report> reports, Dictionary<string, float> taxes)
        {
            DateTime current = DateTime.Now;
            List<string> vendorNames = new List<string>();

            foreach (var expense in expenses)
            {
                if (current.Month == expense.Month.Month + 1 && current.Year == expense.Month.Year)
                {
                    vendorNames.Add(expense.VendorName);
                    if (!expensesDictonary.ContainsKey(expense.VendorName))
                    {
                        expensesDictonary.Add(expense.VendorName, expense.Expenses);
                    }
                    else
                    {
                        expensesDictonary[expense.VendorName] = expensesDictonary[expense.VendorName] + expense.Expenses;
                    }
                }
            }

            foreach (var report in reports)
            {
                if (incomeDictonary.ContainsKey(report.VendorName))
                {
                    incomeDictonary[report.VendorName] = incomeDictonary[report.VendorName] + report.TotalIncomes;
                }
                else
                {
                    if (report.TotalIncomes > 0)
                    {
                        incomeDictonary.Add(report.VendorName, report.TotalIncomes);
                    }
                }

                if (taxesDictonary.ContainsKey(report.VendorName))
                {
                    if (taxes.ContainsKey(report.ProductName))
                    {
                        taxesDictonary[report.VendorName] = taxesDictonary[report.VendorName] + (float)report.TotalIncomes * taxes[report.ProductName];
                    }
                }
                else
                {
                    if (taxes.ContainsKey(report.ProductName))
                    {
                        taxesDictonary.Add(report.VendorName, (float)report.TotalIncomes * taxes[report.ProductName]);
                    }
                }
            }

            GenerateExelTable(vendorNames);
        }

        private static void GenerateExelTable(List<string> vendorNames)
        {
            CreateEmptyExcelFile();

            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();

            using (connection)
            {
                string createTable = @"CREATE TABLE [Sheet1$] (
                        Vendor char(255), 
                        Incomes number,
                        Expenses number,
                        Taxes number,
                        FinancialResults number
                            )";

                OleDbCommand createCommand = new OleDbCommand(createTable, connection);
                createCommand.ExecuteNonQuery();

                string sqlInsert = @"INSERT INTO [Sheet1$]
                                    VALUES (@vendor, @incomes, @expenses, @taxes, @result)";

                foreach (var name in vendorNames)
                {
                    if (!incomeDictonary.ContainsKey(name) || !expensesDictonary.ContainsKey(name) || !taxesDictonary.ContainsKey(name))
                    {
                        continue;
                    }

                    OleDbCommand insertCommand = new OleDbCommand(sqlInsert, connection);
                    insertCommand.Parameters.AddWithValue("@vendor", name);
                    insertCommand.Parameters.AddWithValue("@incomes", string.Format("{0:f2}", incomeDictonary[name]));
                    insertCommand.Parameters.AddWithValue("@expenses", string.Format("{0:f2}", expensesDictonary[name]));
                    insertCommand.Parameters.AddWithValue("@taxes", string.Format("{0:f2}", taxesDictonary[name]));
                    insertCommand.Parameters.AddWithValue("@result", string.Format("{0:f2}", incomeDictonary[name] - expensesDictonary[name] - (decimal)taxesDictonary[name]));

                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        private static void CreateEmptyExcelFile()
        {
            using (System.IO.FileStream fs = System.IO.File.Create(Settings.Default.ReportFileName))
            {
                byte[] allBytesBin = System.IO.File.ReadAllBytes(Settings.Default.ExcelBinTemplate);
                fs.Write(allBytesBin, 0, allBytesBin.GetLength(0));
            }
        }
    }
}