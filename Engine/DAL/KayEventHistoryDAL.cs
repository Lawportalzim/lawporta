using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayEventHistoryDAL
    {
        // SQL
        private const String SelectSql = "SELECT * FROM EventHistory {0}";

        // Details
        public static DataTable GetDetails(Double Id)
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
                "ORDER BY Date DESC");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetListByType(int TypeId)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Type = {0} ORDER BY Date DESC", DbUtilities.SqlSafe(TypeId)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetTaskList(int Count)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Type = 3 ORDER BY Date DESC LIMIT {0}", Count));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add
        public static Double Add(string Title, string Data, int Type)
        {
            // Build Sql
            String Sql = String.Format(
                "INSERT INTO EventHistory (Title, Data, Type, Date) VALUES ({0}, {1}, {2}, {3})",
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(Data),
                DbUtilities.SqlSafe(Type),
                DbUtilities.SqlSafe(DateTime.Now));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? Double.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM EventHistory", "0")) : 0;
        }

        // Update
        public static Boolean Update(Double Id, string Title, string Data)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE EventHistory SET Title = {1}, Data = {2} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(Data));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(Double Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM EventHistory WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }
        public static Boolean Clear(int TypeId)
        {
            // Build SQL
            String Sql = String.Format("DELETE FROM EventHistory WHERE Type = {0}", DbUtilities.SqlSafe(TypeId));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }
    }
}
