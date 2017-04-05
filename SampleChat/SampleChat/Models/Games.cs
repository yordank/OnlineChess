using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleChat.Models
{
    public class Games
    {

        public int Id { get; set; }
        public string TypeOfGame { get; set; }

        public string Result { get; set; }

        public ICollection<Move> Moves { get; set; }

        public ICollection<GamePlayer> GamesPlayers { get; set; }
    }
}