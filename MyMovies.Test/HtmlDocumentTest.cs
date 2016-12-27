using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMovies.Test
{
    [TestClass]
    public class HtmlDocumentTest
    {
        [TestMethod]
        public void GetPageContent()
        {
            var web = new HtmlWeb();

            var doc = web.Load("http://www.google.com");
        }
    }
}
