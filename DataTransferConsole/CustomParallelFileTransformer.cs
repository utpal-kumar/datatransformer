using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTransferConsole
{
    public class CustomParallelFileTransformer
    {
        public static async void Tranform(string dataFilePath, string dataColumnFilePath)
        {
            try
            {
                var targetSeperator = ',';
                var columnSeperator = '~';
                var rowSeperator = 'ý';

                var columnNames = File.ReadAllText(dataColumnFilePath, Encoding.UTF8).Trim().Split(targetSeperator);


                var lines = File.ReadAllLines(dataFilePath, Encoding.UTF7);


                var dataFilePathPrefix = dataFilePath.Substring(0, dataFilePath.LastIndexOf('.'));

                var csvDataFilePath = dataFilePathPrefix + ".csv";

                if (File.Exists(csvDataFilePath))
                {
                    File.Delete(csvDataFilePath);
                }

                var columns = string.Join(",", columnNames.Select(s => s.Trim()));

                File.AppendAllLines(csvDataFilePath, new string[] { columns });

                string 
                    csvLines1 = string.Empty, 
                    csvLines2 = string.Empty, 
                    csvLines3 = string.Empty, 
                    csvLines4 = string.Empty, 
                    csvLines5 = string.Empty, 
                    csvLines6 = string.Empty, 
                    csvLines7 = string.Empty, 
                    csvLines8 = string.Empty, 
                    csvLines9 = string.Empty, 
                    csvLines10 = string.Empty;

                for (var rowIndex = 0; rowIndex < lines.Length; rowIndex++)
                {
                    //if (rowIndex > 5) break;

                    if (lines.Length > rowIndex)
                        csvLines1 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if(lines.Length > rowIndex)
                        csvLines2 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines3 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines4 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines5 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines6 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines7 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines8 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines9 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }
                    rowIndex++;
                    if (lines.Length > rowIndex)
                        csvLines10 = await GenerateColumnNerLineToRelCSV(lines[rowIndex], columnSeperator, rowSeperator, rowIndex);
                    else
                    {
                        csvLines1 = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(csvLines1))
                        File.AppendAllText(csvDataFilePath, csvLines1);
                    if (!string.IsNullOrEmpty(csvLines2))
                        File.AppendAllText(csvDataFilePath, csvLines2);
                    if (!string.IsNullOrEmpty(csvLines3))
                        File.AppendAllText(csvDataFilePath, csvLines3);
                    if (!string.IsNullOrEmpty(csvLines4))
                        File.AppendAllText(csvDataFilePath, csvLines4);
                    if (!string.IsNullOrEmpty(csvLines5))
                        File.AppendAllText(csvDataFilePath, csvLines5);
                    if (!string.IsNullOrEmpty(csvLines6))
                        File.AppendAllText(csvDataFilePath, csvLines6);
                    if (!string.IsNullOrEmpty(csvLines7))
                        File.AppendAllText(csvDataFilePath, csvLines7);
                    if (!string.IsNullOrEmpty(csvLines8))
                        File.AppendAllText(csvDataFilePath, csvLines8);
                    if (!string.IsNullOrEmpty(csvLines9))
                        File.AppendAllText(csvDataFilePath, csvLines9);
                    if (!string.IsNullOrEmpty(csvLines10))
                        File.AppendAllText(csvDataFilePath, csvLines10); 
                }

                Console.WriteLine("Success : " + "Data transformation operation completed successfully.");
             }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured: "+ ex.Message);
            }
        }

        private static async Task<string> GenerateColumnNerLineToRelCSV(string line,  char columnSeperator, char rowSeperator, int rowIndex)
        {
            var stringBuilder = new StringBuilder();
            var columnsDataList = line.Split(columnSeperator);
            var lineData = string.Empty;
             
            Console.WriteLine("Processing line #" + (rowIndex + 1));

            var maxRecordToBe = columnsDataList.Select(s => s.Count(x => x == rowSeperator)).Max();
        
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

            return stringBuilder.ToString();
        }
    }
}

