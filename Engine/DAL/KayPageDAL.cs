using System;
using System.Data;
using Kay.Global;
using Kay.Database;

namespace Kay.DAL
{
    class KayPageDAL
    {
        // SQL
        private const String SelectSql =
            @"SELECT 
                Pages.*, 
                PageTemplates.Title AS TemplateTitle, 
                PageTemplates.Filepath AS TemplateFilepath, 
                PageTemplates.Greedy AS TemplateGreedy
            FROM 
                Pages INNER JOIN 
                PageTemplates ON TemplateId = PageTemplates.Id {0}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Pages.Id = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetDetails(string UrlPath, int Device)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Pages.UrlPath = {0} AND Pages.Device = {1} AND PageTemplates.Title <> 'Shortcut'", DbUtilities.SqlSafe(UrlPath), DbUtilities.SqlSafe(Device)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(SelectSql, "ORDER BY Pages.SortOrder");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetList(int Device)
        {
            // Build filter
            String Filter = String.Format("WHERE Pages.Device = {0} ORDER BY Pages.SortOrder", DbUtilities.SqlSafe(Device));

            // Build Sql
            String Sql = String.Format(SelectSql, Filter);

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetSitemapList(int Device)
        {
            // Build filter
            String Filter = String.Format("WHERE Pages.Status = 1 AND Pages.ShowInMenu = 1 AND Pages.Device = {0} ORDER BY Pages.SortOrder", DbUtilities.SqlSafe(Device));

            // Build Sql
            String Sql = String.Format(SelectSql, Filter);

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        // Find
        public static DataTable Find(String Keywords)
        {
            // Build Sql
            String Sql = String.Format(@"                 

                SELECT 
	                cases.Plaintiff AS SearchPlaintiff,
                    cases.Defendant AS SearchDefendant, 
	                cases.Notes AS SearchNotes, 
	                cases.UrlPath AS SearchUrl,                     
	                'cases' AS SearchType 
                FROM 
	                cases                
                GROUP BY
                    cases.Plaintiff,
                    cases.Defendant,
                    cases.Notes,
                    cases.UrlPath",
                     Utilities.GetMySqlSearchFilter("cases.Plaintiff,cases.Defendant,cases.Notes", Keywords));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // Add
        public static int Add(int ParentId, int TemplateId, string Title, string ShortTitle, string BrowserTitle, string Description, string Keywords, string Content, string UrlPath, int Status, int ShowInMenu, int Device)
        {
            // Sort order
            int SortOrder = int.Parse(DbUtilities.GetValue("SELECT IFNULL(MAX(SortOrder), 0) FROM Pages", "0")) + 1;

            // Build Sql
            String Sql = String.Format(
                "INSERT INTO Pages (ParentId, TemplateId, Title, ShortTitle, BrowserTitle, Description, Keywords, Content, UrlPath, Status, ShowInMenu, SortOrder, Device) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12})",
                ParentId == 0 ? "NULL" : DbUtilities.SqlSafe(ParentId),
                DbUtilities.SqlSafe(TemplateId),
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(ShortTitle),
                DbUtilities.SqlSafe(BrowserTitle),
                DbUtilities.SqlSafe(Description),
                DbUtilities.SqlSafe(Keywords),
                DbUtilities.SqlSafe(Content),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Status),
                DbUtilities.SqlSafe(ShowInMenu),
                DbUtilities.SqlSafe(SortOrder),
                DbUtilities.SqlSafe(Device));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM Pages", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, int ParentId, int TemplateId, string Title, string ShortTitle, string BrowserTitle, string Description, string Keywords, string Content, string UrlPath, int Status, int ShowInMenu, int Device)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE Pages SET ParentId = {1}, TemplateId = {2}, Title = {3}, ShortTitle = {4}, BrowserTitle = {5}, Description = {6}, Keywords = {7}, Content = {8}, UrlPath = {9}, ShowInMenu = {10}, Status = {11}, Device = {12} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                ParentId == 0 ? "NULL" : DbUtilities.SqlSafe(ParentId),
                DbUtilities.SqlSafe(TemplateId),
                DbUtilities.SqlSafe(Title),
                DbUtilities.SqlSafe(ShortTitle),
                DbUtilities.SqlSafe(BrowserTitle),
                DbUtilities.SqlSafe(Description),
                DbUtilities.SqlSafe(Keywords),
                DbUtilities.SqlSafe(Content),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(ShowInMenu),
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
                "SELECT SortOrder FROM Pages WHERE Id = {0}",
                DbUtilities.SqlSafe(ReferenceId));
            Double NewSortOrder = Double.Parse(DbUtilities.GetValue(Sql, "0"));

            // Change existing sort orders
            Sql = String.Format(
                "UPDATE Pages SET SortOrder = SortOrder + 1 WHERE SortOrder {0} {1}",
                (Direction.ToLower() == "before" ? ">=" : ">"),
                DbUtilities.SqlSafe(NewSortOrder));
            DbUtilities.ExecuteSql(Sql);

            // Update selected record
            Sql = String.Format(
                "UPDATE Pages SET SortOrder = {0} WHERE Id = {1}",
                (Direction.ToLower() == "before" ? NewSortOrder.ToString() : (NewSortOrder + 1).ToString()),
                DbUtilities.SqlSafe(Id));
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM Pages WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Unique
        public static Boolean UniqueUrlPath(int Id, string UrlPath, int Device)
        {
            Boolean _unique = true;
            String Sql = String.Empty;

            /* Match exact URLs */
            if (_unique)
            {
                // Build SQL
                Sql = String.Format(
                    "SELECT COUNT(*) FROM Pages INNER JOIN PageTemplates ON PageTemplates.Id = Pages.TemplateId WHERE Pages.Id <> {1} AND Pages.Device = {2} AND Pages.UrlPath = {0} AND PageTemplates.Title <> 'Shortcut'",
                    DbUtilities.SqlSafe(UrlPath),
                    DbUtilities.SqlSafe(Id),
                    DbUtilities.SqlSafe(Device));

                // Get the value 
                _unique = (int.Parse(DbUtilities.GetValue(Sql)) == 0);
            }

            /* Match greedy URLs */
            if (_unique)
            {
                // Build SQL
                Sql = String.Format(
                    "SELECT COUNT(Pages.Id) FROM Pages INNER JOIN PageTemplates ON TemplateId = PageTemplates.Id WHERE Greedy = 1 AND Pages.Id <> {1} AND Pages.Device = {2} AND ({0} REGEXP CONCAT('^', Pages.UrlPath)) = 1",
                    DbUtilities.SqlSafe(UrlPath),
                    DbUtilities.SqlSafe(Id),
                    DbUtilities.SqlSafe(Device));

                // Get the value 
                _unique = (int.Parse(DbUtilities.GetValue(Sql)) == 0);
            }

            // Done
            return _unique;
        }
    }
}
