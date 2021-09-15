using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqlCrud
    {
        private readonly string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();

        public SqlCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BasicContactModel> GetAllContacts()
        {
            string sql = "select Id, FirstName, LastName from dbo.Contacts";

            return db.LoadData<BasicContactModel, dynamic>(sql, new { }, _connectionString);
        }

        public FullContactModel GetFullContactById(int id)
        {
            string sql = "select Id,FirstName,LastName from dbo.Contacts where Id = @Id";

            FullContactModel output = new FullContactModel();
            
            output.BasicInfo = db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

            if (output.BasicInfo == null)
            {
                // do something to tell the user that the record was not found.
                // throw new Exception("User not found");
                return null;
            }

            sql = @"select E.* 
                    from dbo.EmailAddresses E
                    inner join dbo.ContactEmailMapping CE on CE.EmailAddressId = E.Id
                    where CE.ContactId = @Id";

            output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);

            sql = @"select P.* 
                    from dbo.PhoneNumbers P
                    inner join ContactPhoneNumbersMapping CP on CP.PhoneNumberId = P.Id
                    where CP.ContactId = @Id";

            output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);

            return output;
        }

        public void CreateContact(FullContactModel contact)
        {
            // save basic contact
            #region Insert BasicContact Details
            string sql = "insert into dbo.Contacts(FirstName, LastName) values (@FirstName, @LastName)";
            db.SaveData(
                sql,
                new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
                _connectionString);

            // get the ID number of the created contact
            sql = "select Id from dbo.Contacts where FirstName = @FirstName and LastName = @LastName";
            int contactId = db.LoadData<IdLookupModel, dynamic>(
                sql,
                new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
                _connectionString).First().Id;
            #endregion

            // check if email address already exists, and insert to emailAddress table and mapping table
            #region Insert EmailAddress and its mapping
            foreach (var emailAddress in contact.EmailAddresses)
            {
                if (emailAddress.Id == 0)
                {
                    // insert new emailId to the table
                    sql = "insert into dbo.EmailAddresses (EmailAddress) values (@EmailAddress)";
                    db.SaveData(sql, new { emailAddress.EmailAddress }, _connectionString);

                    // select id of inserted EmailId
                    sql = "select Id from dbo.EmailAddresses where EmailAddress = @EmailAddress";
                    emailAddress.Id = db.LoadData<IdLookupModel, dynamic>(
                        sql,
                        new { emailAddress.EmailAddress },
                        _connectionString).First().Id;
                }
                // insert to mapping table
                sql = "insert into dbo.ContactEmailMapping (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId)";
                db.SaveData(sql, new { ContactId = contactId, EmailAddressId = emailAddress.Id }, _connectionString);
            }
            #endregion

            #region Insert PhoneNumbers and its mapping
            // check if phone number already exists, and insert to phoneNumber table and mapping table
            foreach (var phoneNumber in contact.PhoneNumbers)
            {
                if (phoneNumber.Id == 0)
                {
                    sql = "insert into dbo.PhoneNumbers (PhoneNumber) values (@PhoneNumber)";
                    db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

                    sql = "select Id from dbo.PhoneNumbers where PhoneNumber = @PhoneNumber";
                    phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                                                                         new { phoneNumber.PhoneNumber },
                                                                         _connectionString).First().Id;
                }

                sql = "insert into dbo.ContactPhoneNumbersMapping (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId)";
                db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, _connectionString);
            }
            #endregion
        }
    }
}
