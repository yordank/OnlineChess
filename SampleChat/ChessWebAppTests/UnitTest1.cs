using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleChat.Controllers;
using System.Collections.Generic;
using SampleChat.Models;
using System.Linq;

namespace ChessWebAppTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var controller = new HomeController();
            var result = controller.ListGames() as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(List<Results>));
        }
        [TestMethod]
        public void TestMethod2()
        {

            var controller = new HomeController();
            var result = controller.ListGames() as ViewResult;

            var context = new ChatDbContext();

            var res = (List<Results>)result.Model;

            Assert.AreEqual(res.Count(), context.results.Count());
        }


    }
}
