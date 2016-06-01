using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using MealDetailer.Lib;
using Newtonsoft.Json;

namespace MealDetailer.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(string id)
        {
            XmlHelper.ValidationResult validationResult = GetXml("~/Resources/booksSchemaFail.xml", "~/Resources/books.xsd", "urn:bookstore-schema");
            return View();
        }

        public ActionResult GetFoodReport()
        {
            /*var id = "01009";
            var api = new ApiClient();
            var report = api.GetFoodReport(id);*/
            var report = GetXml("~/Resources/foodReport.xml", "~/Resources/foodReport.xsd", "urn:foodreport-schema");
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveReport(string contents)
        {
            XmlDocument reportXml = JsonConvert.DeserializeXmlNode(contents);
            // do validation

            return Json("ok");
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

        public XmlHelper.ValidationResult GetXml(string xmlUri, string xsdUri, string xNamespace)
        {
            XmlHelper xmlHelper = new XmlHelper();
            XmlHelper.ValidationResult response = xmlHelper.ValidateXml(Server.MapPath(xmlUri), Server.MapPath(xsdUri), xNamespace);

            return response;
        }
    }
}