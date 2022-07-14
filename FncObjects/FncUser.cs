using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FncObjects
{
    public class FncUser
    {
        public FncUser() { }
        public FncUser(string Sabun)
        {
            this.sabun = Sabun;
        }
        private string sabun;

        public string Sabun { get { return sabun; } }
    }
}
