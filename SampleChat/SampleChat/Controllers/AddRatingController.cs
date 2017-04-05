using SampleChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleChat.Controllers
{
    public class AddRatingController : Controller
    {
        // GET: AddRating
        public ActionResult Index()
        {
            //var context = new ChatDbContext();

            //context.ratings.Add(new Ratings() { date = DateTime.Now, Value = 1990 });

            //context.SaveChanges();

            return View();
        }
    }
}