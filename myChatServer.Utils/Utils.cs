using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myChatServer.Utils
{
    public static class Utils
    {
        public static string getObjectToString(object o)
        {
            if(o == null || o.ToString().Trim() =="")
                return "";

            return o.ToString();
        }

        public static string getDataTableToJSON(DataTable table )
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public static Dictionary<string, string> getJSONtoDictonary(string json)
        {
            Dictionary < string,string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            
            return dic;
        }
    }


}
