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
    /// Interaction logic for VacationRequests.xaml
    /// </summary>
    public partial class VacationRequests : Window
    {
        public VacationRequests()
        {
            InitializeComponent();
            setWindow();
        }

        private void setWindow()
        {
            String sqlString = "SELECT A.Id, B.FirstName AS [First Name], B.LastName AS [Last Name], A.StartTime AS [Start Time], A.EndTime AS [End Time], A.DateTimeStamp AS [Date Stamp] "+
                @"FROM [REVINT].[HEALTHCARE\eliprice].[ED_TimeOffRequests] A "+
                "JOIN [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] B ON A.EmployeeId = B.Id";
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
                this.dtaRequests.ItemsSource = dtable.DefaultView;
                this.dtaRequests.CanUserAddRows = false;

                //Close connection
                dbConnection.Close();
            }
        }

        private void btnAcceptSchedule_Click(object sender, RoutedEventArgs e)
        {
            object id = ((DataRowView)(dtaRequests).SelectedValue)["Id"].ToString();
            if (id.ToString() != "")
            {
                TimeOffSQL.acceptTimeOffRequest(id);
            }
            setWindow();
        }
    };
}
