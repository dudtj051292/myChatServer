using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fncChatServerModules
{
    class ErrorTextModule
    {
        private string caption;
        private string contents;
        private string errorCode;

        private ErrorTextModule() { }

        private static readonly Lazy<ErrorTextModule> _instance = new Lazy<ErrorTextModule>(() => new ErrorTextModule());

        public static ErrorTextModule getInstance
        {
            get { return _instance.Value; }
        }
 

        public string MakeMsg(FncObjects.WorkType type, int result, string caption , string contents, int errorCode)
        {
            JObject jobj = new JObject();
            jobj.Add(new JProperty("WORKTYPE", type));
            jobj.Add(new JProperty("RESULT", result));
            jobj.Add(new JProperty("CAPTION", caption));
            jobj.Add(new JProperty("CONTENTS", contents));
            jobj.Add(new JProperty("CODE", errorCode));
            return jobj.ToString();
        }


        
    }
}
