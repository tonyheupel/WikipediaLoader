using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace WikipediaLoader
{
    class Program
    {

        private static long PageCount = 0;
        private static long CurrentBucket = 0;
        private static Stopwatch EntireProcessStopwatch = new Stopwatch();
        private static string PathToCurrentBucket = String.Empty;

        private const int PAGES_PER_BUCKET = 10000;
        private const string PATH_TO_BUCKET_FORMAT = "C:\\Users\\theupel\\Projects\\pages\\bucket_{0}";

        static void Main(string[] args)
        {
            StreamProcesing(@"C:\Users\theupel\Downloads\wikipedia\enwiki-20140502-pages-articles-multistream.xml");
        }

        static void StreamProcesing(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (BufferedStream stream = new BufferedStream(file))
                {
                    EntireProcessStopwatch.Start();

                    string pathToCurrentBucket = String.Format(PATH_TO_BUCKET_FORMAT, CurrentBucket);

                    var pageProcessor = new PageXmlToJsonProcessor();
                    pageProcessor.PageProcessed += Program.pageProcessor_PageProcessed;
                    var processor = new WikipediaXmlProcessor(stream, pageProcessor);
                    processor.ProcessPages();

                    EntireProcessStopwatch.Stop();
                    Console.WriteLine("Entire file time elapsed: {0}", EntireProcessStopwatch.Elapsed);
                }
            }
        }

        static void pageProcessor_PageProcessed(PageProcessedEventArgs pageProcessedEventArgs)
        {
            PageCount += 1;
            AdjustCurrentBucket();
            var pageAsJsonString = pageProcessedEventArgs.Result.ToString();
            dynamic pageId = WriteJsonToPageFileInBucketAndReturnPageId(pageAsJsonString);
            
            //Console.WriteLine("Time: {0}, Page #{1} - id: {2}", EntireProcessStopwatch.Elapsed, PageCount, pageId);
        }

        private static long WriteJsonToPageFileInBucketAndReturnPageId(string pageAsJsonString)
        {
            dynamic jsonPage = JsonConvert.DeserializeObject(pageAsJsonString);

            var pageFilePath = String.Format("{0}\\page_{1}.json", PathToCurrentBucket, jsonPage.page.id);

            if (!File.Exists(pageFilePath))
            {
                using (FileStream output = File.OpenWrite(pageFilePath))
                {
                    output.Write(Encoding.UTF8.GetBytes(pageAsJsonString), 0, Encoding.UTF8.GetByteCount(pageAsJsonString));
                }
            }
            else
            {
                //Console.WriteLine("File aready exists!  Skipping: {0}", pageFilePath);
            }

            return jsonPage.page.id;
        }

        static void AdjustCurrentBucket()
        {
            if ((PageCount % (PAGES_PER_BUCKET + 1) == 0) || CurrentBucket == 0)
            {
                CurrentBucket += 1;
                PathToCurrentBucket = String.Format(PATH_TO_BUCKET_FORMAT, CurrentBucket);
                if (!Directory.Exists(PathToCurrentBucket))
                {
                    Directory.CreateDirectory(PathToCurrentBucket);
                }

                Console.WriteLine("Time: {0}, Bucket #{1}", EntireProcessStopwatch.Elapsed, CurrentBucket);
            }
        }
    }
}
