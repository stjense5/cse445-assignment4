using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;


// ASU CSE445 Assignment 4
namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "PASTE YOUR XML URL HERE";
        public static string xmlErrorURL = "PASTE YOUR ERROR XML URL HERE";
        public static string xsdURL = "PASTE YOUR XSD URL HERE";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                string xmlContent = DownloadContent(xmlUrl);
                string xsdContent = DownloadContent(xsdUrl);

                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(xsdContent)));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);

                doc.Schemas = schemas;

                string validationMessage = "No Error";

                doc.Validate((sender, e) =>
                {
                    validationMessage = e.Message;
                });

                return validationMessage;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                string xmlContent = DownloadContent(xmlUrl);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);

                string jsonText = JsonConvert.SerializeXmlNode(doc);

                return jsonText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Helper method
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}