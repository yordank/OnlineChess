using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class Messages
    {
        public Messages(string message)
        {
            this.text = message;
        }

        public int Id { get; set; }
        public string text { get; set; }
    }
}