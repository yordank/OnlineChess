using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return RedirectToAction("Chat");
            return RedirectToAction("Seek");

            return View();
        }
        public ActionResult Chat(string orientation)
        {
            ViewBag.orientation = orientation;

            return View();
        }

        public ActionResult Seek()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}