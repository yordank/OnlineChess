using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using SampleChat.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Timers;
using System.Diagnostics;

namespace SignalRChat
{


public class UserConnection
    {
      public string UserName { set; get; }
      public string userId { set; get; }

      public int Time { get; set; }

      public override bool Equals(object obj)
      {
            var other = obj as UserConnection;
            if (other == null)
            {
                return false;
            }
            return this.UserName == other.UserName && this.userId== other.userId;
      }
        public override int GetHashCode()
        {
            return (UserName + userId).GetHashCode();
        }
    }
public class ChatHub : Hub
    {
        public static HashSet<UserConnection> usersWhoSeekOpponent = new HashSet<UserConnection>();

        public static Dictionary<string, OpponentAndTimer> usersOpponent = new Dictionary<string, OpponentAndTimer>();

        public void chatsend(string name ,string message)
        {
            string userId = usersOpponent[name].OpponentName;

            Clients.User(userId).addNewMessageToPage(name,message,0);

            Clients.User(name).addNewMessageToPage(name, message,0);

        }
        public void Send(string name, string moveString)
        {
            string userId = usersOpponent[name].OpponentName;


            if (usersOpponent[name].Status == "Lost on Time")
            {
                

                Clients.User(userId).addNewMessageToPage(name, $"{name} lost on time!", 1);

                Clients.User(name).addNewMessageToPage(name, $"{name} lost on time!", 1);

                return;
            }

           

            usersOpponent[name].stop();
            

            var Move = moveString;

            var SendMessage = new
            {
                ServiceName = "ChessGameMove",

                Data = Move,

                TimeOpponet = usersOpponent[name].aTimer.Interval/1000,

                TimeUser = usersOpponent[userId].aTimer.Interval/1000

            };

            usersOpponent[userId].start();

            var jsonChessGameMove = JsonConvert.SerializeObject(SendMessage);

            Clients.User(userId).getOpponentMove(jsonChessGameMove);
   
        
            using (var context = new ChatDbContext())
            {

               context.messages.Add(new Messages(moveString));
               context.SaveChanges();

            }

        }


      

        public void Seek(string name,int time)
        {
            
            var us = new UserConnection();
            us.UserName = name;
            us.userId = Context.User.Identity.Name;
            us.Time = time;



            usersWhoSeekOpponent.Add(us);

            
            Seekall();
            
        }
        public void Startgame(string username1,string username2,int time)
        {



            var user1 = usersWhoSeekOpponent.Where(x => x.UserName == username1).FirstOrDefault();
            var user2 = usersWhoSeekOpponent.Where(x => x.UserName == username2).FirstOrDefault();

            Clients.User(user1.userId).beginGame("white",time*60);
            Clients.User(Context.User.Identity.Name).beginGame("black",time*60);


            usersOpponent[username1]= new OpponentAndTimer( Context.User.Identity.Name,1000*60*time);
            usersOpponent[username2]=new OpponentAndTimer( user1.userId,1000*60*time);

            usersWhoSeekOpponent.Remove(user1);
            usersWhoSeekOpponent.Remove(user2);

            Seekall();
        }

        public void Seekall()
        {

            var Users = usersWhoSeekOpponent.Select(x => new { Username=x.UserName,Time=x.Time });

            var responceMessage = new
            {
                ServiceName="AllUsersWhoSeekOpponet",
                Data=Users
            };


            var jsonUsers = JsonConvert.SerializeObject(responceMessage);

            Clients.All.ResponceAllOpponents(jsonUsers);


        }

        

        public override Task OnDisconnected(bool stopCalled)
        {
            var user = usersWhoSeekOpponent.Where(x => x.UserName == Context.User.Identity.Name).FirstOrDefault();
            usersWhoSeekOpponent.Remove(user);
            Seekall();
            return base.OnDisconnected(stopCalled);
        }


    }


    public class OpponentAndTimer

    {
        public OpponentAndTimer(string OpponentName,double time)
        {
            aTimer = new System.Timers.Timer();
            this.aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = time;

            this.sw = new Stopwatch();

            this.OpponentName = OpponentName;

            this.Status = "Playing";
        }
        public System.Timers.Timer aTimer { get; set; }
        public Stopwatch sw { get; set; }
        public string OpponentName { get; set; }

        public string Status { get; set; }

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

                context.messages.Add(new Messages(this.OpponentName+" won on time LIMIT"));
                context.SaveChanges();

            }

             

        }


    }


}