using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayHelpDAL
    {
        // SQL
        private const String SelectSql =
            @"SELECT 
                Helps.Id AS Helps_Id,                
                Helps.Content AS Helps_Content                
            FROM 
                Helps 
            {0}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Helps.Id = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql);

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetLiveList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql);

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add
        public static int Add(string Content)
        {
            // Build Sql
            String Sql = String.Format(
                "INSERT INTO Helps (Content) VALUES ({0})",
                DbUtilities.SqlSafe(Content));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM Helps", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, string Content)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE Helps SET Content = {1} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),                               
                DbUtilities.SqlSafe(Content));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM Helps WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }
        
    }
}
