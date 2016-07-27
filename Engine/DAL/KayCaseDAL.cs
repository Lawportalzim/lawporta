using System;
using System.Data;
using Kay.Database;
using Kay.Global;

namespace Kay.DAL
{
    class KayCaseDAL
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
                Cases.Appealed AS Cases_Appealed,
                Cases.Notes AS Cases_Notes,
                Cases.CaseType AS Cases_CaseType,
                Cases.Ruler AS Cases_Ruler,
                Cases.FullCase AS Cases_FullCase,
                Cases.RulerUrl AS Cases_RulerUrl,
                Cases.Recycled AS Cases_Recycled                
            FROM 
                Cases 
            {0}";

        // Details
        public static DataTable GetDetails(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Cases.Id = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetDetailsByParent(int Id)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Cases.ParentId = {0}", DbUtilities.SqlSafe(Id)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetDetails(string UrlPath)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format("WHERE Cases.UrlPath = {0}", DbUtilities.SqlSafe(UrlPath)));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        // List
        public static DataTable GetList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "WHERE Cases.Recycled = 0 ORDER BY Cases.Number DESC");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetList(int Type)
        {
            return DbUtilities.GetDataSet(string.Format("SELECT                 \r\n                Cases.Id AS Cases_Id,\r\n                Cases.Number AS Cases_Number,\r\n                Cases.ParentId AS Cases_ParentId,\r\n                Cases.Plaintiff AS Cases_Plaintiff,\r\n                Cases.Defendant AS Cases_Defendant,\r\n                Cases.Dates AS Cases_Dates,\r\n                Cases.CourtType AS Cases_CourtType,\r\n                Cases.CourtName AS Cases_CourtName,\r\n                Cases.UrlPath AS Cases_UrlPath,\r\n                Cases.Appealed AS Cases_Appealed,\r\n                Cases.Notes AS Cases_Notes,\r\n                Cases.CaseType AS Cases_CaseType,\r\n                Cases.Ruler AS Cases_Ruler,\r\n                Cases.FullCase AS Cases_FullCase,\r\n                Cases.RulerUrl AS Cases_RulerUrl,\r\n                Cases.Recycled AS Cases_Recycled                \r\n            FROM \r\n                Cases \r\n            {0}", (object)string.Format("WHERE Cases.Recycled = 0 AND Cases.CaseType = {0} ORDER BY Cases.Number DESC", (object)Type))).Tables[0];
        }
        public static DataTable GetLiveList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "WHERE Cases.Recycled = 0 ORDER BY Cases.Number DESC");

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        public static DataTable GetRecycledList()
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                "WHERE Cases.Recycled = 1 ORDER BY Cases.Number DESC");

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
        //search
        public static DataTable Find(String Keywords)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format(@"
                                Where
                                (Cases.Number LIKE '%{0}%' OR
                                Cases.Plaintiff LIKE '%{0}%' OR
                                Cases.Defendant LIKE '%{0}%'  OR 
                                Cases.DATES LIKE '%{0}%' OR
                                Cases.CourtName LIKE '%{0}%' OR
                                Cases.Ruler LIKE '%{0}%' OR
                                Cases.Notes LIKE '%{0}%') AND
                                Cases.Recycled = 0 ORDER BY Cases.Number DESC", Keywords));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }

        public static DataTable Find(String Keywords, int Type)
        {
            // Build Sql
            String Sql = String.Format(
                SelectSql,
                String.Format(@"
                                Where
                                ((Cases.Number LIKE '%{0}%' OR
                                Cases.Plaintiff LIKE '%{0}%' OR
                                Cases.Defendant LIKE '%{0}%'  OR 
                                Cases.DATES LIKE '%{0}%' OR
                                Cases.CourtName LIKE '%{0}%' OR
                                Cases.Ruler LIKE '%{0}%' OR
                                Cases.Notes LIKE '%{0}%') AND
                                Cases.Recycled = 0) AND Cases.CaseType = {1} ORDER BY Cases.Number DESC", Keywords, Type));

            // Return data
            return DbUtilities.GetDataSet(Sql).Tables[0];
        }
        // Add
        public static int Add(string Plaintiff, string Defendant, string Date, int CourtType, string CourtName, string UrlPath, string Notes, int CaseType, int ParentId, Boolean Appealed, string Ruler, string CaseNumber, string FullCase, Boolean Recycled, string RulerUrl)
        {
            // Build Sql
            String Sql = String.Format(
                "INSERT INTO Cases (Plaintiff, Defendant, Dates, CourtType, CourtName, UrlPath, Notes, CaseType, ParentId, Appealed, Ruler, Number, FullCase, Recycled, RulerUrl) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10},{11},{12},{13},{14})",               
                DbUtilities.SqlSafe(Plaintiff),
                DbUtilities.SqlSafe(Defendant),               
                DbUtilities.SqlSafe(Date),               
                DbUtilities.SqlSafe(CourtType),
                DbUtilities.SqlSafe(CourtName),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Notes),
                DbUtilities.SqlSafe(CaseType),
                DbUtilities.SqlSafe(ParentId),
                DbUtilities.SqlSafe(Appealed),
                DbUtilities.SqlSafe(Ruler),
                DbUtilities.SqlSafe(CaseNumber),
                DbUtilities.SqlSafe(FullCase),
                DbUtilities.SqlSafe(Recycled),
                DbUtilities.SqlSafe(RulerUrl));

            // Execute
            Boolean Success = DbUtilities.ExecuteSql(Sql) > 0;

            // Get ID
            return Success ? int.Parse(DbUtilities.GetValue("SELECT MAX(Id) FROM Cases", "0")) : 0;
        }

        // Update
        public static Boolean Update(int Id, string Plaintiff, string Defendant, string Date, int CourtType, string CourtName, string UrlPath, string Notes, int CaseType, int ParentId, Boolean Appealed, string Ruler, string CaseNumber, string FullCase, Boolean Recycled, string RulerUrl)
        {
            // Build SQL
            String Sql = String.Format(
                "UPDATE Cases SET Plaintiff = {1}, Defendant = {2}, Dates = {3}, CourtType = {4}, CourtName = {5}, UrlPath = {6}, Notes = {7}, CaseType = {8}, ParentId = {9}, Appealed = {10}, Ruler = {11}, Number = {12}, FullCase = {13}, Recycled = {14}, RulerUrl = {15} WHERE Id = {0}",
                DbUtilities.SqlSafe(Id),
                DbUtilities.SqlSafe(Plaintiff),
                DbUtilities.SqlSafe(Defendant),
                DbUtilities.SqlSafe(Date),
                DbUtilities.SqlSafe(CourtType),
                DbUtilities.SqlSafe(CourtName),
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Notes),
                DbUtilities.SqlSafe(CaseType),
                DbUtilities.SqlSafe(ParentId),
                DbUtilities.SqlSafe(Appealed),
                DbUtilities.SqlSafe(Ruler),
                DbUtilities.SqlSafe(CaseNumber),
                DbUtilities.SqlSafe(FullCase),
                 DbUtilities.SqlSafe(Recycled),
                DbUtilities.SqlSafe(RulerUrl));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Delete
        public static Boolean Delete(int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "DELETE FROM Cases WHERE Id = {0}",
                DbUtilities.SqlSafe(Id));

            // Execute
            return DbUtilities.ExecuteSql(Sql) > 0;
        }

        // Unique
        public static Boolean UniqueUrlPath(int Id, string UrlPath)
        {
            // Build SQL
            String Sql = String.Format(
                "SELECT COUNT(Id) FROM Cases WHERE UrlPath = {0} AND Id <> {1}",
                DbUtilities.SqlSafe(UrlPath),
                DbUtilities.SqlSafe(Id));

            // Get the value 
            return int.Parse(DbUtilities.GetValue(Sql)) == 0;
        }
        public static Boolean UniqueCaseNumber(String CaseNumber, int Id)
        {
            // Build SQL
            String Sql = String.Format(
                "SELECT COUNT(Id) FROM Cases WHERE Cases.Number = {0} AND Id <> {1}",
                DbUtilities.SqlSafe(CaseNumber),
                DbUtilities.SqlSafe(Id));

            // Get the value 
            return int.Parse(DbUtilities.GetValue(Sql)) == 0;
        }
       
    }
}
