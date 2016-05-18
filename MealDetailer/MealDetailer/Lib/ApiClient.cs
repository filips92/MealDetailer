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
        public ApiClient()
        {
            
        }

        public FullFoodReport GetFoodReport(string id)
        {
            using (WebClient client = new WebClient())
            {
                string url = String.Format(FoodReportUrlBase, id);
                string rawXmlResponse = client.DownloadString(url);
                XmlSerializer serializer = new XmlSerializer(typeof(FullFoodReport));
                FullFoodReport response = (FullFoodReport)serializer.Deserialize(new StringReader(rawXmlResponse));

                return response;
            }
        }
    }
}