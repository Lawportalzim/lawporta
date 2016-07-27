using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayCompanyDAL
    {
        // SQL
        private const String SelectSql =
            @"  SELECT
                    Companies.Id AS Companies_Id,
                    Companies.Name AS Companies_Name,                    
                    Companies.EmailAddress AS Companies_EmailAddress,
                    Companies.Telephone AS Companies_Telephone,                    
                    Companies.ContactPerson AS Companies_ContactPerson,
                    Companies.Address AS Companies_Address,
                    Companies.NumberOfAccounts AS Companies_NumberOfAccounts                    
                FROM 
                    Companies 
                {0}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql, 
                String.Format("WHERE Companies.Id = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "ORDER BY Companies.Name");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add
        public static int Add(string Name, string EmailAddress, string Telephone, string ContactPerson, string Address, int NumberOfAccounts)
        {
            // Build Sql
            String Sql = String.Format(
                "INSERT INTO Companies (Name, EmailAddress, Telephone, ContactPerson, Address, NumberOfAccounts)" +
                "VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                DbUtilities.SqlSafe(Name),               
                DbUtilities.SqlSafe(EmailAddress),
                DbUtilities.SqlSafe(Telephone),
                DbUtilities.SqlSafe(ContactPerson),
                DbUtilities.SqlSafe(Address),
                DbUtilities.SqlSafe(NumberOfAccounts));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM Companies", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, string Name, string EmailAddress, string Telephone, string ContactPerson, string Address, int NumberOfAccounts)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE Companies SET " +
                    "Name = {1}, " +                    
                    "EmailAddress = {2}, " +
                    "Telephone = {3}, " +
                    "ContactPerson = {4}, " +
                    "Address = {5}, " + 
                    "NumberOfAccounts = {6} " +
                "WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(Name),
                DbUtilities.SqlSafe(EmailAddress),
                DbUtilities.SqlSafe(Telephone),
                DbUtilities.SqlSafe(ContactPerson),
                DbUtilities.SqlSafe(Address),
                DbUtilities.SqlSafe(NumberOfAccounts));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM Companies WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Unique
        public static Boolean UniqueEmailAddress(int Id, string EmailAddress)
        {
            // Build SQL
            String Sql = String.Format(
                "SELECT COUNT(Id) FROM Companies WHERE EmailAddress = {0} AND Id <> {1}",
                DbUtilities.SqlSafe(EmailAddress),
                DbUtilities.SqlSafe(Id));

            // Get the value 
            return int.Parse(DbUtilities.GetValue(Sql)) == 0;
        }
    }
}
