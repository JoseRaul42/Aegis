using Microsoft.ML.Data;
using Milvus.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MilvusDatabase
{
    class MilvusConnection
    {
        private string Host = "localhost";
        private int Port = 19530; // This is Milvus's default port
        private bool UseSsl = false; // Default value is false
        private string Database = "AgentKnowledge"; // Defaults to the default Milvus database

        public async Task TestMilvusConnectionAsync()
        {


            try
            {
                // Initialize the Milvus client
                MilvusClient milvusClient = new MilvusClient(Host, Port, UseSsl);

                // Perform a health check
                MilvusHealthState healthState = await milvusClient.HealthAsync();
                Console.WriteLine($"Health State: {healthState}");

                //// Create a CancellationToken
                //CancellationToken cancellationToken = new CancellationToken();

                // List databases
                var databases = await milvusClient.ListDatabasesAsync();

                // Check if the list of databases is null or does not contain "AgentKnowledge"
                if (databases == null || !databases.Contains("AgentKnowledge"))
                {
                    // Create a new database if "AgentKnowledge" is not found
                    await milvusClient.CreateDatabaseAsync("AgentKnowledge");
                }
                else
                {
                    Console.WriteLine("Database AgentKnowledge for vector stores exists..........");
                }

                //// Write databases to a file for testing purposees
                //string filePath = ".\\ListDatabases.txt"; // Replace with your desired file path
                //File.WriteAllText(filePath, "Databases: " + string.Join(", ", databases));  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }



            // Optional collection management code (commented out)
            // string collectionName = "book";
            // MilvusCollection collection = milvusClient.GetCollection(collectionName);

            // // Check if this collection exists
            // var hasCollection = await milvusClient.HasCollectionAsync(collectionName);

            // if (hasCollection)
            // {
            //     await collection.DropAsync();
            //     Console.WriteLine("Dropped collection {0}", collectionName);
            // }

            // await milvusClient.CreateCollectionAsync(
            //     collectionName,
            //     new[]
            //     {
            //         FieldSchema.Create<long>("book_id", isPrimaryKey: true),
            //         FieldSchema.Create<long>("word_count"),
            //         FieldSchema.CreateVarchar("book_name", 256),
            //         FieldSchema.CreateFloatVector("book_intro", 2)
            //     }
            // );








        }
    }



}
