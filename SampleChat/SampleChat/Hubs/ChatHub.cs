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
using SampleChat.Hubs;
using System.Data.Entity;

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
            return this.UserName == other.UserName && this.userId == other.userId;
        }
        public override int GetHashCode()
        {
            return (UserName + userId).GetHashCode();
        }
    }
    public class ChatHub : Hub
    {
        public static HashSet<UserConnection> usersWhoSeekOpponent = new HashSet<UserConnection>();

        public static Dictionary<string, Game> GamesOnline = new Dictionary<string, Game>();

        public void chatsend(string name, string message)
        {
            string userId = GamesOnline[name].OpponentName;

            Clients.User(userId).addNewMessageToPage(name, message, 0);

            Clients.User(name).addNewMessageToPage(name, message, 0);

        }
        public void Send(string name, string moveString, string status)
        {

            using (var context = new ChatDbContext())
            {

                context.messages.Add(new Messages(moveString));
                context.SaveChanges();

            }



            if (status == "Game over, White is in checkmate.")
            {
                using (var context = new ChatDbContext())
                {

                    Results res = new Results() { WhiteUserName = GamesOnline[name].OpponentName, BlackUserName = name, Result = "B" };
                    context.results.Add(res);

                    context.Entry(res).State = EntityState.Added;



                    context.SaveChanges();

                    
                }
            }

            if (status == "Game over, Black is in checkmate.")
            {
                using (var context = new ChatDbContext())
                {

                    context.results.Add(new Results() { WhiteUserName = name, BlackUserName = GamesOnline[name].OpponentName, Result = "W" });

                   

                    context.SaveChanges();

                   
                }
            }

            if (status == "Game over, drawn position.")
            {
                using (var context = new ChatDbContext())
                {
                    if (GamesOnline[name].Color == "white")
                    {
                        context.results.Add(new Results() { WhiteUserName = name, BlackUserName = GamesOnline[name].OpponentName, Result = "D" });
                        context.SaveChanges();
                    }
                    else
                    {
                        context.results.Add(new Results() { WhiteUserName = GamesOnline[name].OpponentName, BlackUserName = name, Result = "D" });
                        context.SaveChanges();
                    }
                    
                }
            }


            string userId = GamesOnline[name].OpponentName;


            if (GamesOnline[name].Status == "Lost on Time")
            {


                Clients.User(userId).addNewMessageToPage(name, $"{name} lost on time!", 1);

                Clients.User(name).addNewMessageToPage(name, $"{name} lost on time!", 1);

                return;
            }



            GamesOnline[name].stop();


            var Move = moveString;
            //var Move = move;
            var SendMessage = new
            {
                ServiceName = "ChessGameMove",

                Data = Move,

                TimeOpponet = GamesOnline[name].aTimer.Interval / 1000,

                TimeUser = GamesOnline[userId].aTimer.Interval / 1000

            };

            GamesOnline[userId].start();

            var jsonChessGameMove = JsonConvert.SerializeObject(SendMessage);

            Clients.User(userId).getOpponentMove(jsonChessGameMove);


          

        }




        public void Seek(string name, int time)
        {

            var us = new UserConnection();
            us.UserName = name;
            us.userId = Context.User.Identity.Name;
            us.Time = time;



            usersWhoSeekOpponent.Add(us);


            Seekall();

        }
        public void Startgame(string username1, string username2, int time)
        {



            var user1 = usersWhoSeekOpponent.Where(x => x.UserName == username1).FirstOrDefault();
            var user2 = usersWhoSeekOpponent.Where(x => x.UserName == username2).FirstOrDefault();

            Clients.User(user1.userId).beginGame("white", time * 60);
            Clients.User(Context.User.Identity.Name).beginGame("black", time * 60);


            GamesOnline[username1] = new Game(username1, Context.User.Identity.Name, 1000 * 60 * time, "white");
            GamesOnline[username2] = new Game(username2, user1.userId, 1000 * 60 * time, "black");

            usersWhoSeekOpponent.Remove(user1);
            usersWhoSeekOpponent.Remove(user2);

            Seekall();
        }

        public void Seekall()
        {

            var Users = usersWhoSeekOpponent.Select(x => new { Username = x.UserName, Time = x.Time });

            var responceMessage = new
            {
                ServiceName = "AllUsersWhoSeekOpponet",
                Data = Users
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
}


 