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
            return View();
        }

        public ActionResult GetFoodReport(string id = "01009")
        {
            ApiClient api = new ApiClient();
            XmlHelper.ValidationResult report = api.GetFoodReport(id, System.Web.HttpContext.Current);

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