using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SampleChat.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    

    public class ChatDbContext : IdentityDbContext<ApplicationUser>
    {
        public ChatDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
           Database.SetInitializer<ChatDbContext>(new DropCreateDatabaseIfModelChanges<ChatDbContext>());
        }



        public virtual IDbSet<Messages>messages{get;set;}

        public virtual IDbSet<Results> results { get; set; }

        public virtual IDbSet<Ratings> ratings { get; set; }

        public virtual IDbSet<Country> Countries { get; set; }

        public virtual IDbSet<Move> Moves { get; set; }

        public virtual IDbSet<Games> Games { get; set; }

        public virtual IDbSet<GamePlayer> GamesPlayers { get; set; }

        public virtual IDbSet<ChatMessages> ChatMessages { get; set; }



        public static ChatDbContext Create()
        {
            return new ChatDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


           

           

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.SenderMessage)
                .WithRequired(x => x.Sender);


            base.OnModelCreating(modelBuilder);
        }
        
    }
}