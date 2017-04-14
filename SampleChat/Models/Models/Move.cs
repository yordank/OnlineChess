using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class Move
    {
        public int Id { get; set; }

        public string moveString { get; set; }

        public virtual ApplicationUser Player { get; set; }

    }
}