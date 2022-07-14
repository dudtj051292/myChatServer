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
using FncObjects;
using System.Security.Cryptography;

namespace DBModule
{
    public class MysqlModule
    {
        private static Lazy<MysqlModule> module = new Lazy<MysqlModule>(() => new MysqlModule());
        private bool connetState { get { return connetState; } }

        private static List<string> sqlList = new List<string>();

        private static readonly string KEY = "01234567890123456789012345678901";
        private static readonly string KEY_128 = KEY.Substring(0, 128 / 8);
        private static readonly string KEY_256 = KEY.Substring(0, 256 / 8);
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

        public static FncObjects.LOGIN Login(string ID, string PW, out FncUser user)
        {
            FncObjects.LOGIN login = LOGIN.UNKNOWS;
            user = null;
            try
            {
                string sql = string.Format("SELECT SABUN FROM HR WHERE SABUN = '{0}' AND PASSWD = '{1}'", ID, AESEncrypt256(PW));
                DataTable dt = GetDataTable(sql);
                if(dt.Rows.Count == 0)
                {
                    login = FncObjects.LOGIN.INCORRECT;
                }
                else
                {
                    user = new FncUser( Convert.ToString(dt.Rows[0]["SABUN"]));
                    login = FncObjects.LOGIN.SUCCESS;

                }
            }
            catch (Exception ex)
            {
                login = FncObjects.LOGIN.UNKNOWS;
            }
            finally
            {
                // 로그 찍을 일 있으면 작성

            }

            return login;
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

        public static void AddSql(string sql)
        {
            sqlList.Add(sql);
        }
        public static string AESDecrypt256(string encrypt)
        {

            try
            {
                byte[] encryptBytes = Convert.FromBase64String(encrypt);

                RijndaelManaged rm = new RijndaelManaged();

                rm.Mode = CipherMode.CBC;
                rm.Padding = PaddingMode.PKCS7;
                rm.KeySize = 256;

                MemoryStream memoryStream = new MemoryStream(encryptBytes);

                ICryptoTransform decryptor = rm.CreateDecryptor(Encoding.UTF8.GetBytes(KEY_256), Encoding.UTF8.GetBytes(KEY_128));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                byte[] plainBytes = new byte[encryptBytes.Length];

                int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

                string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

                cryptoStream.Close();
                memoryStream.Close();

                return plainString;
            }
            catch (Exception)
            {
                return null;
            }
        }
        //AES_256 암호화
        public static string AESEncrypt256(string plain)
        {
            try
            {
                //byte 변환
                byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

                RijndaelManaged rm = new RijndaelManaged();
                rm.Mode = CipherMode.CBC;
                rm.Padding = PaddingMode.PKCS7;
                rm.KeySize = 256;

                MemoryStream memoryStream = new MemoryStream();

                ICryptoTransform encryptor = rm.CreateEncryptor(Encoding.UTF8.GetBytes(KEY_256), Encoding.UTF8.GetBytes(KEY_128));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] encryptBytes = memoryStream.ToArray();

                string encryptString = Convert.ToBase64String(encryptBytes);

                cryptoStream.Close();
                memoryStream.Close();

                return encryptString;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
