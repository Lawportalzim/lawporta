using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayCategoryDAL
    {
        // SQL
        private const String SelectSql = "SELECT * FROM Categories {0}";

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
        public static DataTable GetDetails(String UrlPath, int Type)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE UrlPath = {0} AND Type = {1}", DbUtilities.SqlSafe(UrlPath), DbUtilities.SqlSafe(Type)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList(int Type)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Type = {0} AND Recycled = 0   ORDER BY Title ASC", DbUtilities.SqlSafe(Type)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetListByParent(int ParentId)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE ParentId = {0}  AND Recycled = 0  ORDER BY Title ASC", DbUtilities.SqlSafe(ParentId)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        // List
        public static DataTable GetList(int Type, int ParentId)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Type = {0} AND Recycled = 0   AND ParentId = {1} ORDER BY Title ASC", DbUtilities.SqlSafe(Type), DbUtilities.SqlSafe(ParentId)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetLiveList(int Type)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Status = 1 AND Type = {0} ORDER BY Title ASC", DbUtilities.SqlSafe(Type)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetRecycledList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Status = 1 AND Recycled = 1 ORDER BY Title ASC"));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetLiveListByParent(int Type, int ParentId)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Status = 1  AND Recycled = 0  AND ParentId = {1} AND Type = {0} ORDER BY Title ASC", DbUtilities.SqlSafe(Type), DbUtilities.SqlSafe(ParentId)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        //search
        public static DataTable Find(String Keywords)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format(@"
                                Where Categories.Title LIKE '%{0}%' ORDER BY Title ASC", Keywords));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable Find(String Keywords, int Type)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format(@"
                                Where Categories.Title LIKE '%{0}%' AND Categories.Type = {1} ORDER BY Title ASC", Keywords, Type));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        // Add
        public static int Add(int Type, String Title, String UrlPath, int Status, int ParentId, Boolean Recycled)
        {
            // Sort order
            int SortOrder = int.Parse(DbUtilities.GetValue("SELECT IFNULL(MAX(SortOrder), 0) FROM Categories", "0")) + 1;

            // Build Sql
            String Sql = String.Format(
                "INSERT INTO Categories (Type, Title, UrlPath, Status, SortOrder, ParentId, Recycled) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                DbUtilities.SqlSafe(Type),
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Status),
                DbUtilities.SqlSafe(SortOrder),
                DbUtilities.SqlSafe(ParentId),
                DbUtilities.SqlSafe(Recycled));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM Categories", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, int Type, String Title, String UrlPath, int Status, int ParentId, Boolean Recycled)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE Categories SET Type = {1}, Title = {2}, UrlPath = {3}, Status = {4}, ParentId = {5}, Recycled = {6} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(Type),
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Status),
                DbUtilities.SqlSafe(ParentId),
                DbUtilities.SqlSafe(Recycled));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Sort
        public static Boolean UpdateOrder(int Id, int ReferenceId, String Direction)
        {
            String Sql;

            // Get new sort order
            Sql = String.Format(
                "SELECT SortOrder FROM Categories WHERE Id = {0}",
                DbUtilities.SqlSafe(ReferenceId));
            Double NewSortOrder = Double.Parse(DbUtilities.GetValue(Sql, "0"));

            // Change existing sort orders
            Sql = String.Format(
                "UPDATE Categories SET SortOrder = SortOrder + 1 WHERE SortOrder {0} {1}",
                (Direction.ToLower() == "before" ? ">=" : ">"),
                DbUtilities.SqlSafe(NewSortOrder));
            DbUtilities.ExecuteSql(Sql);

            // Update selected record
            Sql = String.Format(
                "UPDATE Categories SET SortOrder = {0} WHERE Id = {1}",
                (Direction.ToLower() == "before" ? NewSortOrder.ToString() : (NewSortOrder + 1).ToString()),
                DbUtilities.SqlSafe(Id));
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM Categories WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Unique
        public static Boolean UniqueUrlPath(int Id, int Type, string UrlPath)
        {
            // Build SQL
            String Sql = String.Format(
                "SELECT COUNT(Id) FROM Categories WHERE UrlPath = {0} AND Id <> {1} AND Type = {2}",
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(Type));

            // Get the value 
            return int.Parse(DbUtilities.GetValue(Sql)) == 0;
        }
    }
}
