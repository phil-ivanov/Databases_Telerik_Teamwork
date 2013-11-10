using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace TrasnferDataToSqlServer
{
    public class ExcelToTableData
    {
        public static DataTable GetTable(string filename, string sheetName)
        {
            string[] connectionStrings = new[] { 
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;",
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data source={0};Extended Properties=Excel 8.0;"
            };
            foreach (string connectionStringBase in connectionStrings)
            {
                try
                {
                    string connectionString = String.Format(connectionStringBase, filename);
                    string sqlQuery = String.Format("select * from [{0}$]", sheetName);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, connectionString);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset, "Sales");
                    
                    DataTable table = dataset.Tables[0];

                    return table;
                }
                catch (Exception)
                {
                    continue; 
                }
            }

            throw new ArgumentOutOfRangeException("filename", "File does not contain import data in a known format.");
        }
    }
}
