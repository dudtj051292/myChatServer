using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DBModule
{
    public class MysqlModule
    {
        private static Lazy<MysqlModule> module = new Lazy<MysqlModule>(() => new MysqlModule());
        private bool connetState { get { return connetState; } }

        private static List<string> sqlList = new List<string>();

#if DEBUG
        private static string mysql_conn_str = "server=127.0.0.1;user id=intranet;password=dlsxmfkspt;database=intranet;Charset=utf8mb4;";
#else
        private static string mysql_conn_str = "server=112.216.159.66;user id=intranet;password=dlsxmfkspt;database=intranet;Charset=utf8;";
#endif
        private static MySqlConnection sqlCon = new MySqlConnection(mysql_conn_str);
        private static MySqlTransaction sqlTrans = null;
        private static MySqlCommand sqlCmd = new MySqlCommand();

        private MysqlModule()
        {

        }
        public static MysqlModule mySqlModule
        {
            get { return mySqlModule; }
        }

        private static bool OpenConnection()
        {
            bool isSuccess = false;
            if (sqlCon.State != System.Data.ConnectionState.Open)
            {
                CloseConnection();
                sqlCon.Open();
                isSuccess = true;
            }
            return isSuccess;
        }
        private static bool CloseConnection()
        {
            bool success = false;
            if (sqlCon.State != System.Data.ConnectionState.Closed)
            {
                sqlCon.Close();
                success = true;
            }
            return success;
        }
        public static bool BeginTransaction()
        {
            bool success = false;
            // if (sqlCon.State == ConnectionState.Closed)
            if (sqlTrans != null)
                CloseConnection();
            OpenConnection();

            sqlCmd = sqlCon.CreateCommand();
            sqlTrans = sqlCon.BeginTransaction(System.Data.IsolationLevel.Serializable);
            //sqlTrans = sqlCon.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction = sqlTrans;
            success = true;


            return success;
        }
        public static bool CommitTransaction()
        {
            bool success = false;
            if (sqlCon.State != System.Data.ConnectionState.Closed)
            {
                if (sqlTrans != null)
                {
                    sqlTrans.Commit();
                    success = true;
                    sqlTrans = null;
                }
            }
            return success;
        }
        public static bool RollbackTransaction()
        {
            bool success = false;
            if (sqlCon.State != System.Data.ConnectionState.Closed)
            {
                if (sqlTrans != null)
                {
                    sqlTrans.Rollback();
                    success = true;
                    sqlTrans = null;
                }
            }
            return success;
        }
        public static DataTable GetDataTable(string sql, bool openNotePad = false, bool showProgressPopup = false)
        {

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = new MySqlCommand(sql, sqlCon);
            if (openNotePad)
            {
                StreamWriter writer = File.CreateText("sql.txt");
                writer.WriteLine(sql);
                writer.Close();
                System.Diagnostics.Process.Start("sql.txt");

            }

            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            adapter.Fill(dt);

            return dt;
        }

        public static void FillGridView(string sql, DataTable mstTable, bool openNotePad = false)
        {
            var dt = new DataTable();
            dt = MysqlModule.GetDataTable(sql, openNotePad);
        }


        public static bool ExecuteNonQuery(bool openNotePad = false, bool popupOpen = false)
        {

            bool isSuccess = true;
            string nowSql = "";
            try
            {
                if (sqlCon.State != System.Data.ConnectionState.Open)
                    OpenConnection();

                BeginTransaction();

                foreach (string sql in sqlList)
                {
                    sqlCmd.Connection = sqlCon;
                    sqlCmd.CommandType = System.Data.CommandType.Text;
                    sqlCmd.CommandText = sql;
                    nowSql = sql;
                    sqlCmd.ExecuteNonQuery();
                }
                CommitTransaction();
            }
            catch (Exception ex)
            {
                try
                {
                    RollbackTransaction();
                }
                catch (MySqlException e)
                {
                }
                isSuccess = false;
            }
            finally
            {
                sqlCmd.Dispose();
                sqlCmd = null;
                if (sqlCon.State == System.Data.ConnectionState.Open)
                    CloseConnection();
                sqlList.Clear();
                if (openNotePad)
                {
                    //여기에 텍스트 파일 오픈 하는 로직 추가
                }
            }

            return isSuccess;
        }

        public static DataTable getSqlData()
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public static bool Login()
        {
            bool isSuccess = false;


            return isSuccess;
        }

        public static void AddSql(string sql)
        {
            sqlList.Add(sql);
        }

    }
}
