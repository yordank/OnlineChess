using SampleChat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;
using SignalRChat;
namespace SampleChat.Hubs
{
    public class Clock
    {
        public Clock(string White, string Black, long time, string Color,Game Game)
        {
            this.aTimer = new System.Timers.Timer();

            this.sw = new Stopwatch();

            this.aTimer.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);

            this.aTimer.Interval = time;

            this.Color = Color;

            this.Game = Game;

            this.White = White;

            this.Black = Black;

            
        }
        public System.Timers.Timer aTimer { get; set; }
        public Stopwatch sw { get; set; }

        public string Color { get; set; }

        public string White { get; set; }
        public string Black { get; set; }

 

        public Game Game { get; set; }

        public void start()
        {
            if (this.aTimer.Interval > 0)
            {
                sw.Start();
                aTimer.Start();
            }


        }

        public void stop()
        {
            sw.Stop();
            if (aTimer.Interval - sw.ElapsedMilliseconds > 0)
                aTimer.Interval = aTimer.Interval - sw.ElapsedMilliseconds;
            aTimer.Stop();
            sw.Reset();

        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {

            this.stop();

            using (var context = new ChatDbContext())
            {
                if (this.Color == "white")
                {
                    Game.Status = "White Lost on Time";

                    context.results.Add(new Results() { WhiteUserName = this.White, BlackUserName = this.Black, Result = "B" });
                    context.SaveChanges();



                }
                else
                {
                    Game.Status = "Black Lost on Time";

                    context.results.Add(new Results() { WhiteUserName = this.White, BlackUserName = this.Black, Result = "W" });
                    context.SaveChanges();
                }

            }
        }


    }
}