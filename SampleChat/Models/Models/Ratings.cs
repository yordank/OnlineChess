using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class Ratings
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

    

        public virtual ApplicationUser Player { get; set; }
        public DateTime date { get; set; }
    }
}