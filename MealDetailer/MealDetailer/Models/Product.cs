using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealDetailer.Models
{
    public class Product
    {
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public Dictionary<string, int> WeightDictionary { get; set; }

        //Example of weights numbers are the weight equivalents in grams
        //public Product()
        //{
        //    WeightDictionary = new Dictionary<string, int>()
        //    {
        //        {"-", 0},
        //        {"teaspoon", 4},
        //        {"tablespoon", 12},
        //        {"cup", 195},
        //        {"ml", 1},
        //        {"g", 1},
        //        {"l", 1000},
        //        {"kg", 1000}
        //    };
        //}
    }
}