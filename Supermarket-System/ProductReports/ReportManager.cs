using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using MongoDB.Driver;

namespace ProductReports
{
    public class ReportManager
    {
        private static readonly string databaseName = Settings.Default.MongoDatabaseName;
        private static readonly string collectionName = Settings.Default.MongoCollectionName;
        private static readonly string reportsFolderName = Settings.Default.MongoReportsFolderName;

        public void SaveToFileSystemAsJson(Report report, string path)
        {
            if (report == null)
            {
                throw new ArgumentNullException("Invalid report! It cannot be null!");
            }

            if (path == null)
            {
                throw new ArgumentNullException("Invalid path! It cannot be null!");
            }
            else if (string.IsNullOrWhiteSpace(path) || path == string.Empty)
            {
                throw new ArgumentException("Invalid path! It cannot be empty or contain only white spaces!");
            }

            string fullPath = path + "/" + reportsFolderName;

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            string fileName = report.ProductId + ".json";

            fullPath = fullPath + "/" + fileName;

            FileStream stream = new FileStream(fullPath, FileMode.Create);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Report));

            using (stream)
            {
                serializer.WriteObject(stream, report);
                stream.Flush(); 
            }
        }

        public void SaveToMongoDB(Report report, string connectionString)
        {
            if (report == null)
            {
                throw new ArgumentNullException("Invalid report! It cannot be null!");
            }

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

            collection.Insert(report);
        }
    }
}
