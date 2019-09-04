using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Data;
//using System.Text;



namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DataTable alldata = new DataTable();

        
        public MainWindow()
        {
            InitializeComponent();
        }
        public System.Data.DataTable readCSV(string filePath)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filePath))
            {
                string strLine = sr.ReadLine();

                string[] strArray = strLine.Split(',');

                foreach (string value in strArray)
                {
                    dt.Columns.Add(value.Trim());
                }
                System.Data.DataRow dr = dt.NewRow();

                while (sr.Peek() >= 0)
                {
                    strLine = sr.ReadLine();
                    strArray = strLine.Split(',');
                    dt.Rows.Add(strArray);
                }
            }
            return dt;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string file = null;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                file = dlg.FileName;

            }
            DataTable gettable = readCSV(file);

            hp.ItemsSource = gettable.DefaultView;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

          
            alldata.Columns.Add("Song List");

            using (var driver = new ChromeDriver())
            {

                driver.Navigate().GoToUrl("http://www.radiomirchi.com/more/tamil-top-20/");


             
                
                var all = driver.FindElements(By.ClassName("header"));
                

                foreach(IWebElement name in all)
                {

                  var maindata = name.FindElements(By.TagName("h2"));

                    foreach (IWebElement subdatat in maindata)
                    {

                        DataRow dr = alldata.NewRow();


                        dr["Song List"] = subdatat.Text;

                        alldata.Rows.Add(dr);
                    }
                }


                hp.ItemsSource = alldata.DefaultView;
          


            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            hp.ItemsSource = null;
            string file = null;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                file = dlg.FileName;

            }
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = alldata.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in alldata.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(file, sb.ToString());

        }
    }

}