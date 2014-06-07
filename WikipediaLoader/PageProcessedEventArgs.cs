using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaLoader
{
    public class PageProcessedEventArgs : PageProcessedEventArgs<object>
    {
        public PageProcessedEventArgs(object result) : base(result)
        {
        }
    }
    public class PageProcessedEventArgs<T> : EventArgs
    {
        public PageProcessedEventArgs(T result)
        {
            this.Result = result;
        }

        public T Result { get; private set; }
    }
}
