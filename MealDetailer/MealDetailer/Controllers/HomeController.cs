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

        public ActionResult QueryDatabase(string query = "butter")
        {
            ApiClient api = new ApiClient();
            XmlHelper.ValidationResult searchResult = api.Search(query, System.Web.HttpContext.Current);

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveReport(string contents)
        {
            XmlDocument reportXml = JsonConvert.DeserializeXmlNode(contents);
            // do validation

            return Json("ok");
        }
    }
}