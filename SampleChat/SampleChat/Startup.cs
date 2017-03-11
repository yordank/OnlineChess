using Microsoft.Owin;
using Owin;
using SampleChat.Migrations;
using SampleChat.Models;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(SampleChat.Startup))]
namespace SampleChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChatDbContext, Configuration>());

            app.MapSignalR();
            ConfigureAuth(app);
            
        }
    }
}
