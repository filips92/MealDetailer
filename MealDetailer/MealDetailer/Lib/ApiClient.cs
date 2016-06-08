using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MealDetailer.Lib
{
    public class ApiClient
    {
        private string FoodReportUrlBase =
            "http://api.nal.usda.gov/ndb/reports/?ndbno={0}&type=b&format=xml&api_key=DEMO_KEY";
        private string TempFilePathBase = "~/App_Data/{0}.xml";
        private string XSDFilePath = "~/Resources/foodReport.xsd";
        private string XSDNamespace = "urn:foodreport-schema";

        public ApiClient()
        {
            
        }

        public XmlHelper.ValidationResult GetFoodReport(string id, HttpContext httpContext)
        {
            using (WebClient client = new WebClient())
            {
                string tempFilePath = httpContext.Server.MapPath(String.Format(TempFilePathBase, id));
                string url = String.Format(FoodReportUrlBase, id);
                string rawXmlResponse = client.DownloadString(url);
                File.WriteAllText(tempFilePath, rawXmlResponse);
                XmlHelper xmlHelper = new XmlHelper();
                XmlHelper.ValidationResult response = xmlHelper.ValidateXml(
                    tempFilePath, 
                    httpContext.Server.MapPath(XSDFilePath), 
                    XSDNamespace
                );
                return response;
            }
        }
    }
}