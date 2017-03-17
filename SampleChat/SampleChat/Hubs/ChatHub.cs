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
      public string ConnectionID { set; get; }
    }
public class ChatHub : Hub
    {
        public static HashSet<UserConnection> usersWhoSeekOpponent = new HashSet<UserConnection>();



        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);

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
            us.ConnectionID = Context.ConnectionId;



            usersWhoSeekOpponent.Add(us);

            
            Seekall();
            
        }
        public void Startgame(string username1,string username2)
        {



            var user1 = usersWhoSeekOpponent.Where(x => x.UserName == username1).FirstOrDefault();
            var user2 = usersWhoSeekOpponent.Where(x => x.UserName == username2).FirstOrDefault();

            Clients.Client(user1.ConnectionID).beginGame("white");
            Clients.Client(Context.ConnectionId).beginGame("black");

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