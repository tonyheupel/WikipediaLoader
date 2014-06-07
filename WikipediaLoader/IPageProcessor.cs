using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaLoader
{
    public delegate void PageProcessedEventHandler(PageProcessedEventArgs pageProcessedEventArgs);
    public interface IPageProcessor
    {
        void ProcessPage(string page);
        event PageProcessedEventHandler PageProcessed;
    }
}
