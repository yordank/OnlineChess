using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class GamePlayer
    {
        
        [Key, Column(Order = 0), ForeignKey("Game")]
        public int Game_Id { get; set; }
     
        public Games Game { get; set; }

  
        [Key, Column(Order = 1), ForeignKey("Player")]
        public string Player_Id { get; set; }
    
        public ApplicationUser Player { get; set; }
        public string Color { get; set; }
    }
}