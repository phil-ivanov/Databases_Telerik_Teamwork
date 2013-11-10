using System;
using System.Linq;
using MSSQLSupermarket.Data;
using MySQLSupermarket.Data;

namespace TrasnferDataToSqlServer
{
    public class MySqlDataTransferer
    {
        public void Transfer()
        {
            TransferMeasures();

            TransferVendors();

            TransferProducts();
        }

        private void TransferMeasures()
        {
            using (var mySqlContext = new MySqlSupermarketEntities())
            {
                using (var msSqlContext = new SupermarketEntities())
                {
                    var mySqlMeasures = mySqlContext.Measures;

                    foreach (var measure in mySqlMeasures)
                    {
                        var currentMeasure = new MSSQLSupermarket.Data.Measure();
                        currentMeasure.MeasureName = measure.MeasureName;

                        msSqlContext.Measures.Add(currentMeasure);
                    }

                    msSqlContext.SaveChanges();
                }
            }
        }

        private void TransferVendors()
        {
            using (var mySqlContext = new MySqlSupermarketEntities())
            {
                using (var msSqlContext = new SupermarketEntities())
                {
                    var mySqlVendors = mySqlContext.Vendors;

                    foreach (var vendor in mySqlVendors)
                    {
                        var currentVendor = new MSSQLSupermarket.Data.Vendor();
                        currentVendor.VendorName = vendor.VendorName;

                        msSqlContext.Vendors.Add(currentVendor);
                    }

                    msSqlContext.SaveChanges();
                }
            }
        }

        private void TransferProducts()
        {
            using (var mySqlContext = new MySqlSupermarketEntities())
            {
                using (var msSqlContext = new SupermarketEntities())
                {
                    var mySqlProducts = mySqlContext.Products;

                    foreach (var product in mySqlProducts)
                    {
                        var currentProduct = new MSSQLSupermarket.Data.Product();
                        currentProduct.VendorID = product.VendorID;
                        currentProduct.ProductName = product.ProductName;
                        currentProduct.MeasureID = product.MeasureID;
                        currentProduct.BasicPrice = (decimal)product.BasicPrice;

                        msSqlContext.Products.Add(currentProduct);
                    }

                    msSqlContext.SaveChanges();
                }
            }
        }
    }
}
