using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleChat.Hubs
{
    public class UserConnection
    {
       
        public string userId { set; get; }

        public int Time { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as UserConnection;
            if (other == null)
            {
                return false;
            }
            return this.userId== other.userId && this.userId == other.userId;
        }
        public override int GetHashCode()
        {
            return userId.GetHashCode();
        }
    }

}