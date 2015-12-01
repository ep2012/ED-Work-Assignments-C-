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
        public GenerateSchedule()
        {
            InitializeComponent();
            TempSchedulerSQL.clear();
            dtStart.Text = DateTime.Now.ToString();
            dtEnd.Text = DateTime.Now.AddMonths(1).ToString();

            btnAcceptSchedule.Visibility = Visibility.Hidden;
        }

        private void btnAcceptSchedule_Click(object sender, RoutedEventArgs e)
        {
            TempSchedulerSQL.accept();
            TempSchedulerSQL.clear();
            Close();
        }

        private void btnGenerateSchedule_Click(object sender, RoutedEventArgs e)
        {
            TempSchedulerSQL.clear();
            ScheduleMaker maker = new ScheduleMaker(DateTime.Parse(dtStart.Text), DateTime.Parse(dtEnd.Text), SchedulingMode.Type1);
            setWindows();
            btnAcceptSchedule.Visibility = Visibility.Visible;
        }

        private void setWindows()
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            
            String sqlString = "SELECT CONCAT(B.[FirstName], ' ' , B.[LastName]) AS [Employee], A.[StartShift] AS [Start Time], A.[EndShift] AS [End Time], A.[Id] AS [Shift Id] " +
                @"FROM [REVINT].[HEALTHCARE\eliprice].ED_ScheduleMakerShifts A " +
                "LEFT JOIN [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] B " +
                "ON B.Id = A.[Employee] " +
                "WHERE (StartShift BETWEEN '" + dtStart.Text + "' AND '" + dtEnd.Text + "' OR" +
                " EndShift BETWEEN '" + dtStart.Text + "' AND '" + dtEnd.Text + "') " +
                "";

            //create an OdbcConnection object and connect it to the data source.
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                //Create adapter from connection and sql to obtain desired data
                OdbcDataAdapter dadapterWOW1 = new OdbcDataAdapter(sqlString + " AND Seat = 3 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterWOW2 = new OdbcDataAdapter(sqlString + " AND Seat = 4 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterCheckIn = new OdbcDataAdapter(sqlString + " AND Seat = 1 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterCheckOut = new OdbcDataAdapter(sqlString + " AND Seat = 5 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterPOD12 = new OdbcDataAdapter(sqlString + " AND Seat = 6 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterPOD34 = new OdbcDataAdapter(sqlString + " AND Seat = 7 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterJetPeds = new OdbcDataAdapter(sqlString + " AND Seat = 8 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapteriPad = new OdbcDataAdapter(sqlString + " AND Seat = 2 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterSupervising = new OdbcDataAdapter(sqlString + " AND Seat = 9 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterOverStaffing = new OdbcDataAdapter(sqlString + " AND Seat = 10 ORDER BY StartShift", dbConnection);
                OdbcDataAdapter dadapterStaffingHoles = new OdbcDataAdapter(sqlString + " AND A.[Employee] IS NULL ORDER BY StartShift", dbConnection);

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

                DataTable dtableOverStaffing = new DataTable();
                dadapterOverStaffing.Fill(dtableOverStaffing);

                DataTable dtableStaffingHoles = new DataTable();
                dadapterStaffingHoles.Fill(dtableStaffingHoles);

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

                this.dtaOverStaffing.ItemsSource = dtableOverStaffing.DefaultView;
                this.dtaOverStaffing.CanUserAddRows = false;

                this.dtaMinStaffingHoles.ItemsSource = dtableStaffingHoles.DefaultView;
                this.dtaMinStaffingHoles.CanUserAddRows = false;
                //Close connection
                dbConnection.Close();
            }
        }

        private void btnGenerateSchedule2_Click(object sender, RoutedEventArgs e)
        {
            TempSchedulerSQL.clear();
            ScheduleMaker maker = new ScheduleMaker(DateTime.Parse(dtStart.Text), DateTime.Parse(dtEnd.Text), SchedulingMode.Type2);
            setWindows();
            btnAcceptSchedule.Visibility = Visibility.Visible;
        }
    }
}
