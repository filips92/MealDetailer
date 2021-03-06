﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace MealDetailer.Lib
{
    public class XmlHelper
    {
        public string Report { get; set; }
        private List<string> Errors { get; set; }
        private bool IsValidationSuccessful { get; set; }

        public XmlHelper()
        {
            Errors = new List<string>();
        }

        public ValidationResult ValidateXml(string documentToValidateUri, string validationTemplateUri, string validationSchemaNamespace)
        {
            IsValidationSuccessful = true;
            // Create the XmlSchemaSet class.
            XmlSchemaSet sc = new XmlSchemaSet();

            // Add the schema to the collection.
            sc.Add(validationSchemaNamespace, validationTemplateUri);

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler (ValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(documentToValidateUri, settings);
            
            // Parse the file.             
            while (reader.Read());

            if (IsValidationSuccessful == true)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(documentToValidateUri);
                Report = JsonConvert.SerializeXmlNode(doc);
            }

            return new ValidationResult()
            {
                IsValid = IsValidationSuccessful,
                Contents = Report,
                Errors = Errors
            };
        }
        // Display any validation errors.
        private void ValidationCallBack(object sender, ValidationEventArgs e) {
            this.Errors.Add(e.Message);
            IsValidationSuccessful = false;
        }

        public class ValidationResult
        {
            public List<String> Errors { get; set; } 
            public string Contents { get; set; }
            public bool IsValid { get; set; }
        }
    }

}