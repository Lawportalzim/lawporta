using System;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

namespace Kay.Database
{
    /// <summary>	
    ///		Common SQL utilities for use with SQL Server	
    /// </summary>
    public class OleDb
    {
        // ConnectionString
        // Returns the connection string property stored in the public.config file
        private static string ConnectionString
        {
            get
            {
                string TmpStr = ConfigurationManager.ConnectionStrings["MsAccess"].ConnectionString;
                if (TmpStr == null || TmpStr == "")
                    throw (new ApplicationException("ConnectionString configuration is missing from your public.config"));
                else
                    return (TmpStr);
            }
        }

        // GetValue()
        // Returns a single value using a SQL string or a Command object (defaults to empty string)		
        public static string GetValue(string Sql)
        {
            // Call overloaded method with default value
            return GetValue(Sql, "");
        }

        public static string GetValue(OleDbCommand Cmd)
        {
            // Call overloaded method with default value
            return GetValue(Cmd, "");
        }

        public static string GetValue(string Sql, string DefaultValue)
        {
            // Throw error when no SQL is passed
            if (Sql == string.Empty) throw (new ArgumentNullException("string"));

            // Create a command object
            OleDbCommand Cmd = new OleDbCommand(Sql);

            // Return the result
            return GetValue(Cmd, DefaultValue);
        }

        public static string GetValue(OleDbCommand Cmd, string DefaultValue)
        {
            // Throw error when no SQL is passed
            if (Cmd == null) throw (new ArgumentNullException("OleDbCommand"));

            // Create and open a database connection
            OleDbConnection Cn = new OleDbConnection(ConnectionString);
            Cn.Open();

            // Assign connection to command
            Cmd.Connection = Cn;

            // Get the result
            string CmdResult = "";
            object Result = Cmd.ExecuteScalar();

            // Set default value if no results returned
            if (Result == null)
            {
                CmdResult = DefaultValue;
            }
            else
            {
                CmdResult = Result.ToString();
            }

            // Close the connection
            Cn.Close();

            // Return the result
            return CmdResult;
        }

        // GetDataReader()
        // Returns a simple DataReader object
        public static OleDbDataReader GetDataReader(string Sql)
        {
            // Throw error when no SQL is defined
            if (Sql == null) throw new Exception("SQL string was not defined");

            // Create and open a database connection
            OleDbConnection Cn = new OleDbConnection(ConnectionString);
            Cn.Open();

            // Assign command properties
            OleDbCommand Cmd = new OleDbCommand(Sql);
            Cmd.Connection = Cn;

            // Return the data reader object (implicitly closes connection)
            return Cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // GetDataSet()
        // Returns a simple DataSet
        public static DataSet GetDataSet(string Sql)
        {
            // Throw error when no SQL is defined
            if (Sql == null) throw new Exception("SQL string was not defined");

            // Create a database connection	(implicitly opened and closed when Fill is called)
            OleDbConnection Cn = new OleDbConnection(ConnectionString);

            // Assign command properties
            OleDbCommand Cmd = new OleDbCommand(Sql);
            Cmd.Connection = Cn;

            // Get the DataAdapter object
            OleDbDataAdapter DtAdp = new OleDbDataAdapter(Cmd);

            // Create and fill the DataSet
            DataSet Data = new DataSet();
            DtAdp.Fill(Data);

            // Create SQL Adapter object
            return Data;
        }

        // ExecuteSql()
        // Run SQL statement and return number of rows affected
        public static int ExecuteSql(string Sql)
        {
            // Throw error when no SQL is defined
            if (Sql == null) throw new Exception("SQL string was not defined");

            // Create a database connection	(implicitly opened and closed when Fill is called)
            OleDbConnection Cn = new OleDbConnection(ConnectionString);

            // Assign command properties
            OleDbCommand Cmd = new OleDbCommand(Sql);
            Cmd.Connection = Cn;

            // Execute the query
            Cn.Open();
            int RowsAffected = Cmd.ExecuteNonQuery();
            Cn.Close();

            // Return value
            return RowsAffected;
        }
    }
}
