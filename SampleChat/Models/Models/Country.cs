using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class Country
    {
        
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Country")]
        public virtual ICollection<ApplicationUser> Players { get; set; }
    }
}