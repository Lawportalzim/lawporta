using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayUserDAL
    {
        // SQL
        private const String SelectSql =
            @"  SELECT
                    Users.Id AS Users_Id,
                    Users.FirstName AS Users_FirstName,
                    Users.LastName AS Users_LastName,
                    Users.EmailAddress AS Users_EmailAddress,
                    Users.Telephone AS Users_Telephone,
                    Users.Password AS Users_Password,
                    Users.ExpiryDate AS Users_ExpiryDate,
                    Users.Online AS Users_Online,
                    Users.Status AS Users_Status,
                    Users.StartDate AS Users_StartDate,
                    Users.LastSeen AS Users_LastSeen,
                   
                    Companies.Id AS Companies_Id,
                    Companies.Name AS Companies_Name,                    
                    Companies.EmailAddress AS Companies_EmailAddress,
                    Companies.Telephone AS Companies_Telephone,                    
                    Companies.ContactPerson AS Companies_ContactPerson,
                    Companies.Address AS Companies_Address,
                    Companies.NumberOfAccounts AS Companies_NumberOfAccounts,                      

                    Users.Address AS Users_Address,
                    Users.Groups AS Users_Groups
                FROM 
                    Users LEFT OUTER JOIN
                    Companies ON Users.CompanyId = Companies.Id
                {0}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql, 
                String.Format("WHERE Users.Id = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetDetails(string EmailAddress)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Users.EmailAddress = {0}", DbUtilities.SqlSafe(EmailAddress)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "ORDER BY Users.FirstName");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetList(int Company)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                string.Format("WHERE Users.CompanyId = {0} ORDER BY Users.FirstName", Company));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add
        public static int Add(string FirstName, string LastName, string EmailAddress, string Telephone, int CompanyId, string Address, string Password, int Groups, DateTime ExpiryDate, Boolean Online, Boolean Active, DateTime StartDate, DateTime LastSeen)
        {
            // Build Sql
            String Sql = String.Format(
                "INSERT INTO Users (FirstName, LastName, EmailAddress, Telephone, CompanyId, Address, Password, Groups, ExpiryDate, Online, Status, StartDate, LastSeen)" +
                "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12})",
                DbUtilities.SqlSafe(FirstName),
                DbUtilities.SqlSafe(LastName),
                DbUtilities.SqlSafe(EmailAddress),
                DbUtilities.SqlSafe(Telephone),
                DbUtilities.SqlSafe(CompanyId),
                DbUtilities.SqlSafe(Address),
                DbUtilities.SqlSafe(Password),
                DbUtilities.SqlSafe(Groups),
                DbUtilities.SqlSafe(ExpiryDate),
                DbUtilities.SqlSafe(Online),
                DbUtilities.SqlSafe(Active),
                DbUtilities.SqlSafe(StartDate),
                DbUtilities.SqlSafe(LastSeen));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM Users", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, string FirstName, string LastName, string EmailAddress, string Telephone, int CompanyId, string Address, string Password, int Groups, DateTime ExpiryDate, Boolean Online, Boolean Active, DateTime StartDate, DateTime LastSeen)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE Users SET " +
                    "FirstName = {1}, " +
                    "LastName = {2}, " +
                    "EmailAddress = {3}, " +
                    "Telephone = {4}, " +
                    "CompanyId = {5}, " +
                    "Address = {6}, " +
                    "Password = {7}, " +
                    "Groups = {8}, " +
                    "ExpiryDate = {9}, " +
                    "Online = {10}, " +
                    "Status  = {11}," +
                    "StartDate = {12}," +
                    "LastSeen = {13}" +
                "WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(FirstName),
                DbUtilities.SqlSafe(LastName),
                DbUtilities.SqlSafe(EmailAddress),
                DbUtilities.SqlSafe(Telephone),
                DbUtilities.SqlSafe(CompanyId),
                DbUtilities.SqlSafe(Address),
                DbUtilities.SqlSafe(Password),
                DbUtilities.SqlSafe(Groups),
                DbUtilities.SqlSafe(ExpiryDate),
                DbUtilities.SqlSafe(Online),
                DbUtilities.SqlSafe(Active),
                DbUtilities.SqlSafe(StartDate),
                DbUtilities.SqlSafe(LastSeen));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM Users WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Unique
        public static Boolean UniqueEmailAddress(int Id, string EmailAddress)
        {
            // Build SQL
            String Sql = String.Format(
                "SELECT COUNT(Id) FROM Users WHERE EmailAddress = {0} AND Id <> {1}",
                DbUtilities.SqlSafe(EmailAddress),
                DbUtilities.SqlSafe(Id));

            // Get the value 
            return int.Parse(DbUtilities.GetValue(Sql)) == 0;
        }
    }
}
