using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Kay.Database
{
    /// <summary>	
    ///		Common SQL utilities for use with MySQL	
    /// </summary>
    public class MySql
    {
        // Returns the connection string property stored in the public.config file
        private static string connectionString = String.Empty;
        public static String ConnectionString
        {
            get
            {
                if (connectionString == String.Empty)
                {
                    connectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
                }
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        // Returns a single value using a SQL string or a Command object (defaults to empty string)		
        public static string GetValue(string Sql)
        {
            return GetValue(Sql, "");
        }
        public static string GetValue(MySqlCommand Cmd)
        {
            return GetValue(Cmd, "");
        }
        public static string GetValue(string Sql, string DefaultValue)
        {
            // Throw error when no SQL is passed
            if (Sql == string.Empty) throw (new ArgumentNullException("string"));

            // Create a command object
            MySqlCommand Cmd = new MySqlCommand(Sql);

            // Return the result
            return GetValue(Cmd, DefaultValue);
        }
        public static string GetValue(MySqlCommand Cmd, string DefaultValue)
        {
            // Throw error when no SQL is passed
            if (Cmd == null) throw (new ArgumentNullException("MySqlCommand"));

            // Store result
            string CmdResult = "";

            // Create a connection
            using (MySqlConnection Cn = new MySqlConnection(ConnectionString))
            {
                // Open
                Cn.Open();

                // Assign connection to command
                Cmd.Connection = Cn;

                // Get the result
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
            }

            // Return the result
            return CmdResult;
        }

        // Returns a simple DataSet
        public static DataSet GetDataSet(string Sql)
        {
            // Throw error when no SQL is passed
            if (Sql == string.Empty) throw (new ArgumentNullException("string"));

            // Create a command object
            MySqlCommand Cmd = new MySqlCommand(Sql);

            // Get data set
            return GetDataSet(Cmd);
        }
        public static DataSet GetDataSet(MySqlCommand Cmd)
        {
            // Throw error when no Command object is passed
            if (Cmd == null) throw (new ArgumentNullException("MySqlCommand"));

            // Create a return dataset
            DataSet Data = new DataSet();

            // Create a database connection	(implicitly opened and closed when Fill is called)
            using (MySqlConnection Cn = new MySqlConnection(ConnectionString))
            {
                // Assign connection to command
                Cmd.Connection = Cn;

                // Get the DataAdapter object
                MySqlDataAdapter DtAdp = new MySqlDataAdapter(Cmd);

                // Create and fill the DataSet
                DtAdp.Fill(Data);
            }

            // Return the DataSet
            return Data;
        }

        // Executes a SQL statement
        public static int ExecuteSql(string Sql)
        {
            // Throw error when no SQL is passed
            if (Sql == string.Empty) throw (new ArgumentNullException("string"));

            // Create a command object
            MySqlCommand Cmd = new MySqlCommand(Sql);

            // Create return value
            int RecordsAffected = 0;

            // Create and open a database connection	
            using (MySqlConnection Cn = new MySqlConnection(ConnectionString))
            {
                // Open
                Cn.Open();

                // Assign connection to command
                Cmd.Connection = Cn;

                // Execute the stored procedure
                RecordsAffected = Cmd.ExecuteNonQuery();

                // Close the connection
                Cn.Close();
            }

            // Return result
            return RecordsAffected;
        }

        // Executes a stored procedure that does not return any results				
        public static int ExecuteSp(MySqlCommand Cmd)
        {
            // Throw error when no Command object is passed
            if (Cmd == null) throw (new ArgumentNullException("MySqlCommand"));

            // Create return value
            int RecordsAffected = 0;

            // Create a database connection	
            using (MySqlConnection Cn = new MySqlConnection(ConnectionString))
            {
                // Open
                Cn.Open();

                // Set Command properties
                Cmd.Connection = Cn;
                Cmd.CommandType = CommandType.StoredProcedure;

                // Execute the stored procedure
                RecordsAffected = Cmd.ExecuteNonQuery();

                // Close the connection
                Cn.Close();
            }

            // Return result
            return RecordsAffected;
        }

        // Executes a stored procedure that returns a Data Reader object			
        public static MySqlDataReader ExecuteSpGetData(MySqlCommand Cmd)
        {
            // Throw error when no Command object is passed
            if (Cmd == null) throw (new ArgumentNullException("MySqlCommand"));

            // Create return valud
            MySqlDataReader reader = null;

            // Create a database connection
            using (MySqlConnection Cn = new MySqlConnection(ConnectionString))
            {
                // Open
                Cn.Open();

                // Assign command properties
                Cmd.Connection = Cn;
                Cmd.CommandType = CommandType.StoredProcedure;

                // Return the data reader object (implicitly closes connection)
                reader = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            // Return dataset
            return reader;
        }

        // Adds paramaters to a SQL Command object
        public static void AddCmdParam(MySqlCommand Cmd, string Name, MySqlDbType Type, int Size, ParameterDirection Direction, object Value)
        {
            // Create Paramater object
            MySqlParameter Param = new MySqlParameter();
            Param.ParameterName = Name;
            Param.MySqlDbType = Type;
            Param.Direction = Direction;
            Param.Size = Size;
            Param.Value = Value;

            // Add parameter object to SQL Command object
            Cmd.Parameters.Add(Param);
        }
    }
}
