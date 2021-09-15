using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SqliteUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //SqlCrud sql = new SqlCrud(GetConnectionString());
            //ReadAllContacts(sql);
            //ReadContact(sql, 1);
            //CreateNewContact(sql);            
            //UpdateContact(sql);
            //RemoveContact(sql, 1, 1);

            Console.WriteLine("Done processing Sqlite!");
            Console.ReadLine();
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
