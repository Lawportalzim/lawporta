using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace Kay.Database
{
    /// <summary>	
    ///	Common SQL utilities for use with SQL Server	
    /// </summary>
    public class SqlServer
    {
        // ConnectionString
        // Returns the connection string property stored in the public.config file
        private static string ConnectionString
        {
            get
            {
                string TmpStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
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

        public static string GetValue(SqlCommand Cmd)
        {
            // Call overloaded method with default value
            return GetValue(Cmd, "");
        }

        public static string GetValue(string Sql, string DefaultValue)
        {
            // Throw error when no SQL is passed
            if (Sql == string.Empty) throw (new ArgumentNullException("string"));

            // Create a command object
            SqlCommand Cmd = new SqlCommand(Sql);

            // Return the result
            return GetValue(Cmd, DefaultValue);
        }

        public static string GetValue(SqlCommand Cmd, string DefaultValue)
        {
            // Throw error when no SQL is passed
            if (Cmd == null) throw (new ArgumentNullException("SqlCommand"));

            // Create and open a database connection	
            SqlConnection Cn = new SqlConnection(ConnectionString);
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

        // GetDataAdapter()
        // Returns a DataAdapter to fill a DataSet
        public static SqlDataAdapter GetDataAdapter(SqlCommand Cmd)
        {
            // Throw error when no Command object is passed
            if (Cmd == null) throw (new ArgumentNullException("SqlCommand"));

            // Create a database connection	(implicitly opened and closed when Fill is called)
            SqlConnection Cn = new SqlConnection(ConnectionString);

            // Assign connection to command
            Cmd.Connection = Cn;

            // Create SQL Adapter object
            return new SqlDataAdapter(Cmd);
        }

        // GetDataSet()
        // Returns a simple DataSet
        public static DataSet GetDataSet(string Sql)
        {
            // Throw error when no SQL is passed
            if (Sql == string.Empty) throw (new ArgumentNullException("string"));

            // Create a command object
            SqlCommand Cmd = new SqlCommand(Sql);

            // Get data set
            return GetDataSet(Cmd);
        }
        public static DataSet GetDataSet(SqlCommand Cmd)
        {
            // Throw error when no Command object is passed
            if (Cmd == null) throw (new ArgumentNullException("SqlCommand"));

            // Create a database connection	(implicitly opened and closed when Fill is called)
            SqlConnection Cn = new SqlConnection(ConnectionString);

            // Assign connection to command
            Cmd.Connection = Cn;

            // Get the DataAdapter object
            SqlDataAdapter DtAdp = new SqlDataAdapter(Cmd);

            // Create and fill the DataSet
            DataSet Data = new DataSet();
            DtAdp.Fill(Data);

            // Return the DataSet
            return Data;
        }

        // ExecuteSql()
        // Executes a SQL statement
        public static int ExecuteSql(string Sql)
        {
            // Throw error when no SQL is passed
            if (Sql == string.Empty) throw (new ArgumentNullException("string"));

            // Create a command object
            SqlCommand Cmd = new SqlCommand(Sql);

            // Create and open a database connection	
            SqlConnection Cn = new SqlConnection(ConnectionString);
            Cn.Open();

            // Assign connection to command
            Cmd.Connection = Cn;

            // Execute the stored procedure
            int RecordsAffected = Cmd.ExecuteNonQuery();

            // Close the connection
            Cn.Close();

            // Return result
            return RecordsAffected;
        }

        // ExecuteSp()
        // Executes a stored procedure that does not return any results				
        public static int ExecuteSp(SqlCommand Cmd)
        {
            // Throw error when no Command object is passed
            if (Cmd == null) throw (new ArgumentNullException("SqlCommand"));

            // Create and open a database connection	
            SqlConnection Cn = new SqlConnection(ConnectionString);
            Cn.Open();

            // Set Command properties
            Cmd.Connection = Cn;
            Cmd.CommandType = CommandType.StoredProcedure;

            // Execute the stored procedure
            int RecordsAffected = Cmd.ExecuteNonQuery();

            // Close the connection
            Cn.Close();

            // Return result
            return RecordsAffected;
        }

        // ExecuteSpGetData()
        // Executes a stored procedure that returns a Data Reader object			
        public static SqlDataReader ExecuteSpGetData(SqlCommand Cmd)
        {
            // Throw error when no Command object is passed
            if (Cmd == null) throw (new ArgumentNullException("SqlCommand"));

            // Create and open a database connection
            SqlConnection Cn = new SqlConnection(ConnectionString);
            Cn.Open();

            // Assign command properties
            Cmd.Connection = Cn;
            Cmd.CommandType = CommandType.StoredProcedure;

            // Return the data reader object (implicitly closes connection)
            return Cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // AddCmdParam
        // Adds paramaters to a SQL Command object
        public static void AddCmdParam(SqlCommand Cmd, string Name, SqlDbType Type, int Size, ParameterDirection Direction, object Value)
        {
            // Create Paramater object
            SqlParameter Param = new SqlParameter();
            Param.ParameterName = Name;
            Param.SqlDbType = Type;
            Param.Direction = Direction;
            Param.Size = Size;
            Param.Value = Value;

            // Add parameter object to SQL Command object
            Cmd.Parameters.Add(Param);
        }
    }
}
