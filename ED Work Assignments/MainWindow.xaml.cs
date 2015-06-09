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
            //dtPicker.Text = "5/6/2015";
            setWindows();

            Users users = new Users();

            String [] name = users.getName(Environment.UserName).Split(null as string[], StringSplitOptions.RemoveEmptyEntries);

            lblWelcome.Content = "Hello, " + name[0] + " " + name[1] + "!";
            
            if (!isAdmin())
            {
                btnAddAssignment.Visibility = Visibility.Hidden;
                btnUploadAssignment.Visibility = Visibility.Hidden;
                btnChangeTracker.Visibility = Visibility.Hidden;
            }
        }
        private void setWindows()
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            DateTime lastDay; 
            DateTime.TryParse(dtPicker.Text.ToString(), out lastDay);
            String otherDate = lastDay.AddDays(1).ToString();
            String sqlString = "SELECT [REVINT].[dbo].[ED_Employees].[FirstName] AS [First Name], [REVINT].[dbo].[ED_Employees].LastName AS [Last Name], [REVINT].[dbo].[ED_Shifts].[StartShift] AS [Start Time], [REVINT].[dbo].[ED_Shifts].[EndShift] AS [End Time] "+
                "FROM [REVINT].[dbo].[ED_Shifts] "+
                "JOIN [REVINT].[dbo].[ED_Employees] "+
                "ON [REVINT].[dbo].[ED_Employees].Id = [REVINT].[dbo].[ED_Shifts].[Employee] "+
                "WHERE Seat = ";
            //String sqlString = "Select lastName AS [Last Name], firstName AS [First Name], start AS [Start Time], [end] AS [End Time] FROM ed_employeeWorkTable WHERE ((start BETWEEN '" + dtPicker.Text.ToString() + "' AND '" + otherDate + "') OR ([end] BETWEEN '" + dtPicker.Text.ToString() + "' AND '" + otherDate + "')) AND seat = '";
            
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
                //OdbcDataAdapter dadapterSupervising = new OdbcDataAdapter(sqlString + "9", dbConnection);

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

                //Close connection
                dbConnection.Close();
            }
        }

        private void dtPicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            setWindows();
        }
        public bool isAdmin() 
        {
            String [] admins = {"eliprice", "miaria", "soversm", "jensend", "stoutk"};
            for (int i = 0; i < admins.Length; i++ )
            {
                if (Environment.UserName == admins[i])
                {
                    return true;
                }
            }    
            return false;
        }

        private void btnAddAssignment_Click(object sender, RoutedEventArgs e)
        {
            NewAssignment win = new NewAssignment();

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
    }
}
