using System;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BatchOps
{
    class Program
    {
        static void Main(string[] args)
        {

            //init table params and extablish connection to storage container
            string storageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            Console.WriteLine("Enter the name of the table");
            var tableName = Console.ReadLine();
            CloudTable _table = tableClient.GetTableReference(tableName);
            //connection established


            //create table
            _table.CreateIfNotExistsAsync().GetAwaiter().GetResult();//when ever there is asyn call you wait pls
            //create a bulk insert object and pass the table arg

            //create list of customers here
            List<Customer> customerList = new List<Customer> { };
            for (int i = 1; i <= 990; i++)
            {
                customerList.Add(new Customer(NewCustomerCreator.getCustName(), NewCustomerCreator.getEmail(), "registered"));
            }




            try
            {
                BulkInsert.BatchInsert(_table, customerList).GetAwaiter().GetResult();
            }
            catch (StorageException ex)
            {
                Console.WriteLine("Exception" + ex.Message);
            }


            Console.ReadKey();


            //DeleteTable.DropTable(_table).GetAwaiter().GetResult();
            //have to work on retrieving all items and deleting by bulk


        }
    }

    static class BulkInsert
    {
        public static async Task BatchInsert(CloudTable table, List<Customer> customers)
        {
            TableBatchOperation batch = new TableBatchOperation();
            /*
            while (batch.Count != 100)
            {
                foreach (var cust in customers)
                {
               
                    if(batch.Count!=100) 
                    batch.Insert(cust);
                }
                await table.ExecuteBatchAsync(batch);
            }
            */
            Console.WriteLine(customers.Count);
            foreach (var cust in customers)
            {
                Console.WriteLine(batch.Count);
                batch.Insert(cust);

                if (batch.Count == 100)
                {
                    Console.WriteLine("Before sending" + batch.Count);
                    await table.ExecuteBatchAsync(batch);
                    //have to delete all the elements in this batch then later add
                    batch.Clear();
                    Console.WriteLine("After sending" + batch.Count);
                }
            }

            Console.WriteLine("final"+batch.Count);
            if (batch.Count > 0)
                await table.ExecuteBatchAsync(batch);

        }



    }

    static class DeleteTable
    {
        public static async Task DropTable(CloudTable table)
        {
            await table.DeleteIfExistsAsync();
        }
    }
}
