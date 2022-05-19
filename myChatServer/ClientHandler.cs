using myChatServer.OracleConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace myChatServer
{
    class ClientHandler
    {
        Socket socket = null;

        NetworkStream stream = null;
        StreamReader reader = null;
        StreamWriter writer = null;

        public ClientHandler(Socket socket)
        {
            this.socket = socket;
        }

        public void chat()
        {
            stream = new NetworkStream(socket);
            Encoding en = Encoding.GetEncoding("utf-8");
            reader = new StreamReader(stream, en);
            writer = new StreamWriter(stream, en) { AutoFlush = true };

            while (true)
            {
                string str = reader.ReadLine();
                
                if(str == "getMember")
                {
                    string sqlString = 
                        $@"SELECT A.SEQ, A.NAME, A.DEPT, A.TITLE 
                             FROM FNC_USER A  ";
                    str = Utils.Utils.getDataTableToJSON(myOracleConnection.getSqlData(sqlString));
                    writer.WriteLine(str);
                }
                else if ( str == "getDept")
                {
                    string sqlString =
                        $@"SELECT A.DEPT, A.DEPTNAME
                             FROM FNC_DEPT A";
                    str = Utils.Utils.getDataTableToJSON(myOracleConnection.getSqlData(sqlString));
                    writer.WriteLine(str);
                }
                else
                {
                    Dictionary<string,string> dic = Utils.Utils.getJSONtoDictonary(str);

                    StringBuilder builder = new StringBuilder();
                    builder.Append(dic["Sender"]).Append(":").Append(dic["text"]);

                    writer.WriteLine(builder);
                }

            }
           
        }


    }
}
