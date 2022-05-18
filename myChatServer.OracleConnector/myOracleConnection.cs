using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace myChatServer.OracleConnector
{
    public class myOracleConnection
    {

        private static OracleConnection conn;

        public static OracleConnection getConnection()
        {
            if (conn == null)
            {
                conn = connectionSetting();
                return conn;
            }
            else
            {
                return conn;
            }

        }

        private static OracleConnection connectionSetting()
        {
            OracleConfiguration.OracleDataSources.Add("orcl_high", "(description= (retry_count=20)(retry_delay=3)(address=(protocol=tcps)(port=1522)(host=adb.ap-seoul-1.oraclecloud.com))(connect_data=(service_name=g21d97f7712585e_orcl_high.adb.oraclecloud.com))(security=(ssl_server_cert_dn=\"CN = adb.ap - seoul - 1.oraclecloud.com, OU = Oracle ADB SEOUL, O = Oracle Corporation, L = Redwood City, ST = California, C = US\")))");
            OracleConfiguration.StatementCacheSize = 25;
            OracleConfiguration.SelfTuning = false;
            OracleConfiguration.BindByName = true;
            OracleConfiguration.CommandTimeout = 60;
            OracleConfiguration.FetchSize = 1024 * 1024;
            OracleConnection conn = new OracleConnection("user id=PROJ_BLD; password=aldwlTkfkd1!; data source=orcl_high");
            conn.WalletLocation = @"D:\FNC\toyProj\Wallet_orcl";

            return conn;
        }

        public static DataTable getSqlData(string sql, OracleParameter[] paramArray = null)
        {
            DataTable dt = new DataTable();

            try
            {
                getConnection();

                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.BindByName = true;
                cmd.CommandText = sql;


                if (paramArray != null)
                {
                    for (int i = 0; i < paramArray.Length; i++)
                        cmd.Parameters.Add(paramArray[i]);
                }
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                DataSet ds = new DataSet();

                adapter.Fill(ds, "TABLE");

                dt = ds.Tables["TABLE"];



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
    }
}
