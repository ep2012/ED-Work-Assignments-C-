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
    /// Interaction logic for IndividualSchedule.xaml
    /// </summary>
    public partial class IndividualSchedule : Window
    {
        Users users = new Users();
        String name;
        String[] names;
        public IndividualSchedule()
        {
            InitializeComponent();

            name = users.getName(Environment.UserName);
            names = users.getName(Environment.UserName).Split(null);
            dtStart.Text = DateTime.Today.ToString();
            dtEnd.Text = DateTime.Today.AddDays(7).ToString();

            setWindow();
        }
        private void setWindow()
        {
            lblSchedule.Content = name + "'s Schedule";

            String sqlString = "Select [REVINT].[dbo].[ED_Shifts].StartShift AS [Start Time], [REVINT].[dbo].[ED_Shifts].EndShift AS [End Time], [REVINT].[dbo].[ED_Seats].Name AS [Seat] "+
                "FROM [REVINT].[dbo].[ED_Shifts] " +
                "JOIN [REVINT].[dbo].[ED_Employees] ON [REVINT].[dbo].[ED_Employees].Id = [REVINT].[dbo].[ED_Shifts].Employee " +
                "JOIN [REVINT].[dbo].[ED_Seats] ON [REVINT].[dbo].[ED_Seats].Id = [REVINT].[dbo].[ED_Shifts].Seat " +
                "WHERE ([REVINT].[dbo].[ED_Employees].FirstName = '" + names[0] + "' AND [REVINT].[dbo].[ED_Employees].LastName = '" + names[1] + "') AND (([REVINT].[dbo].[ED_Shifts].StartShift BETWEEN '" + dtStart.Text.ToString() + "' AND '" + dtEnd.Text.ToString() + "') OR ([REVINT].[dbo].[ED_Shifts].EndShift BETWEEN '" + dtStart.Text.ToString() + "' AND '" + dtEnd.Text.ToString() + "'))";
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            //create an OdbcConnection object and connect it to the data source.
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                //Create adapter from connection and sql to obtain desired data
                OdbcDataAdapter dadapter = new OdbcDataAdapter(sqlString, dbConnection);

                //Create a table and fill it with the data from the adapter
                DataTable dtable = new DataTable();
                dadapter.Fill(dtable);

                //set the contents of the gui grid table to the data table created
                //this.tblView.AutoGenerateColumns = false;
                this.dgSchedule.ItemsSource = dtable.DefaultView;
                this.dgSchedule.CanUserAddRows = false;

                //Close connection
                dbConnection.Close();
            }
        }

        private void CalendarClosed(object sender, RoutedEventArgs e)
        {
            setWindow();
        }
    }
}
