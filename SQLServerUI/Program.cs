﻿using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using DataAccessLibrary;
using DataAccessLibrary.Models;

namespace SQLServerUI
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlCrud sql = new SqlCrud(GetConnectionString());
            //ReadAllContacts(sql);
            //ReadContact(sql, 1);
            CreateNewContact(sql);            

            Console.ReadLine();
        }

        private static void CreateNewContact(SqlCrud sql)
        {
            FullContactModel user = new FullContactModel
            {
                BasicInfo = new BasicContactModel
                {
                    FirstName = "Athulya",
                    LastName = "Vijay"
                },
            };

            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "athulyavj@gmail.com" });
            user.EmailAddresses.Add(new EmailAddressModel { Id = 3, EmailAddress = "info@adithyavj.in" });

            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "1234656789" });
            user.PhoneNumbers.Add(new PhoneNumberModel { Id = 2, PhoneNumber = "9946173471" });

            sql.CreateContact(user);

        }

        private static void ReadAllContacts(SqlCrud sql)
        {
            var rows = sql.GetAllContacts();
            foreach (var row in rows)
            {
                Console.WriteLine($"{row.Id} {row.FirstName} {row.LastName}");
            }
        }

        private static void ReadContact(SqlCrud sql,int contactId)
        {
            var contact = sql.GetFullContactById(contactId);

            Console.WriteLine($"{contact.BasicInfo.Id} {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName}");
            
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
