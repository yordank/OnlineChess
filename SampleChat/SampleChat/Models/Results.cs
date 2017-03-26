using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class Results
    {
        public int Id { get; set; }
        public string WhiteUserName { get; set; }
        public string BlackUserName { get; set; }

        public string  Result { get; set; }
    }
}