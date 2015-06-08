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
        String[] name;
        public IndividualSchedule()
        {
            InitializeComponent();

            name = users.getName(Environment.UserName).Split(null as string[], StringSplitOptions.RemoveEmptyEntries);
            dtStart.Text = DateTime.Today.ToString();
            dtEnd.Text = DateTime.Today.AddDays(7).ToString();

            setWindow();
        }
        private void setWindow()
        {
            lblSchedule.Content = name[0] + " " + name[1] + "'s Schedule";

            String sqlString = "Select lastName AS [Last Name], firstName AS [First Name], start AS [Start Time], [end] AS [End Time], seat AS [Seat] FROM ed_employeeWorkTable WHERE (firstName = '" + name[0] + "' AND lastName = '" + name[1] + "') AND ((start BETWEEN '" + dtStart.Text.ToString() + "' AND '" + dtEnd.Text.ToString() + "') OR ([end] BETWEEN '" + dtStart.Text.ToString() + "' AND '" + dtEnd.Text.ToString() + "'))";
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
