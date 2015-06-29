using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
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

namespace ED_Work_Assignments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dtPicker.Text = DateTime.Today.ToString();
            setWindows();

            Users users = new Users();

            String name = users.getName(Environment.UserName);

            lblWelcome.Content = "Hello, " + name + "!";
            
            if (!users.isAdmin(Environment.UserName))
            {
                btnAddAssignment.Visibility = Visibility.Hidden;
                btnUploadAssignment.Visibility = Visibility.Hidden;
                btnChangeTracker.Visibility = Visibility.Hidden;
                btnReportCreator.Visibility = Visibility.Hidden;
                btnManageEmployees.Visibility = Visibility.Hidden;
            }
        }

        private void setWindows()
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            DateTime lastDay; 
            DateTime.TryParse(dtPicker.Text.ToString(), out lastDay);
            String otherDate = lastDay.AddDays(1).ToString();
            String sqlString = "SELECT CONCAT([REVINT].[dbo].[ED_Employees].[FirstName], ' ' , [REVINT].[dbo].[ED_Employees].[LastName]) AS [Employee], [REVINT].[dbo].[ED_Shifts].[StartShift] AS [Start Time], [REVINT].[dbo].[ED_Shifts].[EndShift] AS [End Time], [REVINT].[dbo].[ED_Shifts].[Id] AS [Shift Id] " +
                "FROM [REVINT].[dbo].[ED_Shifts] " +
                "JOIN [REVINT].[dbo].[ED_Employees] " +
                "ON [REVINT].[dbo].[ED_Employees].Id = [REVINT].[dbo].[ED_Shifts].[Employee] " +
                "WHERE (StartShift BETWEEN '" + dtPicker.Text.ToString() + "' AND '" + otherDate + "' OR EndShift BETWEEN '" + dtPicker.Text.ToString() + "' AND '" + otherDate + "') "+
                "AND Seat = ";
            
            //create an OdbcConnection object and connect it to the data source.
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                //Create adapter from connection and sql to obtain desired data
                OdbcDataAdapter dadapterWOW1 = new OdbcDataAdapter(sqlString + "3" , dbConnection);
                OdbcDataAdapter dadapterWOW2 = new OdbcDataAdapter(sqlString + "4" , dbConnection);
                OdbcDataAdapter dadapterCheckIn = new OdbcDataAdapter(sqlString + "1" , dbConnection);
                OdbcDataAdapter dadapterCheckOut = new OdbcDataAdapter(sqlString + "5" , dbConnection);
                OdbcDataAdapter dadapterPOD12 = new OdbcDataAdapter(sqlString + "6" , dbConnection);
                OdbcDataAdapter dadapterPOD34 = new OdbcDataAdapter(sqlString + "7" , dbConnection);
                OdbcDataAdapter dadapterJetPeds = new OdbcDataAdapter(sqlString + "8" , dbConnection);
                OdbcDataAdapter dadapteriPad = new OdbcDataAdapter(sqlString + "2", dbConnection);
                OdbcDataAdapter dadapterSupervising = new OdbcDataAdapter(sqlString + "9", dbConnection);

                //Create a table and fill it with the data from the adapter
                DataTable dtableWOW1 = new DataTable();
                dadapterWOW1.Fill(dtableWOW1);

                DataTable dtableWOW2 = new DataTable();
                dadapterWOW2.Fill(dtableWOW2);

                DataTable dtableCheckIn = new DataTable();
                dadapterCheckIn.Fill(dtableCheckIn);

                DataTable dtableCheckOut = new DataTable();
                dadapterCheckOut.Fill(dtableCheckOut);

                DataTable dtablePOD12 = new DataTable();
                dadapterPOD12.Fill(dtablePOD12);

                DataTable dtablePOD34 = new DataTable();
                dadapterPOD34.Fill(dtablePOD34);

                DataTable dtableJetPeds = new DataTable();
                dadapterJetPeds.Fill(dtableJetPeds);

                DataTable dtableiPad = new DataTable();
                dadapteriPad.Fill(dtableiPad);

                DataTable dtableSupervising = new DataTable();
                dadapterSupervising.Fill(dtableSupervising);

                //set the contents of the gui grid table to the data table created
                this.dtaWOW1.ItemsSource = dtableWOW1.DefaultView;
                this.dtaWOW1.CanUserAddRows = false;

                this.dtaWOW2.ItemsSource = dtableWOW2.DefaultView;
                this.dtaWOW2.CanUserAddRows = false;

                this.dtaCheckIn.ItemsSource = dtableCheckIn.DefaultView;
                this.dtaCheckIn.CanUserAddRows = false;

                this.dtaCheckOut.ItemsSource = dtableCheckOut.DefaultView;
                this.dtaCheckOut.CanUserAddRows = false;

                this.dtaPOD12.ItemsSource = dtablePOD12.DefaultView;
                this.dtaPOD12.CanUserAddRows = false;

                this.dtaPOD34.ItemsSource = dtablePOD34.DefaultView;
                this.dtaPOD34.CanUserAddRows = false;

                this.dtaJetPeds.ItemsSource = dtableJetPeds.DefaultView;
                this.dtaJetPeds.CanUserAddRows = false;

                this.dtaiPad.ItemsSource = dtableiPad.DefaultView;
                this.dtaiPad.CanUserAddRows = false;

                this.dtaSupervising.ItemsSource = dtableSupervising.DefaultView;
                this.dtaSupervising.CanUserAddRows = false;

                //Close connection
                dbConnection.Close();
            }
        }

        private void dtPicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            setWindows();
        }

        private void btnAddAssignment_Click(object sender, RoutedEventArgs e)
        {
            NewAssignment win = new NewAssignment(this);

            win.Show();
        }

        private void btnIndividualSchedule_Click(object sender, RoutedEventArgs e)
        {
            IndividualSchedule win = new IndividualSchedule();

            win.Show();
        }

        private void btnChangeTracker_Click(object sender, RoutedEventArgs e)
        {
            ChangeTracker win = new ChangeTracker();

            win.Show();
        }

        private void btnReportCreator_Click(object sender, RoutedEventArgs e)
        {
            ReportCreator win = new ReportCreator();

            win.Show();
        }
        public void update()
        {
            setWindows();
        }

        private void dta_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NewAssignment win = new NewAssignment(this, (DataRowView)((DataGrid)sender).SelectedValue,((TabItem) tbControl.SelectedItem).Header.ToString());

            win.Show();
        }

        private void btnNextDay_Click(object sender, RoutedEventArgs e)
        {
            dtPicker.Text = DateTime.Parse(dtPicker.Text).AddDays(1).ToString();
            setWindows();
        }

        private void btnPreviousDay_Click(object sender, RoutedEventArgs e)
        {
            dtPicker.Text = DateTime.Parse(dtPicker.Text).AddDays(-1).ToString();
            setWindows();
        }

        private void dta_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            ((DataGrid)sender).CancelEdit();
        }

        private void btnManageEmployees_Click(object sender, RoutedEventArgs e)
        {
            ManageEmployees win = new ManageEmployees();

            win.Show();
        }

        private void btnUploadAssignment_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = "*.xlsx;*.xlsm:.xls"; // Default file extension
            dlg.Filter = "Excel Worksheets|*.xlsx"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = excelApp.Workbooks.Open(dlg.FileName.ToString());

                Microsoft.Office.Interop.Excel.Worksheet excelSheet = wb.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)excelSheet.Cells;

                Microsoft.Office.Interop.Excel.Range last = excelSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);

                int lastUsedRow = last.Row;

                Mouse.OverrideCursor = Cursors.Wait;

                int notSuccessful = 0;
                List<int> notSuccessfulRows = new List<int>();

                int row;

                Users users = new Users();

                Seats seats = new Seats();

                for (row = 2; row <= lastUsedRow; row++)
                {
                    try
                    {
                        using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                        {
                            //open OdbcConnection object
                            dbConnection.Open();

                            OdbcCommand cmd = new OdbcCommand();

                            cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_newWorkAssignment(?, ?, ?, ?)}";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Connection = dbConnection;

                            cmd.Parameters.Add("@employee", OdbcType.Int).Value = users.getID(((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 1]).Value);
                            cmd.Parameters.Add("@seat", OdbcType.Int).Value = seats.getID(((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 2]).Value);
                            cmd.Parameters.Add("@start", OdbcType.DateTime).Value = ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 3]).Value;
                            cmd.Parameters.Add("@end", OdbcType.DateTime).Value = ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 4]).Value;

                            cmd.ExecuteNonQuery();

                            dbConnection.Close();
                        }
                        
                    }
                    catch (System.Data.Odbc.OdbcException)
                    {
                        Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)excelSheet.Rows[1];

                        firstRow.EntireRow.Font.Bold = true;
                        firstRow.Cells.Interior.ColorIndex = 36;
                        notSuccessful++;
                        notSuccessfulRows.Add(row);
                    }
                    using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                    {
                        //open OdbcConnection object
                        dbConnection.Open();

                        OdbcCommand cmd = new OdbcCommand();

                        cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateChangeTracker(?, ?)}";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection = dbConnection;

                        cmd.Parameters.Add("@username", OdbcType.NVarChar, 100).Value = Environment.UserName;
                        cmd.Parameters.Add("@notes", OdbcType.NVarChar, 4000).Value = "Uploaded file: " + dlg.FileName;

                        cmd.ExecuteNonQuery();

                        dbConnection.Close();
                    }
                }

                Mouse.OverrideCursor = null;

                if (notSuccessful > 0)
                {
                    var dialogResult = MessageBox.Show("Completed inserting records into database. Could not insert " + notSuccessful.ToString() + " records.\nView the row numbers for these records?", "Finished inserting records", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        var dialogResult2 = MessageBox.Show(ArrayToString(notSuccessfulRows.ToArray()) + "\n\nCopy to Clipboard?", "Unsuccessful row numbers", MessageBoxButton.YesNo);
                        if (dialogResult2 == MessageBoxResult.Yes)
                        {
                            Clipboard.SetText(ArrayToString(notSuccessfulRows.ToArray()));
                        }
                    }
                }

                wb.Close();
                setWindows();
            }
        }

        private string ArrayToString(int[] arr)
        {
            string builder = "";
            foreach (int val in arr)
            {
                builder = builder + " " + val.ToString() + " ";
            }
            builder = builder.Trim();
            return builder;
        }

    }
}
