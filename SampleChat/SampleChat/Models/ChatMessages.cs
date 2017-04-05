using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class ChatMessages
    {
        public int Id { get; set; }
        public string Text { get; set; }

 
        public ApplicationUser Sender { get; set; }

 
        public ApplicationUser Receiver { get; set; }

        public DateTime date { get; set; }
    }
}