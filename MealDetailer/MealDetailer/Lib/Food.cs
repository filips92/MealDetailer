using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MealDetailer.Lib
{

    public class FullFoodReport
    {
        public Report Report { get; set; }
    }

    [XmlRoot(ElementName="report", Namespace="")]
    public class Report
    {
        public Object Footnotes { get; set; }
        public Food Food { get; set; }
    }

    [XmlRoot("food")]
    public class Food
    {
        [XmlArray("nutrients"), XmlArrayItem(ElementName = "nutrient", Type = typeof(Nutrient))]
        public List<Nutrient> Nutrients { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }

        public Food()
        {
            this.Nutrients = new List<Nutrient>();
        }

        [XmlRoot("nutrient")]
        public class Nutrient
        {
            [XmlAttribute("name")]
            public string Name { get; set; }
            [XmlArray("measures"), XmlArrayItem(ElementName = "measure", Type = typeof(Measure))]
            public List<Measure> Measures { get; set; }

            public Nutrient()
            {
                this.Measures = new List<Measure>();
            }
        }

        [XmlRoot("measure")]
        public class Measure
        {
            public Measure()
            {

            }
            public Measure(XmlNode singleMeasureNode)
            {
                this.Label = singleMeasureNode.Attributes["label"].Value;
                this.Equivalent = float.Parse(singleMeasureNode.Attributes["eqv"].Value);
                this.Value = float.Parse(singleMeasureNode.Attributes["value"].Value);
            }
            [XmlAttribute("label")]
            public string Label { get; set; }
            [XmlAttribute("eqv")]
            public float Equivalent { get; set; }
            [XmlAttribute("value")]            
            public float Value { get; set; }
        }

    }
}