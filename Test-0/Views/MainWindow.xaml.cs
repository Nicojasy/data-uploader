using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Test_0.Data;

namespace Test_0
{
    public partial class MainWindow : Window
    {
        public static bool checkPressed = false;
        public static int lineValue = 0;
        public static int lineCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<Movement> custdata = GetData();

            //Bind the DataGrid to the customer data
            DG1.DataContext = custdata;
        }

        private void Button_LoadCVS_Click(object sender, RoutedEventArgs e)
        {
            LoadCVS(false, "..\\Test_File.csv");
        }

        private void Button_SaveCVS_Click(object sender, RoutedEventArgs e)
        {
            LoadCVS(true, "..\\Test_File2.csv");
        }

        private async void LoadCVS(bool mode, string filepath)
        {
            //if (dataGrid.ItemsSource!=null) {
            //    CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).Refresh();
            //    }
            //DataContext.Refresh(RefreshMode.OverwriteCurrentValues, DataContext.SomeDataType);
            Button_SaveCVS.IsEnabled = false;
            Button_LoadCVS.IsEnabled = false;
            Button_StopLoadCVS.IsEnabled = true;
            lineValue = 0;
            lineCount = 0;

            TextBox1.Text += "\n1\n";
            Task<int> loadCVS;
            if (!mode)
            {
                loadCVS = CSVtoDG(filepath);
            }
            else
            {
                loadCVS = DGtoCSV(filepath);
            }
            TextBox1.Text += "2\n";
            var update_pbStatus = PBUpdate();
            TextBox1.Text += "3\n";

            var allTasks = new List<Task> { loadCVS, update_pbStatus };
            while (allTasks.Any())
            {
                Task finished = await Task.WhenAny(allTasks);
                if (finished == loadCVS)
                {
                    TextBox1.Text += "4\n";
                }
                else if (finished == update_pbStatus)
                {
                    TextBox1.Text += "5\n";
                }
                allTasks.Remove(finished);
            }
            TextBox1.Text += "6\n";

            Button_SaveCVS.IsEnabled = true;
            Button_LoadCVS.IsEnabled = true;
            Button_StopLoadCVS.IsEnabled = false;
        }

        //  IMPORT
        public async Task<int> CSVtoDG(string filepath)
        {
            TextBox1.Text += ">>A1\n";
            string[] lines = await Task.Run(() => ReadAllLines(filepath));
            TextBox1.Text += ">>B1\n";
            DataContext = await Task.Run(() => LINQ(lines));
            TextBox1.Text += ">>C1\n";
            checkPressed = true;
            //DataContext = null;
            //DataContext = await Task.Run(() => LINQ(lines));
            return 1;
        }

        public static string[] ReadAllLines(string filepath)
        {
            using (var reader = File.OpenText(filepath))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return File.ReadAllLines(filepath);
        }

        public static IEnumerable<Movement> LINQ(string[] lines)
        {
            IEnumerable<Movement> enumerable()
            {
                foreach (var Lu in lines.Skip(1))
                {
                    var split = Lu.Split(';');
                    yield return new Movement
                    {
                        Date = split[0],
                        Object_A = split[1],
                        Type_A = split[2],
                        Object_B = split[3],
                        Type_B = split[4],
                        Direction = split[5],
                        Color = split[6],
                        Intensity = int.Parse(split[7]),
                        LatitudeA = double.Parse(split[8], CultureInfo.InvariantCulture),
                        LongitudeA = double.Parse(split[9], CultureInfo.InvariantCulture),
                        LatitudeB = double.Parse(split[10], CultureInfo.InvariantCulture),
                        LongitudeB = double.Parse(split[11], CultureInfo.InvariantCulture)
                    };
                    if (checkPressed == true) break;
                    lineValue++;
                }
            }
            var data = enumerable();
            return data.ToList(); ;
        }

        public async Task<int> PBUpdate()
        {
            TextBox1.Text += ">>A2\n";
            do
            {
                pbStatus.Value = Convert.ToInt32(lineValue*100/lineCount);
                await Task.Delay(100);
            } while (checkPressed != true);
            TextBox1.Text += ">>B2\n";
            return 1;
        }

        //  EXPORT
        private async Task<int> DGtoCSV(string filepath)
        {
            dataGrid.SelectAllCells();
            dataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dataGrid);
            dataGrid.UnselectAllCells();
            String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            await Task.Run(() => File.AppendAllText(filepath, result, Encoding.UTF8));
            return 1;
        }

        //  STOP
        private void Button_StopLoadCVS_Click(object sender, RoutedEventArgs e)
        {
            checkPressed = true;
        }

        private void B11_Click(object sender, RoutedEventArgs e)
        {
            //Grid.Children.Remove(DataContext);
            while (dataGrid.SelectedItems.Count > 0)
            {
                ((Movement)DataContext).WallItems.RemoveAt(1);
            }
            //    ((DataRowView)(dataGrid.SelectedItem)).Row.Delete();
            DataContext = null;

            //try
            //{
            //    if (dataGrid.ItemsSource.GetEnumerator.RemoveVisualChild != null)
            //    {
            //        int count = 0;
            //        {
            //            Employee emp = (from ep in EmpCollection
            //                            where ep.EmpNo == eno
            //                            select ep).First();
            //            EmpCollection.Remove(emp);
            //            count++;
            //        }
            //        MessageBox.Show(count + "Row's Deleted");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //            var grid = dataGrid1;
            //            var mygrid = dataGrid1
            //if (grid.SelectedIndex >= 0)
            //            {
            //                for (int i = 0; i <= grid.SelectedItems.Count; i++)
            //                {
            //                    mygrid.Items.Remove(grid.SelectedItems[i]);
            //                };
            //            }

            //            grid = mygrid;
            //for (int i = -datagrid1.SelectedItems.Count; i < datagrid1.SelectedItems.Count; i++)
            //{
            //    datagrid1.SelectedItems.RemoveAt(datagrid1.SelectedIndex);
            //}

            //if (dataGrid.SelectedItem != null)
            //{
            //    ((DataRowView)(dataGrid.SelectedItem)).Row.Delete();
            //}

            //var grid = dataGrid;
            //while (dataGrid.SelectedItems.Count > 0)
            //{
            //    if (dataGrid.SelectedItem == CollectionView.NewItemPlaceholder)
            //        dataGrid.SelectedItems.Remove(grid.SelectedItem);
            //    else
            //        dataGrid.Items.Remove(dataGrid.SelectedItem);
            //}

            //while (dataGrid.SelectedItems.Count >= 1)
            //{
            //    DataRowView row = (DataRowView)dataGrid.SelectedItem;
            //    dt.Rows.Remove(row.Row);
            //}

            //dataGrid.ItemsSource;
            //while (dataGrid.SelectedItems.Count > 0)
            //{
            //    TextBox1 += dataGrid.ItemsSource.ToString();

            //    /*.Rows.RemoveAt(dataGrid.SelectedIndex);*/
            //}
        }
    }
}