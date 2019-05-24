using System;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;

namespace BatchOps
{
    
    public class Customer:TableEntity
    {
        public string CustID;
        public string CustEmail;
        public string CustStat;

       public Customer(string id,string email,string stat)
        {
            this.PartitionKey = stat;
            this.RowKey = email;
            this.CustID = id;
            this.CustEmail = email;
            this.CustStat = stat;
        }
    }
}
