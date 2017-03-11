using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using SampleChat.Models;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
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
    }
}