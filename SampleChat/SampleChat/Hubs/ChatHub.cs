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



    public class ChatHub : Hub
    {
        public static HashSet<UserConnection> usersWhoSeekOpponent = new HashSet<UserConnection>();

        public static Dictionary<string, Game> GamesOnline = new Dictionary<string, Game>();

        public void chatsend(string GameName, string message)
        {
            string white = GamesOnline[GameName].White;

            string black = GamesOnline[GameName].Black;

            string sender= Context.User.Identity.Name;

            Clients.User(white).addNewMessageToPage(sender, message, 0);

            Clients.User(black).addNewMessageToPage(sender, message, 0);

        }



        public void Send(string white,string black,string color, string moveString, string status)
        {
            string gameName = white  + black;

            string sender = Context.User.Identity.Name;

            using (var context = new ChatDbContext())
            {

                context.messages.Add(new Messages(moveString));
                context.SaveChanges();

            }

            //test

            if (status == "Game over, White is in checkmate.")
            {
                using (var context = new ChatDbContext())
                {
                    Results res = new Results() { WhiteUserName = white, BlackUserName = black, Result = "B" };
                    context.results.Add(res);
                    context.SaveChanges();
                                       
                }
            }

            if (status == "Game over, Black is in checkmate.")
            {
                using (var context = new ChatDbContext())
                {

                    context.results.Add(new Results() { WhiteUserName = white, BlackUserName = black, Result = "W" });
                    context.SaveChanges();
                                       
                }
            }

            if (status == "Game over, drawn position.")
            {
                using (var context = new ChatDbContext())
                {
                     
                        context.results.Add(new Results() { WhiteUserName = white, BlackUserName = black, Result = "D" });
                        context.SaveChanges();
             
                    
                }
            }


           


            if (GamesOnline[gameName].Status != "Playing")
            {
             
                Clients.User(white).addNewMessageToPage(sender, $"{GamesOnline[gameName].Status}", 1);

                Clients.User(black).addNewMessageToPage(sender, $"{GamesOnline[gameName].Status}", 1);

                return;
            }



            GamesOnline[gameName].stopClock(color);


            var Move = moveString;
            //var Move = move;
            var SendMessage = new
            {
                ServiceName = "ChessGameMove",

                Data = Move,

               // TimeOpponet = GamesOnline[name].aTimer.Interval / 1000,

               // TimeUser = GamesOnline[userId].aTimer.Interval / 1000

            };

            if(color=="white")
            GamesOnline[gameName].startClock("black");
            if (color == "black")
            GamesOnline[gameName].startClock("white");




            var jsonChessGameMove = JsonConvert.SerializeObject(SendMessage);

            if(sender==white)
            Clients.User(black).getOpponentMove(jsonChessGameMove);

            if (sender == black)
            Clients.User(white).getOpponentMove(jsonChessGameMove);



        }




        public void Seek(string name, int time)
        {

            var us = new UserConnection();
            us.userId = name;
            us.Time = time;



            usersWhoSeekOpponent.Add(us);


            Seekall();

        }
        public void Startgame(string username1, string username2, int time)
        {

            {
                GamesOnline[username1 + username2] = new Game(username1, username2, 1000 * 60 * time);

                Clients.User(username1).beginGame("white", time * 60, username1, username2);
                Clients.User(username2).beginGame("black", time * 60, username1, username2);
            }

            var user1 = usersWhoSeekOpponent.Where(x => x.userId == username1).FirstOrDefault();

            var user2 = usersWhoSeekOpponent.Where(x => x.userId == username2).FirstOrDefault();

            usersWhoSeekOpponent.Remove(user1);

            usersWhoSeekOpponent.Remove(user2);

          

            int a = 5;

            Seekall();


        }

        public void Seekall()
        {

            var Users = usersWhoSeekOpponent.Select(x => new { Username = x.userId, Time = x.Time });

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
            var user = usersWhoSeekOpponent.Where(x => x.userId == Context.User.Identity.Name).FirstOrDefault();
            usersWhoSeekOpponent.Remove(user);
            Seekall();
            return base.OnDisconnected(stopCalled);
        }


    }
}


 