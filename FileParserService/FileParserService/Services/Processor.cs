using Newtonsoft.Json;
using System.Xml.Linq;
using FileParserService.Services.Interfaces;

namespace FileParserService.Services
{
    public class Processor : IProcessor
    {
        public void ProcessFile(string filePath)
        {
            string queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE") ?? "xmlToJsonQueue"; 
            IFileMessageProducer producer = new FIleMessageProducer();

            try
            {
                XDocument doc = XDocument.Load(filePath);
                var rapidControlStatusElements = doc.Descendants("RapidControlStatus");

                foreach (var rapidControlStatusElement in rapidControlStatusElements)
                {
                    var combinedStatusXml = rapidControlStatusElement.Value;
                    XDocument combinedStatusDoc = XDocument.Parse(combinedStatusXml);
                    var moduleStateElement = combinedStatusDoc.Descendants("ModuleState").FirstOrDefault();

                    if (moduleStateElement != null)
                    {
                        Random localRandom = new Random();
                        int randomNumber = localRandom.Next(4);

                        switch (randomNumber)
                        {
                            case 0:
                                moduleStateElement.Value = "Online";
                                break;
                            case 1:
                                moduleStateElement.Value = "Run";
                                break;
                            case 2:
                                moduleStateElement.Value = "NotReady";
                                break;
                            case 3:
                                moduleStateElement.Value = "Offline";
                                break;
                        }

                        rapidControlStatusElement.Value = combinedStatusDoc.ToString();
                    }
                }

                doc.Save(filePath);
                Console.WriteLine($"File processed successfully: {filePath}");
                string json = ConvertXmlToJson(filePath);
                producer.SendMessage(json, queueName);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
            }

        }

        static string ConvertXmlToJson(string xmlFilePath)
        {
            try
            {
                XDocument doc = XDocument.Load(xmlFilePath);
                return JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting XML to JSON for file {xmlFilePath}: {ex.Message}");
                return null;
            }
        }
    }
}