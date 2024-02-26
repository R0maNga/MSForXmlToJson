using System.Text.RegularExpressions;
using DataProcessorService.Interfaces;

namespace DataProcessorService.Services
{
    public class Finder : IFinder
    {
        public List<string> FindAllData(string message, string dataToFind)
        {
            List<string> dataList = new List<string>();
            int startIndex = 0;

            while (true)
            {
                startIndex = message.IndexOf("<" + dataToFind + ">", startIndex);
                
                if (startIndex == -1)
                    break;

                startIndex += ("<" + dataToFind + ">").Length;
                int endIndex = message.IndexOf("</" + dataToFind + ">", startIndex);
                
                if (endIndex == -1)
                    break;
               
                string data = message.Substring(startIndex, endIndex - startIndex);
                dataList.Add(data);
                startIndex = endIndex;
            }
            return dataList;
        }


        public List<string> FindAllModuleCategoryIds(string message)
        {
            string unescapedJson = Regex.Unescape(message);
            List<string> categoryList = new List<string>();
            int startIndex = 0;

            while (true)
            {
                startIndex = unescapedJson.IndexOf("ModuleCategoryID\": \"", startIndex);

                if (startIndex == -1)
                    break;

                startIndex += "ModuleCategoryID\": \"".Length;
                int endIndex = unescapedJson.IndexOf("\"", startIndex);
                string moduleCategoryID = unescapedJson.Substring(startIndex, endIndex - startIndex);
                moduleCategoryID = moduleCategoryID.Replace("\\r", "").Replace("\\n", "").Replace(" ", "");
                categoryList.Add(moduleCategoryID);
                startIndex = endIndex;
            }

            return categoryList;
        }

    }
}
