using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WikipediaLoader
{
    class PageXmlToJsonProcessor : IPageProcessor
    {
        public event PageProcessedEventHandler PageProcessed;
        public void ProcessPage(string page)
        {
            string pageAsJsonString = ConvertPageXmlStringToJsonString(page);

            if (PageProcessed != null) PageProcessed(new PageProcessedEventArgs(pageAsJsonString));
        }

        private string ConvertPageXmlStringToJsonString(string pageAsXml)
        {
            var pageDoc = new XmlDocument();
            pageDoc.LoadXml(pageAsXml);

            return JsonConvert.SerializeXmlNode(pageDoc.DocumentElement, Newtonsoft.Json.Formatting.Indented);
        }


        
    }
}
