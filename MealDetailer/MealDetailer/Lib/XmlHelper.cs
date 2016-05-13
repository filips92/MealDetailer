using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace MealDetailer.Lib
{
    public class XmlHelper
    {
        public void ValidateXml(string documentToValidateUri, string validationTemplateUri)
        {
            // Create the XmlSchemaSet class.
            XmlSchemaSet sc = new XmlSchemaSet();

            // Add the schema to the collection.
            sc.Add("urn:bookstore-schema", validationTemplateUri);

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler (ValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(documentToValidateUri, settings);

            // Parse the file. 
            while (reader.Read());
        }
        // Display any validation errors.
        private static void ValidationCallBack(object sender, ValidationEventArgs e) {
            Console.WriteLine("Validation Error: {0}", e.Message);
            Debug.WriteLine("Validation Error: {0}", e.Message);
        }
    }

}