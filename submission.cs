using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://stjense5.github.io/cse445-assignment4/NationalParks.xml";
        public static string xmlErrorURL = "https://stjense5.github.io/cse445-assignment4/NationalParksErrors.xml";
        public static string xsdURL = "https://stjense5.github.io/cse445-assignment4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string errorMessages = "";
            try
            {
                string xmlContent = DownloadContent(xmlUrl);

                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = schemas
                };

                settings.ValidationEventHandler += (sender, e) =>
                {
                    errorMessages += e.Message + "\n";
                };

                using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent), settings))
                {
                    while (reader.Read()) { }
                }

                if (string.IsNullOrEmpty(errorMessages.Trim()))
                    return "No errors are found";
                else
                    return errorMessages.Trim();
            }
            catch (XmlException ex)
            {
                return "XML Structure Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            string xmlContent = DownloadContent(xmlUrl);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            return JsonConvert.SerializeXmlNode(doc);
        }

        private static string DownloadContent(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}