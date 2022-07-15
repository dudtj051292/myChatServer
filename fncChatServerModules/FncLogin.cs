using DBModule;
using FncObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fncChatServerModules
{
    public class FncLogin : FncTODO
    {
        public override string DoWork(JObject jobj)
        {
            string sql = "";

            jobj.GetValue("IP");

            string sabun = Convert.ToString(jobj.GetValue("SABUN"));
            string pw    = Convert.ToString(jobj.GetValue("PW"));
            FncUser user = new FncUser();

            int result = (int)FncObjects.RESULT.FAIL;
            string caption = string.Empty;
            string contents = string.Empty;
            int errorCode = (int)FncObjects.LOGIN.UNKNOWS;
            FncObjects.LOGIN Login = MysqlModule.Login(sabun, pw, out user);
            if (Login == LOGIN.INCORRECT)
            {
                result = (int)FncObjects.RESULT.FAIL;
                caption = "";
                contents = "";
                errorCode = (int)Login;
            }
            else if (Login == LOGIN.SUCCESS)
            {
                result = (int)FncObjects.RESULT.SUCCESS;
                caption = "";
                contents = "";
                errorCode = (int)Login;
                userlist.LoginUsers.Add(user);
            }else if(Login == LOGIN.UNKNOWS)
            {
                result = (int)FncObjects.RESULT.FAIL;
                caption = "";
                contents = "";
                errorCode = (int)Login;
            }
            return ErrorTextModule.getInstance.MakeMsg(WorkType.LOGIN, result, caption, contents, errorCode);
            MysqlModule.GetDataTable(sql);
        }
    }
}
