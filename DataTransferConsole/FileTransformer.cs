using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTransferConsole
{
    public class FileTransformer
    {
        public static void Tranform(string dataFilePath, string dataColumnFilePath)
        {
            try
            {
                var targetSeperator = ',';

                if (string.IsNullOrWhiteSpace(dataFilePath))
                {
                    Console.WriteLine("Please provide the data file");

                    return;
                }

                if (string.IsNullOrWhiteSpace(dataColumnFilePath))
                {
                    Console.WriteLine("Please Select the columns file");

                    return;
                }

                var columnSeperator = '~';
                var rowSeperator = 'ý';

                var columnNames = File.ReadAllText(dataColumnFilePath, Encoding.UTF8).Trim().Split(targetSeperator);


                var lines = //new string[] { "Mr. X~BDTýUSDýGBPýEUR~1000ý150ý25ý~20150101ý20150215ý20160310ý20160415~10" };

                 File.ReadAllLines(dataFilePath, Encoding.UTF7);


                var dataFilePathPrefix = dataFilePath.Substring(0, dataFilePath.LastIndexOf('.'));

                var csvDataFilePath = dataFilePathPrefix + ".csv";

                if (File.Exists(csvDataFilePath))
                {
                    File.Delete(csvDataFilePath);
                }

                var columns = string.Join(",", columnNames.Select(s => s.Trim()));

                File.AppendAllLines(csvDataFilePath, new string[] { columns });

                var stringBuilder = new StringBuilder();

                var rowIndex = -1;

                foreach (var line in lines)
                {
                    if (rowIndex == 1) break;

                    var columnsDataList = line.Split(columnSeperator);
                    var lineData = string.Empty;

                    rowIndex = rowIndex + 1;

                    Console.WriteLine("Processing line #" + (rowIndex + 1));

                    var maxRecordToBe = columnsDataList.Select(s => s.Count(x => x == rowSeperator)).Max();
                    //string[,] rows = new string[maxRecordToBe, columnNames.Length];
                    stringBuilder.Clear();

                    for (var i = 0; i <= maxRecordToBe; i++)
                    {
                        lineData = string.Empty;

                        var columnIndex = -1;

                        foreach (var columnData in columnsDataList)
                        {
                            columnIndex = columnIndex + 1;

                            if (columnData.IndexOf(rowSeperator) == -1)
                            {
                                lineData = string.Format("{0}{1}{2}", lineData, columnIndex > 0 ? "," : "", columnData);
                                //rows[rowIndex + i, columnIndex] = columnData;
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

                                //rows[rowIndex + i, columnIndex] = columnArray[columnIndex];
                            }

                            //rows[index] = lineData;
                        }

                        stringBuilder.AppendLine(lineData);

                        //Aappend one line at a time to avoide out of memory exception
                        //File.AppendAllLines(csvDataFilePath, new string[] { lineData });
                    }


                    //Aappend one line at a time to avoide out of memory exception
                    //File.AppendAllLines(csvDataFilePath, new string[] { stringBuilder.ToString() });
                    File.AppendAllText(csvDataFilePath, stringBuilder.ToString());
                }

                //This take huge memory and throw out of memory exception 
                //var csvData = stringBuilder.ToString();              

                //File.WriteAllText(csvDataFilePath, csvData);



                Console.WriteLine("Success : " + "Data transformation operation completed successfully.");
                //MessageBox.Show("CSV data file created: " + csvDataFilePath, "Data transformation operation completed successfully", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured: "+ ex.Message);
                //MessageBox.Show("Error occured: " + ex.Message, "Data transformation operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

