using System;
using System.Data;

namespace Kay.Database
{
    /// <summary>	
    ///	Database utilities
    /// </summary>
    public class DbUtilities
    {
        /* SQL safe */
        public static String SqlSafe(Object Value)
        {
            return SqlSafe(Value, sqlDate, 0);
        }
        public static String SqlSafe(Object Value, int MaxLength)
        {
            return SqlSafe(Value, sqlDate, MaxLength);
        }
        public static String SqlSafe(Object Value, String DateFormatString, int MaxLength)
        {
            switch (Value.GetType().Name.ToString().ToLower())
            {
                case "datetime":
                    {
                        return "'" + ((DateTime)Value).ToString(DateFormatString) + "'";
                    }
                case "string":
                    {
                        String value = Value.ToString();

                        // Truncate
                        if (MaxLength > 0 && value.Length > MaxLength)
                            value = value.Substring(0, MaxLength);

                        // Replace invalid characters                        
                        if (value != String.Empty)
                        {
                            value = value.Replace("'", "''");
                            value = value.Replace("’", "''");
                            value = value.Replace("‘", "''");
                            value = value.Replace(@"\", @"\\");
                        }

                        // Wrap
                        return "'" + value + "'";
                    }
                case "bool":
                case "boolean":
                    {
                        return (bool)Value ? "1" : "0";
                    }
                default:
                    {
                        return Value.ToString();
                    }
            }
        }

        // Date formatting
        public const String sqlDate = "yyyy-MM-dd HH:mm:ss";
        public const String sqlDateStart = "yyyy-MM-dd 00:00:00";
        public const String sqlDateEnd = "yyyy-MM-dd 23:59:59";

        // Create BIT operator SQL
        public static string BitAnd(string FieldName, int CompareValue)
        {
            return BitAnd(FieldName, double.Parse(CompareValue.ToString()));
        }
        public static string BitAnd(string FieldName, double CompareValue)
        {
            switch (Global.Config.DatabaseType)
            {
                case "MsAccess": return "(" + FieldName + " BAND " + CompareValue.ToString() + ")";
                case "SqlServer": return "(" + FieldName + " & " + CompareValue.ToString() + ")";
                case "MySql": return "(" + FieldName + " & " + CompareValue.ToString() + ")";
                default: throw new Exception("DatabaseType not defined in public.config");
            }
        }

        // Return a single value
        public static string GetValue(string Sql)
        {
            return GetValue(Sql, "");
        }
        public static string GetValue(string Sql, string DefaultValue)
        {
            switch (Global.Config.DatabaseType)
            {
                case "MsAccess": return OleDb.GetValue(Sql, DefaultValue);
                case "SqlServer": return SqlServer.GetValue(Sql, DefaultValue);
                case "MySql": return MySql.GetValue(Sql, DefaultValue);
                default: throw new Exception("DatabaseType not defined in public.config");
            }
        }

        // Return a data set
        public static DataSet GetDataSet(string Sql)
        {
            switch (Global.Config.DatabaseType)
            {
                case "MsAccess": return OleDb.GetDataSet(Sql);
                case "SqlServer": return SqlServer.GetDataSet(Sql);
                case "MySql": return MySql.GetDataSet(Sql);
                default: throw new Exception("DatabaseType not defined in public.config");
            }
        }

        // Execute a SQL statement, return the number of records affected
        public static int ExecuteSql(string Sql)
        {
            switch (Global.Config.DatabaseType)
            {
                case "MsAccess": return OleDb.ExecuteSql(Sql);
                case "SqlServer": return SqlServer.ExecuteSql(Sql);
                case "MySql": return MySql.ExecuteSql(Sql);
                default: throw new Exception("DatabaseType not defined in public.config");
            }
        }
    }
}