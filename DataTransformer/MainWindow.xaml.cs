using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataTransformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private char targetSeperator = ',';

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtDataFilePath.Text =  openFileDialog.FileName;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtColumnFilePath.Text = openFileDialog.FileName;
        }

        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (txtDataFilePath.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select the data file");

                    return;
                }

                if (txtColumnFilePath.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select the columns file");

                    return;
                }

                var columnSeperator = txtColumnSeperator.Text.ToArray().First();
                var rowSeperator = txtRowSeperator.Text.ToArray().First();

                var columnNames = File.ReadAllText(txtColumnFilePath.Text, Encoding.UTF8).Trim().Split(targetSeperator);
                 

                var lines = //new string[] { "Mr. X~BDTýUSDýGBPýEUR~1000ý150ý25ý~20150101ý20150215ý20160310ý20160415~10" };

                 File.ReadAllLines(txtDataFilePath.Text, Encoding.UTF7);
                
                var dataFilePath = txtDataFilePath.Text;

                var dataFilePathPrefix = dataFilePath.Substring(0, dataFilePath.LastIndexOf('.'));

                var csvDataFilePath = dataFilePathPrefix + ".csv";

                if (File.Exists(csvDataFilePath))
                {
                    File.Delete(csvDataFilePath);
                }

                var columns = string.Join(",", columnNames.Select(s => s.Trim()));

                File.AppendAllLines(csvDataFilePath, new string[] { columns });

                var rowIndex = -1;                 

                foreach (var line in lines)
                {
                    var columnsDataList = line.Split(columnSeperator);
                    var lineData = string.Empty;

                    rowIndex = rowIndex + 1;
                     
                    var maxRecordToBe = columnsDataList.Select(s => s.Count(x => x == rowSeperator)).Max();
                    //string[,] rows = new string[maxRecordToBe, columnNames.Length];

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

                        //stringBuilder.AppendLine(lineData);

                        //Aappend one line at a time to avoide out of memory exception
                        File.AppendAllLines(csvDataFilePath, new string[] { lineData });
                    }
                }

                //This take huge memory and throw out of memory exception 
                //var csvData = stringBuilder.ToString();              

                //File.WriteAllText(csvDataFilePath, csvData);

                 

                MessageBox.Show("CSV data file created: " + csvDataFilePath, "Data transformation operation completed successfully", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Data transformation operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
    }
}
