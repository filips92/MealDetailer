using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        private string SearchUrlBase =
            "http://api.nal.usda.gov/ndb/search/?format=xml&q={0}&max=25&offset=0&api_key=DEMO_KEY";
        private string ReportFilePathBase = "~/App_Data/{0}.xml";
        private string ReportXSDFilePath = "~/Resources/foodReport.xsd";
        private string ReportXSDNamespace = "urn:foodreport-schema";
        private string SearchFilePathBase = "~/App_Data/search_{0}.xml";
        private string SearchXSDFilePath = "~/Resources/search.xsd";
        private string SearchXSDNamespace = "urn:search-schema";

        public ApiClient()
        {

        }

        public XmlHelper.ValidationResult GetFoodReport(string id, HttpContext httpContext)
        {
            using (WebClient client = new WebClient())
            {
                string tempFilePath = httpContext.Server.MapPath(String.Format(ReportFilePathBase, id));
                string url = String.Format(FoodReportUrlBase, id);
                string rawXmlResponse = client.DownloadString(url);
                File.WriteAllText(tempFilePath, rawXmlResponse);
                XmlHelper xmlHelper = new XmlHelper();
                XmlHelper.ValidationResult response = xmlHelper.ValidateXml(
                    tempFilePath,
                    httpContext.Server.MapPath(ReportXSDFilePath),
                    ReportXSDNamespace
                );
                return response;
            }
        }

        public XmlHelper.ValidationResult Search(string query, HttpContext httpContext)
        {
            using (WebClient client = new WebClient())
            {
                string tempFilePath = httpContext.Server.MapPath(String.Format(SearchFilePathBase, MakeValidFileName(query)));
                string url = String.Format(SearchUrlBase, query);
                string rawXmlResponse = client.DownloadString(url);
                File.WriteAllText(tempFilePath, rawXmlResponse);
                XmlHelper xmlHelper = new XmlHelper();
                XmlHelper.ValidationResult response = xmlHelper.ValidateXml(
                    tempFilePath,
                    httpContext.Server.MapPath(SearchXSDFilePath),
                    SearchXSDNamespace
                );
                return response;
            }
        }

        private string MakeValidFileName(string name)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            string escapedInvalidChars = Regex.Escape(name);
            string invalidRegex = string.Format(@"([{0}]*\.+$)|([{0}]+)", escapedInvalidChars);

            return Regex.Replace(name, invalidRegex, "_");
        }
    }
}