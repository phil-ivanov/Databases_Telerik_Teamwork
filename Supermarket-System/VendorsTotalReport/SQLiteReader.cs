using System;
using System.Linq;
using System.Collections.Generic;
using SQLiteSupermarket.Data;

namespace VendorsTotalReport
{
    public class SQLiteReader
    {
        public Dictionary<string, float> GetProductTaxes()
        {
            SQLiteEntities context = new SQLiteEntities();
            var result = new Dictionary<string, float>();

            using (context)
            {
                var allTaxes = context.Taxes;

                foreach (var tax in allTaxes)
                {
                    result.Add(tax.ProductName, tax.Tax1 / 100);
                }

                return result;
            }
        }
    }
}
