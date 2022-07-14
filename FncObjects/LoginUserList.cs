using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FncObjects
{
    public partial class userlist
    {

        List<FncUser> userList;

        private userlist()
        {
            userList = new List<FncUser>();
        }

        private static Lazy<userlist> _loginUsers = new Lazy<userlist>(() => new userlist());

        public static userlist LoginUsers
        {
            get { return _loginUsers.Value; }
        }
        
        public bool Contains(string sabun)
        {
            bool result = false;

            foreach(FncUser s in userList)
            {
                if (s.Sabun == sabun)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool Remove(string sabun)
        {
            bool result = false;
            foreach (FncUser s in userList)
            {
                if (s.Sabun == sabun)
                {
                    userList.Remove(s);
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool Add(FncUser user)
        {
            bool result = false;
            foreach (FncUser s in userList)
            {
                if (s.Sabun == user.Sabun)
                {
                    result = true;
                    break;
                }
            }
            if(result == false)
            {
                userList.Add(user);
            }
            return result;
        }
    }

}
