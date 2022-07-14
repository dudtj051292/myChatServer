using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FncObjects
{
    public enum WorkType
    {
        LOGIN = 1,
        SEND_MSG = 2,
        SEND_DATA = 3,
        MAKE_CHAT = 4,
    }

    public enum LOGIN
    {
        SUCCESS = 0,
        INCORRECT = 1,
        UNKNOWS = 2,
    }

    public enum RESULT
    {
        FAIL = 0,
        SUCCESS = 1,        
    }
}
