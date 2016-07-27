using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayContactListDAL
    {
        // SQL
        private const String SelectSql = "SELECT ContactLists.*, (SELECT COUNT(UserId) FROM ContactListUsers WHERE ContactListUsers.ListId = ContactLists.Id) AS UserCount FROM ContactLists WHERE {0} ORDER BY {1}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("Id = {0}", DbUtilities.SqlSafe(Id)),
                "Id");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "Id IS NOT NULL",
                "SortOrder");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add
        public static int Add(String Title)
        {
            // Sort order
            int SortOrder = int.Parse(DbUtilities.GetValue("SELECT IFNULL(MAX(SortOrder), 0) FROM ContactLists", "0")) + 1;

            // Build Sql
            String Sql = String.Format(
                "INSERT INTO ContactLists (Title, SortOrder) VALUES ({0}, {1})",
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(SortOrder));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM ContactLists", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, String Title)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE ContactLists SET Title = {1} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(Title));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Sort order
        public static Boolean UpdateOrder(int Id, int ReferenceId, String Direction)
        {
            String Sql;

            // Get new sort order
            Sql = String.Format(
                "SELECT SortOrder FROM ContactLists WHERE Id = {0}",
                DbUtilities.SqlSafe(ReferenceId));
            int NewSortOrder = int.Parse(DbUtilities.GetValue(Sql, "0"));

            // Change existing sort orders
            Sql = String.Format(
                "UPDATE ContactLists SET SortOrder = SortOrder + 1 WHERE SortOrder {0} {1}",
                (Direction.ToLower() == "before" ? ">=" : ">"),
                DbUtilities.SqlSafe(NewSortOrder));
            DbUtilities.ExecuteSql(Sql);

            // Update selected record
            Sql = String.Format(
                "UPDATE ContactLists SET SortOrder = {0} WHERE Id = {1}",
                (Direction.ToLower() == "before" ? NewSortOrder.ToString() : (NewSortOrder + 1).ToString()),
                DbUtilities.SqlSafe(Id));
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM ContactLists WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Unique title
        public static Boolean UniqueTitle(int Id, String Title)
        {
            // Build SQL
            String Sql = String.Format(
                "SELECT COUNT(Id) FROM ContactLists WHERE Title = {0} AND Id <> {1}",
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(Id));

            // Get the value 
            return int.Parse(DbUtilities.GetValue(Sql)) == 0;
        }

        #region Users

        // List
        public static DataTable Users(int ListId)
        {
            // Build SQL
            String Sql = String.Format(
                @"SELECT Users.Id AS Users_Id,
                    Users.FirstName AS Users_FirstName,
                    Users.LastName AS Users_LastName,
                    Users.EmailAddress AS Users_EmailAddress,
                    Users.Telephone AS Users_Telephone,
                    Users.Password AS Users_Password,
                    Users.Company AS Users_Company,
                    Users.Address AS Users_Address,
                    Users.Groups AS Users_Groups FROM Users INNER JOIN ContactListUsers ON Users.Id = ContactListUsers.UserId WHERE ListId = {0} ORDER BY FirstName",
                DbUtilities.SqlSafe(ListId));

            // Execute
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add user
        public static Boolean AddUser(int ListId, int UserId)
        {
            // Build SQL
            String Sql = String.Format(
                "INSERT INTO ContactListUsers (ListId, UserId) VALUES ({0}, {1})",
                DbUtilities.SqlSafe(ListId),
                DbUtilities.SqlSafe(UserId));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Remove user
        public static Boolean RemoveUser(int ListId, int UserId)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM ContactListUsers WHERE ListId = {0} AND UserId = {1}",
                DbUtilities.SqlSafe(ListId),
                DbUtilities.SqlSafe(UserId));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Remove user (from all lists)
        public static Boolean RemoveUser(int UserId)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM ContactListUsers WHERE UserId = {0}",
                DbUtilities.SqlSafe(UserId));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        #endregion
    }
}
