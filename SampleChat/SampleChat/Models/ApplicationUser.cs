using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SampleChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Country Country { get; set; }

        public decimal CurrentRating { get; set; }

        public virtual ICollection<Ratings> Ratings { get; set; }

      
        public virtual  ICollection<ChatMessages> SenderMessage { get; set; }

        public virtual  ICollection<ChatMessages> ReceiverMessage { get; set; }

        public ICollection<Games> GamesWithBlackColor { get; set; }

        public ICollection<Games> GamesWithWhiteColor { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


    }
}