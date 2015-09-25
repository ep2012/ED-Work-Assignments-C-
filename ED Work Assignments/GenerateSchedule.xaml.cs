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
using System.Windows.Shapes;

namespace ED_Work_Assignments
{
    /// <summary>
    /// Interaction logic for GenerateSchedule.xaml
    /// </summary>
    public partial class GenerateSchedule : Window
    {
        TempScheduler tempScheduler = new TempScheduler();
        public GenerateSchedule()
        {
            InitializeComponent();
            tempScheduler.clear();
            dtStart.Text = DateTime.Now.ToString();
            dtEnd.Text = DateTime.Now.AddDays(14).ToString();
            btnAcceptSchedule.Visibility = Visibility.Hidden;
        }

        private void btnAcceptSchedule_Click(object sender, RoutedEventArgs e)
        {
            tempScheduler.accept();
            tempScheduler.clear();
            Close();
        }

        private void btnGenerateSchedule_Click(object sender, RoutedEventArgs e)
        {
            tempScheduler.clear();
            ScheduleMaker maker = new ScheduleMaker(DateTime.Parse(dtStart.Text), DateTime.Parse(dtEnd.Text));
            setWindows();
            btnAcceptSchedule.Visibility = Visibility.Visible;
        }

        private void setWindows()
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            
            String sqlString = "SELECT CONCAT(B.[FirstName], ' ' , B.[LastName]) AS [Employee], A.[StartShift] AS [Start Time], A.[EndShift] AS [End Time], A.[Id] AS [Shift Id] " +
                @"FROM [REVINT].[HEALTHCARE\eliprice].ED_ScheduleMakerShifts A " +
                "JOIN [REVINT].[dbo].[ED_Employees] B " +
                "ON B.Id = A.[Employee] " +
                "WHERE (StartShift BETWEEN '" + dtStart.Text + "' AND '" + dtEnd.Text + "' OR" +
                " EndShift BETWEEN '" + dtStart.Text + "' AND '" + dtEnd.Text + "') " +
                "AND Seat = ";

            //create an OdbcConnection object and connect it to the data source.
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                //Create adapter from connection and sql to obtain desired data
                OdbcDataAdapter dadapterWOW1 = new OdbcDataAdapter(sqlString + "3 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterWOW2 = new OdbcDataAdapter(sqlString + "4 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterCheckIn = new OdbcDataAdapter(sqlString + "1 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterCheckOut = new OdbcDataAdapter(sqlString + "5 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterPOD12 = new OdbcDataAdapter(sqlString + "6 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterPOD34 = new OdbcDataAdapter(sqlString + "7 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterJetPeds = new OdbcDataAdapter(sqlString + "8 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapteriPad = new OdbcDataAdapter(sqlString + "2 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterSupervising = new OdbcDataAdapter(sqlString + "9 ORDER BY StartShift", dbConnection);

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
    }
}
