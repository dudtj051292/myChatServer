using DBModule;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fncChatServerModules
{
    class FncSendMsg : FncTODO
    {
        public override string DoWork(JObject jobj)
        {
            string sql = "";

            //msg 관련한 
            jobj.GetValue("IP");
            jobj.GetValue("SABUN");
            jobj.GetValue("PW");

            MysqlModule.GetDataTable(sql);
            return "";
        }
    }
}
