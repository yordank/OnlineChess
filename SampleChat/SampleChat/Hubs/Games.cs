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
        public Game(string Name, string OpponentName, double time, string Color)
        {
            aTimer = new System.Timers.Timer();
            this.aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = time;

            this.sw = new Stopwatch();

            this.OpponentName = OpponentName;

            this.Status = "Playing";

            this.Color = Color;

            this.Name = Name;
        }
        public System.Timers.Timer aTimer { get; set; }
        public Stopwatch sw { get; set; }

        public string Name { get; set; }
        public string OpponentName { get; set; }

        public string Status { get; set; }

        public string Color { get; set; }

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



            aTimer.Stop();

            sw.Stop();

            aTimer.Dispose();

            Status = "Lost on Time";

            using (var context = new ChatDbContext())
            {
                if (this.Color == "white")
                {
                    context.results.Add(new Results() { WhiteUserName = this.Name, BlackUserName = this.OpponentName, Result = "B" });
                    context.SaveChanges();
                }
                else
                {
                    context.results.Add(new Results() { WhiteUserName = this.OpponentName, BlackUserName = this.Name, Result = "W" });
                    context.SaveChanges();
                }



            }



        }


    }


}
