using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MySqlUI
{
    class Program
    {
        static void Main(string[] args)
        {

            MySqlCrud sql = new MySqlCrud(GetConnectionString());
            //ReadAllContacts(sql);
            //ReadContact(sql, 2);
            //CreateNewContact(sql);            
            //UpdateContact(sql);
            //RemoveContact(sql, 1, 1);

            Console.WriteLine("Done processing MySQL!");
            Console.ReadLine();
        }

        private static void RemoveContact(MySqlCrud sql, int contactId, int phoneNumberId)
        {
            sql.RemovePhoneNumberFromContact(contactId, phoneNumberId);
        }

        private static void UpdateContact(MySqlCrud sql)
        {
            BasicContactModel user = new BasicContactModel
            {
                Id = 1,
                FirstName = "Adithya",
                LastName = "Vijay K"
            };
            sql.UpdateContactName(user);
        }

        private static void CreateNewContact(MySqlCrud sql)
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
            user.EmailAddresses.Add(new EmailAddressModel { Id = 2, EmailAddress = "info@adithyavj.in" });

            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "1234656789" });
            user.PhoneNumbers.Add(new PhoneNumberModel { Id = 2, PhoneNumber = "9946173471" });

            sql.CreateContact(user);

        }

        private static void ReadAllContacts(MySqlCrud sql)
        {
            var rows = sql.GetAllContacts();
            foreach (var row in rows)
            {
                Console.WriteLine($"{row.Id} {row.FirstName} {row.LastName}");
            }
        }

        private static void ReadContact(MySqlCrud sql, int contactId)
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
