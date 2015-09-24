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
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Wrapper = Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System.Data.SqlClient;


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

            Users users = new Users();

            if (!users.isAdmin(Environment.UserName))
            {
                IndividualSchedule win = new IndividualSchedule();
                win.Left = Left;
                win.Top = Top;
                win.Show();
                Close();
            }
            else
            {
                dtPicker.Text = DateTime.Today.ToString();
                setWindows();

                String name = users.getName(Environment.UserName);

                lblWelcome.Content = "Hello, " + name + "!";
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
                "WHERE (StartShift BETWEEN '" + dtPicker.Text.ToString() + "' AND '" + otherDate + "' OR" +
                " EndShift BETWEEN '" + dtPicker.Text.ToString() + "' AND '" + otherDate + "') "+
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

            win.Left = Left;
            win.Top = Top;

            win.Show();
        }

        private void btnReportCreator_Click(object sender, RoutedEventArgs e)
        {
            ReportCreator win = new ReportCreator();

            win.Left = Left;
            win.Top = Top;

            win.Show();
        }
        public void update()
        {
            setWindows();
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

        private void btnManageEmployees_Click(object sender, RoutedEventArgs e)
        {
            ManageEmployees win = new ManageEmployees();

            win.Left = Left;
            win.Top = Top;

            win.Show();
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
        private void dtadblClick(object sender, String tableString)
        {
            NewAssignment win = new NewAssignment(this, (DataRowView)((DataGrid)sender).SelectedValue, tableString);

            win.Left = Left;
            win.Top = Top;
            win.Show();
            win.Topmost = true;
        }

        private void dtaWOW1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "WOW 1");
        }

        private void dtaWOW2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "WOW 2");
        }

        private void dtaCheckIn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "Check In");
        }

        private void dtaPOD12_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "POD 1/2");
        }

        private void dtaCheckOut_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "Check Out");
        }

        private void dtaPOD34_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "POD 3/4");
        }

        private void dtaJetPeds_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "Jet/Peds");
        }

        private void dtaiPad_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "iPad");
        }

        private void dtaSupervising_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dtadblClick(sender, "Supervising");
        }

        private void btnGenerateSchedule_Click(object sender, RoutedEventArgs e)
        {
            GenerateSchedule win = new GenerateSchedule();

            win.Show();

            win.Left = this.Left;
            win.Top = this.Top;
        }

        private void btnVacationRequests_Click(object sender, RoutedEventArgs e)
        {
            VacationRequests win = new VacationRequests();

            win.Left = Left;
            win.Top = Top;

            win.Show();
        }

        private void btnAbsenseReporting_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnUploadAssignment_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnPlannedTimeOff_Click(object sender, RoutedEventArgs e)
        {
            EmployeeTimeOff win = new EmployeeTimeOff();

            win.Left = Left;
            win.Top = Top;

            win.Show();
        }
    }
}
