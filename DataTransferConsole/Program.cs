using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransformerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var dataFileDirectory = ConfigurationManager.AppSettings["DataFileDirectory"];
                var columnDataFiles = Directory.GetFiles(dataFileDirectory, "*.Columns.txt");

                Parallel.ForEach(columnDataFiles, (file) =>
                {
                    var dataColumnFilePath = file;
                    var dataFilePath = file.Replace(".Columns", "");

                    var watch = Stopwatch.StartNew();

                    CustomMultiThreadingDataTransformer.Tranform(dataFilePath, dataColumnFilePath);

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
 
                    LogWriter.Instance.WriteToLog(string.Format("The data file {0} generated csv in {1} minutes", dataFilePath, (elapsedMs / 1000) / 60));
                });
            }
            catch (Exception ex)
            {
                var eventLog = new EventLog("DataTransformerConsole");

                eventLog.Source = "DataTransformerConsole Error";

                eventLog.WriteEntry(ex.Message);
            }
        }
    }
}
