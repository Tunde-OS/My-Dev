using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk.Sfc;
 
namespace database_sys
{
    class Program
    {
        static void Main(string[] args)
        {
            string ss = "";
            db_set_up db = new db_set_up();
         //  string ss= db.Create_Database("test");
        
          Console.WriteLine(ss);
          string[,] fields = new string[0, 0];
          //fields[0, 0] = "fname";
          //fields[0, 1] = "nvarchar(10)";
          //fields[0, 2] = "not null";
          //fields[1, 0] = "lname";
          //fields[1, 1] = "nvarchar(10)";
          //fields[1, 2] = "not null";
          //fields[2, 0] = "Class";
          //fields[2, 1] = "nvarchar(10)";
          //fields[2, 2] = "not null";
          //ss = db.Create_Table("test", "mine", "std_id", "int", true, true, fields);
        //  ss = db.Create_Table("test", "Class", "Class", "nvarchar(10)", true,false, fields);
          ss = db.Add_Foreign_Key("test", "mine", "Class", "Class", "Class", "nvarchar(10)");
           Console.WriteLine(ss);
           Console.ReadKey();
        }
    }

    public class db_set_up
    {
        public static SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adp;
        string database = "";
        string sql = "";
        public db_set_up()
        {
            conn = new SqlConnection("Data Source=.\\sqlexpress;Database=Master;Integrated Security=True;Pooling=False");
        }

        public string Create_Database(string db_name)
        {
            string result = "";
            if (db_name.Length != 0)
            {
                sql = "IF EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE name ='" + db_name + "') DROP database " + db_name + " CREATE database " + db_name;
                result = query(sql);

            }
            return result;
        }

        public string Create_Table(string database, string table_name, string first_column, string datat, bool is_primary, bool increment, string[,] fields_data_null)
        {

            string result = "";
            if (is_primary)
            {
                sql = "use " + database + " CREATE TABLE " + table_name + "(" + first_column + " " + datat + " PRIMARY KEY)";
                if (increment)
                {
                    sql = "use " + database + " CREATE TABLE " + table_name + "(" + first_column + " " + datat + " IDENTITY(1,1) PRIMARY KEY)";
                }
            }
            else
            {
                sql = "use " + database + " CREATE TABLE " + table_name + "(" + first_column + " " + datat + ")";
            }
            result = query(sql);

            if (fields_data_null.Length != 0)
            {
                //add other columns
                for (int i = 0; i < fields_data_null.Length; i++)
                {
                    try
                    {
                        sql = "use " + database + " ALTER TABLE " + table_name + " ADD " + fields_data_null[i, 0] + " " + fields_data_null[i, 1] + " " + fields_data_null[i, 2];
                        query(sql);
                    }
                    catch (Exception ex) { }
                    finally
                    {

                    }
                }
            }
            return result;
        }

        public string Append_Columns(string database, string table_name, string[,] fields_data_null)
        {
            string result = "";
            for (int i = 0; i < fields_data_null.Length; i++)
            {
                try
                {
                    sql = "use " + database + " ALTER TABLE " + table_name + " ADD " + fields_data_null[i, 0] + " " + fields_data_null[i, 1] + " " + fields_data_null[i, 2];
                    query(sql);
                }
                catch (Exception ex) { }
                finally
                {

                }
            }
            return result;
        }

        public string Add_Foreign_Key(string database, string table_name, string f_column, string fry_table, string fry_column, string datat)
        {
            //SchoolId int NOT NULL FOREIGN KEY REFERENCES SCHOOL (SchoolId),
            string result = "";
            sql = "use " + database + " ALTER TABLE " + table_name + " ALTER COLUMN " + f_column + " " + datat + " NOT NULL FOREIGN KEY REFERENCES " + fry_table + " (" + fry_column + ")";
            result = query(sql);
            return result;
        }
        string query(string que)
        {
            string err = "Query Executed Successfully";
            conn.Open();
            cmd = new SqlCommand(que, conn);
            cmd.Connection = conn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                err = ex.Message;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return err;
        }
    }
}
