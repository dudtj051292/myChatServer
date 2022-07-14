using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fncChatServerModules
{
    public abstract class FncTODO
    {
        public abstract string DoWork(JObject jobj );
    }
}
