using FncObjects;
using myChatServer.OracleConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                StringBuilder msg = new StringBuilder();

                int index = 0;
                string txt = getJSonMsg(reader);
                
                JObject obj = (JObject)JsonConvert.DeserializeObject(txt);

                writer.WriteLine("몬가일어나고있어");
                WorkType workType = (FncObjects.WorkType)Enum.Parse(typeof(FncObjects.WorkType), Convert.ToString(obj.GetValue("TYPE")));
                //string str = reader.ReadLine();

                switch (workType)
                {
                    case WorkType.LOGIN: 
                        Console.WriteLine("LOGIN");
                        break;
                    case WorkType.SEND_MSG:
                        Console.WriteLine("SendMsg");
                        break;
                    case WorkType.SEND_DATA:
                        Console.WriteLine("SendData");
                        break;
                    case WorkType.MAKE_CHAT:
                        Console.WriteLine("MakeChat");
                        break;
                }


                //if(str == "getMember")
                //{
                //    string sqlString = 
                //        $@"SELECT A.SEQ, A.NAME, A.DEPT, A.TITLE 
                //             FROM FNC_USER A  
                //            WHERE SEQ <> '{str.Replace("getMember", "") }'";
                //    str = Utils.Utils.getDataTableToJSON(myOracleConnection.getSqlData(sqlString));
                //    writer.WriteLine(str);
                //}
                //else if ( str == "getDept")
                //{
                //    string sqlString =
                //        $@"SELECT A.DEPT, A.DEPTNAME
                //             FROM FNC_DEPT A";
                //    str = Utils.Utils.getDataTableToJSON(myOracleConnection.getSqlData(sqlString));
                //    writer.WriteLine(str);
                //}
                //else
                //{
                //    Dictionary<string,string> dic = Utils.Utils.getJSONtoDictonary(str);

                //    StringBuilder builder = new StringBuilder();
                //    builder.Append(dic["Sender"]).Append(":").Append(dic["text"]);

                //    writer.WriteLine(builder);
                //}

            }

        }

        private string getJSonMsg(StreamReader reader)
        {
            StringBuilder msg = new StringBuilder();
            while(reader.Peek() >= 0)
                msg.Append((char)reader.Read());
            Console.WriteLine(msg.ToString());
            return msg.ToString();
        }
    }
}
