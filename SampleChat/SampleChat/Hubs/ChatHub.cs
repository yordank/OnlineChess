using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using SampleChat.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace SignalRChat
{


public class UserConnection
    {
      public string UserName { set; get; }
      public string userId { set; get; }

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

        public static Dictionary<string, string> usersOpponent = new Dictionary<string, string>();

        public void Send(string name, string moveString)
        {

            string userId = usersOpponent[name];


            var Move = moveString;

            var SendMessage = new
            {
                ServiceName = "ChessGameMove",
                Data = Move
            };



            var jsonChessGameMove = JsonConvert.SerializeObject(SendMessage);



            Clients.User(userId).getOpponentMove(jsonChessGameMove);
      
           
            using (var context = new ChatDbContext())
            {

               context.messages.Add(new Messages(moveString));
               context.SaveChanges();

            }

        }


      

        public void Seek(string name)
        {
            
            var us = new UserConnection();
            us.UserName = name;
            us.userId = Context.User.Identity.Name;




            usersWhoSeekOpponent.Add(us);

            
            Seekall();
            
        }
        public void Startgame(string username1,string username2)
        {



            var user1 = usersWhoSeekOpponent.Where(x => x.UserName == username1).FirstOrDefault();
            var user2 = usersWhoSeekOpponent.Where(x => x.UserName == username2).FirstOrDefault();

            Clients.User(user1.userId).beginGame("white");
            Clients.User(Context.User.Identity.Name).beginGame("black");

            usersOpponent[username1]= Context.User.Identity.Name;
            usersOpponent[username2]= user1.userId;

            usersWhoSeekOpponent.Remove(user1);
            usersWhoSeekOpponent.Remove(user2);

            Seekall();
        }

        public void Seekall()
        {

            var Users = usersWhoSeekOpponent.Select(x => x.UserName);

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
}