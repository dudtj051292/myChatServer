using DBModule;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fncChatServerModules
{
    class FncLogin : FncTODO
    {
        public override void DoWork(JObject jobj)
        {
            string sql = "";

            jobj.GetValue("SABUN")

            MysqlModule.GetDataTable(sql);
        }
    }
}
