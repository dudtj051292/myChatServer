using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myChetServer.Utils
{
    public class Utils
    {
        public static string getObjectToString(object o)
        {
            if(o == null || o.ToString().Trim() =="")
                return "";

            return o.ToString();
        }
    }
}
