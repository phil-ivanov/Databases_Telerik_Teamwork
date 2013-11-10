using System;
using System.IO;
using System.Linq;
using System.Xml;
using MSSQLSupermarket.Data;
using System.Collections.Generic;
using MongoDB.Driver;

namespace LoadVendorExpenses
{
    public class XmlDataTransferer
    {
        private static readonly string databaseName = Settings.Default.MongoDatabaseName;
        private static readonly string collectionName = Settings.Default.MongoCollectionName;

        private string xmlLocation;
        private List<VendorExpens> expensesToSql;
        private List<MongoDbVendorExpense> expensesToMongo;

        public XmlDataTransferer(string xmlLocation)
        {
            this.XmlLocation = xmlLocation;

            this.ReadExpenses();
        }

        public string XmlLocation
        {
            get
            {
                return this.xmlLocation;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid path! It cannot be null!");
                }
                else if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
                {
                    throw new ArgumentException("Invalid path! It cannot be empty or contain only white spaces!");
                }

                if (!File.Exists(value))
                {
                    throw new ArgumentException("Invalid path! The file does not exist!");
                }

                this.xmlLocation = value;
                this.ReadExpenses();
            }
        }

        public void TransferExpensesToMssql()
        {
            using (var context = new SupermarketEntities())
            {
                foreach (var expense in this.expensesToSql)
                {
                    context.VendorExpenses.Add(expense);
                }

                context.SaveChanges();
            }
        }

        public void TransferExpensesToMongoDb(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("Invalid connection string! It cannot be null!");
            }
            else if (string.IsNullOrWhiteSpace(connectionString) || connectionString == string.Empty)
            {
                throw new ArgumentException("Invalid connection string! It cannot be empty or contain only white spaces!");
            }

            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(databaseName);
            var collection = database.GetCollection(collectionName);

            this.ReadExpenses();

            foreach (var expense in this.expensesToMongo)
            {
                collection.Insert(expense);
            }
        }

        private void ReadExpenses()
        {
            this.expensesToSql = new List<VendorExpens>();
            this.expensesToMongo = new List<MongoDbVendorExpense>();

            using (var context = new SupermarketEntities())
            {
                using (XmlReader reader = XmlReader.Create(this.xmlLocation))
                {
                    string vendorName = string.Empty;

                    while (reader.Read())
                    {
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "sale"))
                        {
                            vendorName = reader.GetAttribute(0);
                        }
                        else if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "expenses"))
                        {
                            var vendorId = context.Vendors.Where(x => x.VendorName == vendorName).First().ID;
                            var date = DateTime.Parse(reader.GetAttribute(0));
                            decimal expense = decimal.Parse(reader.ReadElementString());

                            VendorExpens sqlExpense = new VendorExpens();
                            sqlExpense.VendorID = vendorId;
                            sqlExpense.Month = date;
                            sqlExpense.Expenses = expense;
                            this.expensesToSql.Add(sqlExpense);

                            MongoDbVendorExpense mongoExpense = new MongoDbVendorExpense();
                            mongoExpense.VendorID = vendorId;
                            mongoExpense.VendorName = vendorName;
                            mongoExpense.Month = date;
                            mongoExpense.Expenses = expense;
                            this.expensesToMongo.Add(mongoExpense);
                        }
                    }
                }
            }
        }
    }
}
