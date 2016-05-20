using System; 
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransformerConsole
{
    public class CustomMultiThreadingDataTransformer
    {
        private static Char targetSeperator = ',';
        private static Char columnSeperator = '~';
        private static Char rowSeperator = 'ý';
        private static LogWriter logWriter = LogWriter.Instance;

        public static async void Tranform(string dataFilePath, string dataColumnFilePath)
        {
            try
            {
                var dataFilePathPrefix = dataFilePath.Substring(0, dataFilePath.LastIndexOf('.'));
                var csvDataFilePath = dataFilePathPrefix + ".csv";
                //Delete this output CSV file before writing columns and then CSV data if there any older one
                if (File.Exists(csvDataFilePath))
                {
                    File.Delete(csvDataFilePath);
                }

                logWriter.WriteToLog(string.Format("Reading columns from file \"{0}\"", dataColumnFilePath));
                var columnNames = File.ReadAllText(dataColumnFilePath, Encoding.UTF8).Trim().Split(targetSeperator);
                var columns = string.Join(",", columnNames.Select(s => s.Trim()));
                File.AppendAllLines(csvDataFilePath, new string[] { columns });
                
                logWriter.WriteToLog(string.Format("Reading data from file \"{0}\"", dataFilePath));
                var lines = File.ReadAllLines(dataFilePath, Encoding.UTF7);
                logWriter.WriteToLog(string.Format("#{0} lines found in the data file \"{1}\"", lines.Length , dataFilePath));
           
                logWriter.WriteToLog(string.Format("CSV records generation process of the data file \"{0}\" \n starting ", dataFilePath));
                GenerateCSVInCustomParallel(lines, csvDataFilePath);
                logWriter.WriteToLog(string.Format("Success : Data transformation operation of the file \"{0}\" completed successfully.", dataFilePath));
            }
            catch (Exception ex)
            {
                logWriter.WriteToLog(string.Format("Error Occured: {0}", ex.Message));
            }
        }
        
        private static void GenerateCSV(string line, int rowIndex, string dataFileTempPath)
        {
            var stringBuilder = new StringBuilder();
            var columnsDataList = line.Split(columnSeperator);
            var lineData = string.Empty;

            logWriter.WriteToLog(string.Format("Generating CSV records of line #{0}", (rowIndex + 1)));

            var maxRecordsToBe = columnsDataList.Select(s => s.Count(x => x == rowSeperator)).Max();

            for (var i = 0; i <= maxRecordsToBe; i++)
            {
                lineData = string.Empty;

                var columnIndex = -1;

                foreach (var columnData in columnsDataList)
                {
                    columnIndex = columnIndex + 1;

                    if (columnData.IndexOf(rowSeperator) == -1)
                    {
                        lineData = string.Format("{0}{1}{2}", lineData, columnIndex > 0 ? "," : "", columnData);
                    }
                    else
                    {
                        var columnArray = columnData.Split(rowSeperator);
                        var data = string.Empty;

                        if (columnArray.Length > i)
                        {
                            data = columnArray[i];
                        }

                        lineData = string.Format("{0}{1}{2}", lineData, columnIndex > 0 ? "," : "", data);
                    }

                }

                stringBuilder.AppendLine(lineData);

            }

           File.AppendAllText(dataFileTempPath, stringBuilder.ToString());
        }

        private static void GenerateCSVInCustomParallel(string[] lines, string csvDataFilePath)
        {
            //var numberOfLinesPerBatch = 20000;
            //var degreeOfParallelism = lines.Length > numberOfLinesPerBatch ? (lines.Length/ numberOfLinesPerBatch) + 1 : Environment.ProcessorCount;
            var degreeOfParallelism = Environment.ProcessorCount;
            var tasks = new Task[degreeOfParallelism];

            for (int taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
            {
                // capturing taskNumber in lambda wouldn't work correctly
                int taskNumberCopy = taskNumber;

                var dataFileTempPath = csvDataFilePath.Replace(".", (taskNumber + "."));

                //delete if there is any older one
                if (File.Exists(dataFileTempPath))
                {
                    File.Delete(dataFileTempPath);
                }

                tasks[taskNumber] = Task.Factory.StartNew(
                    () =>
                    {
                        var max = lines.Length * (taskNumberCopy + 1) / degreeOfParallelism;
                        var start = lines.Length * taskNumberCopy / degreeOfParallelism;

                        for (int i = start; i < max; i++)
                        {
                            GenerateCSV(lines[i], i, dataFileTempPath);
                        }
                    });
            }

            Task.WaitAll(tasks);

            //Comment this line if no need to merge temp data files
            MergeTemDataFilesIntoOne(csvDataFilePath, degreeOfParallelism);
        }

        private static void MergeTemDataFilesIntoOne(string csvDataFilePath, int degreeOfParallelism)
        {
            //Combile all thread geberated temp files into target csv file and delete temp files
            using (var outputStream = File.Open(csvDataFilePath, FileMode.Append))
            {
                for (int taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
                {
                    var dataFileTempPath = csvDataFilePath.Replace(".", (taskNumber + "."));

                    using (var inputStream = File.OpenRead(dataFileTempPath))
                    {
                        // Buffer size can be passed as the second argument.
                        inputStream.CopyTo(outputStream);
                    }

                    logWriter.WriteToLog(string.Format("The file {0} has been copied to {1}.", dataFileTempPath, csvDataFilePath));

                    //delete to clean directory
                    File.Delete(dataFileTempPath);
                }
            }
        }
    }
}
