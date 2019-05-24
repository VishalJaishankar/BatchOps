using System;
using System.Collections.Generic;
using System.Text;

namespace BatchOps
{
    static class NewCustomerCreator
    {
        static string custname = "Customer";
        static int num = 0;
       public static string  getCustName()
        {
            num++;
            return custname + num.ToString();
        }
        public static string getEmail()
        {
            return custname + num.ToString() + "@gmail.com";
        }
    }
}
