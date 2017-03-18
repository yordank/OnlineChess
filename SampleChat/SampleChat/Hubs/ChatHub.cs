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
    }
public class ChatHub : Hub
    {
        public static HashSet<UserConnection> usersWhoSeekOpponent = new HashSet<UserConnection>();

        public static Dictionary<string, string> usersOpponent = new Dictionary<string, string>();

        public void Send(string name, string message)
        {

            string userId = usersOpponent[name];
            // Call the addNewMessageToPage method to update clients.
            //Clients.Client(connectionId).addNewMessageToPage( message);

            Clients.User(userId).addNewMessageToPage(message);
      
            //  Clients.All.addNewMessageToPage(name, message);
            using (var context = new ChatDbContext())
            {

               context.messages.Add(new Messages(message));
               context.SaveChanges();

            }

        }


      

        public void Seek(string name)
        {
            //   var username = Context.User.Identity.Name;
            //  Console.WriteLine(username);
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
            //   var username = Context.User.Identity.Name;
            //  Console.WriteLine(username);
            //string UsersString = JsonConvert.SerializeObject(usersWhoSeekOpponent);
            Clients.All.ResponceAllOpponents(usersWhoSeekOpponent.Select(x=>x.UserName));
        }



    }
}