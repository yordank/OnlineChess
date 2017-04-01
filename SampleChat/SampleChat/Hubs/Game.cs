using SampleChat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;

namespace SampleChat.Hubs
{
    public class Game

    {
        public Game(string White, string Black, long time)
        {

            WhiteClock = new Clock(White,Black,time,"white",this);

            BlackClock = new Clock(White,Black,time,"black",this);
                      
            this.White = White;

            this.Black = Black;

            this.Status = "Playing";
 
        }

   

        public Clock WhiteClock { get; set; }

        public Clock BlackClock { get; set; }

        public string White { get; set; }
        public string Black { get; set; }

        public string Status { get; set; }

       

        public void startClock(string side)
        {
            if (side == "white")
            {
                WhiteClock.start();
            }

            if (side == "black")
                BlackClock.start();

        }

        public void stopClock(string side)
        {

            if (side == "white")
                WhiteClock.stop();

            if (side == "black")
                BlackClock.stop();
        }

      


    }


}
