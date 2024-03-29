﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WikipediaLoader
{
    public class WikipediaXmlProcessor
    {
        public WikipediaXmlProcessor(Stream inputStream, IPageProcessor pageProcessor)
        {
            if (inputStream == null) throw new ArgumentNullException("inputStream");
            if (pageProcessor == null) throw new ArgumentNullException("pageProcessor");

            this.InputStream = inputStream;
            this.PageProcessor = pageProcessor;
        }

        public Stream InputStream { get; private set; }
        public IPageProcessor PageProcessor { get; private set; }

        public void ProcessPages()
        {
            XmlTextReader reader = new XmlTextReader(this.InputStream);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "page")
                        {
                            PageProcessor.ProcessPage(reader.ReadOuterXml());
                        }
                        break;

                    default:
                        // Do nothing for now
                        break;
                }
            }
        }
    }
}
