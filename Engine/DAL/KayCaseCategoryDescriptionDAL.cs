using System;
using System.Data;
using Kay.Database;

namespace Kay.DAL
{
    class KayCaseCategoryDescriptionDAL
    {
        // SQL
        private const String SelectSql =
            @"SELECT  
                Cases.Id AS Cases_Id,
                Cases.Number AS Cases_Number,
                Cases.ParentId AS Cases_ParentId,
                Cases.Plaintiff AS Cases_Plaintiff,
                Cases.Defendant AS Cases_Defendant,
                Cases.Dates AS Cases_Dates,
                Cases.CourtType AS Cases_CourtType,
                Cases.CourtName AS Cases_CourtName,
                Cases.UrlPath AS Cases_UrlPath,
                Cases.Notes AS Cases_Notes,
                Cases.CaseType AS Cases_CaseType,
                Cases.Appealed AS Cases_Appealed,
                Cases.Ruler AS Cases_Ruler,
                Cases.FullCase AS Cases_FullCase,
                Cases.Recycled AS Cases_Recycled,
                Cases.RulerUrl AS Cases_RulerUrl,
                
                Categories.Id AS Categories_Id,
                Categories.ParentId AS Categories_ParentId,
                Categories.Type AS Categories_Type,
                Categories.Title AS Categories_Title,
                Categories.UrlPath AS Categories_UrlPath,
                Categories.Recycled AS Categories_Recycled,
               
                CaseCategoryDescriptions.Id AS CaseCategoryDescriptions_Id,
                CaseCategoryDescriptions.Description AS CaseCategoryDescriptions_Description,
                CaseCategoryDescriptions.UrlPath AS CaseCategoryDescriptions_UrlPath          
            FROM 
                CaseCategoryDescriptions LEFT OUTER JOIN
                Categories ON CaseCategoryDescriptions.CategoryId = Categories.Id
                LEFT OUTER JOIN
                Cases ON CaseCategoryDescriptions.CaseId = Cases.Id
            {0}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE CaseCategoryDescriptions.Id = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetDetails(string UrlPath)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE CaseCategoryDescriptions.UrlPath = {0}", DbUtilities.SqlSafe(UrlPath)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "ORDER BY Cases.Number DESC");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetLiveList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "WHERE CaseCategoryDescriptions.Status = 1  ORDER BY Cases.Number DESC");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetRecentList(int Count)
        {
            // Build filter
            String Filter = String.Format(
                "",
                DbUtilities.SqlSafe(Count));

            // Build Sql
            String Sql = String.Format(
                SelectSql,
                Filter);

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        //list by case
        public static DataTable GetListByCase(int CaseId)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE CaseId = {0} ORDER BY Cases.Number DESC", DbUtilities.SqlSafe(CaseId)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        //list by category
        public static DataTable GetListByCategory(int CategoryId)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE CategoryId = {0} ORDER BY Categories.Title ASC", DbUtilities.SqlSafe(CategoryId)));

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
                                Where CaseCategoryDescriptions.Description LIKE '%{0}%' AND Cases.Recycled = 0 ORDER BY Cases.Number DESC", Keywords));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable Find(String Keywords, int Type)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format(@"
                                Where (CaseCategoryDescriptions.Description LIKE '%{0}%' AND Cases.Recycled = 0) AND Cases.CaseType = {1}  ORDER BY Cases.Number DESC", Keywords, Type));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        // Add
        public static int Add(int CaseId, int CategoryId, string Description, string UrlPath)
        {
            // Build Sql
            String Sql = String.Format(
                "INSERT INTO CaseCategoryDescriptions (CaseId, CategoryId, Description, UrlPath) VALUES ({0}, {1}, {2}, {3})",               
                DbUtilities.SqlSafe(CaseId),
                DbUtilities.SqlSafe(CategoryId),               
                DbUtilities.SqlSafe(Description),               
                DbUtilities.SqlSafe(UrlPath));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM CaseCategoryDescriptions", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, int CaseId, int CategoryId, string Description, string UrlPath)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE CaseCategoryDescriptions SET CaseId = {1}, CategoryId = {2}, Description = {3}, UrlPath = {4} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(CaseId),
                DbUtilities.SqlSafe(CategoryId),
                DbUtilities.SqlSafe(Description),
                DbUtilities.SqlSafe(UrlPath));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM CaseCategoryDescriptions WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Unique
        public static Boolean UniqueUrlPath(int Id, string UrlPath)
        {
            // Build SQL
            String Sql = String.Format(
                "SELECT COUNT(Id) FROM CaseCategoryDescriptions WHERE UrlPath = {0} AND Id <> {1}",
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Id));

            // Get the value 
            return int.Parse(DbUtilities.GetValue(Sql)) == 0;
        }
       
    }
}
