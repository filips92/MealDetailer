using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using MealDetailer.Lib;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace MealDetailer.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(string id)
        {
            return View();
        }

        public ActionResult ValidationDebug()
        {
            XmlHelper.ValidationResult validationResult = new XmlHelper().ValidateXml(Server.MapPath("~/Resources/debug.xml"), Server.MapPath("~/Resources/debug.xsd"), "");
            return Content(validationResult.IsValid.ToString());
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
        public ActionResult SaveReport(string name, string contents)
        {
            String filepathBase = "~/App_Data/export_{0}_{1}.xml";
            String sanitizedName = PathSanitizer.MakeValidFileName(name);
            String token = DateTime.Now.Ticks.ToString();
            String formattedPath = String.Format(filepathBase, sanitizedName, token);
            string mappedPath = Server.MapPath(formattedPath);
            XmlDocument reportXml = JsonConvert.DeserializeXmlNode(contents);
            System.IO.File.WriteAllText(mappedPath, "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + reportXml.OuterXml);

            XmlHelper.ValidationResult validationResult = new XmlHelper().ValidateXml(mappedPath, Server.MapPath("~/Resources/foodReport.xsd"), "");

            if (validationResult.IsValid)
            {
                validationResult.Contents = String.Format("/Home/PreviewReport?path={0}", formattedPath);
            }

            return Json(validationResult);
        }

        public ActionResult PreviewReport(string path)
        {
            return Content(TransformXMLToHTML(path));
        }

        private string TransformXMLToHTML(string xmlPath)
        {
            String xsltString = System.IO.File.ReadAllText(Server.MapPath("~/Resources/dish.xslt"));
            String inputXml = System.IO.File.ReadAllText(Server.MapPath(xmlPath));

            XslCompiledTransform transform = new XslCompiledTransform();
            using (XmlReader reader = XmlReader.Create(new StringReader(xsltString)))
            {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(inputXml)))
            {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }
    }
}