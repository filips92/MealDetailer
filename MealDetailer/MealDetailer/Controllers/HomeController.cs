using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealDetailer.Lib;

namespace MealDetailer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //XmlHelper xmlHelper = new XmlHelper();
            //xmlHelper.ValidateXml(Server.MapPath("~/Resources/booksSchemaFail.xml"), Server.MapPath("~/Resources/books.xsd"));
            //string xml = GetXml();
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

        public string GetXml(string xmlUri)
        {
            XmlHelper xmlHelper = new XmlHelper();
            string response = xmlHelper.ValidateXml(Server.MapPath(xmlUri), Server.MapPath("~/Resources/books.xsd"));

            return response;
        }
    }
}