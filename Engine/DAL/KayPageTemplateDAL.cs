using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayPageTemplateDAL
    {
        // SQL
        private const String SelectSql = "SELECT * FROM PageTemplates {0}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Id = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "ORDER BY SortOrder");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add
        public static int Add(String Title, String UrlPath, String Placeholders, String Filepath, int Greedy, int Status, int Device)
        {
            // Sort order
            int SortOrder = int.Parse(DbUtilities.GetValue("SELECT IFNULL(MAX(SortOrder), 0) FROM PageTemplates", "0")) + 1;

            // Build Sql
            String Sql = String.Format(
                "INSERT INTO PageTemplates (Title, UrlPath, Placeholders, Filepath, Greedy, Status, SortOrder, Device) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Placeholders),
                DbUtilities.SqlSafe(Filepath),
                DbUtilities.SqlSafe(Greedy),
                DbUtilities.SqlSafe(Status),
                DbUtilities.SqlSafe(SortOrder),
                DbUtilities.SqlSafe(Device));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM PageTemplates", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, String Title, String UrlPath, String Placeholders, String Filepath, int Greedy, int Status, int Device)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE PageTemplates SET Title = {1}, UrlPath = {2}, Placeholders = {3}, Filepath = {4}, Greedy = {5}, Status = {6}, Device = {7} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Placeholders),
                DbUtilities.SqlSafe(Filepath),
                DbUtilities.SqlSafe(Greedy),
                DbUtilities.SqlSafe(Status),
                DbUtilities.SqlSafe(Device));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Sort order
        public static Boolean UpdateOrder(int Id, int ReferenceId, String Direction)
        {
            String Sql;

            // Get new sort order
            Sql = String.Format(
                "SELECT SortOrder FROM PageTemplates WHERE Id = {0}",
                DbUtilities.SqlSafe(ReferenceId));
            Double NewSortOrder = Double.Parse(DbUtilities.GetValue(Sql, "0"));

            // Change existing sort orders
            Sql = String.Format(
                "UPDATE PageTemplates SET SortOrder = SortOrder + 1 WHERE SortOrder {0} {1}",
                (Direction.ToLower() == "before" ? ">=" : ">"),
                DbUtilities.SqlSafe(NewSortOrder));
            DbUtilities.ExecuteSql(Sql);

            // Update selected record
            Sql = String.Format(
                "UPDATE PageTemplates SET SortOrder = {0} WHERE Id = {1}",
                (Direction.ToLower() == "before" ? NewSortOrder.ToString() : (NewSortOrder + 1).ToString()),
                DbUtilities.SqlSafe(Id));
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM PageTemplates WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }
    }
}
