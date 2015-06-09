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
    /// Interaction logic for ReportCreator.xaml
    /// </summary>
    public partial class ReportCreator : Window
    {
        bool totalHours;
        public ReportCreator()
        {
            InitializeComponent();
            totalHours = false;

            Users listData1 = new Users();
            Binding binding1 = new Binding();

            binding1.Source = listData1;
            cboEmployee.SetBinding(ListBox.ItemsSourceProperty, binding1);

            //cboEmployee.Items.Add("Eli Price");

            Seats listData2 = new Seats();
            Binding binding2 = new Binding();

            binding2.Source = listData2;
            cboSeat.SetBinding(ListBox.ItemsSourceProperty, binding2);
        }

        private void rdoTotalHours_Click(object sender, RoutedEventArgs e)
        {
            if (!totalHours)
            {
                rdoTotalHours.IsChecked = true;
                totalHours = true;
            }
            else
            {
                rdoTotalHours.IsChecked = false;
                totalHours = false;
            }
        }

        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            String sqlString = "SELECT lastName AS [Last Name], firstName AS [First Name]";

            if (totalHours)
            {
                sqlString += ", SUM(DATEDIFF(MI, start, [end])/60.0) AS [Hours Worked] ";
            }
            else
            {
                sqlString += ", seat AS [Seat], start AS [Start Date], [end] AS [End Date], DATEDIFF(MI, start, [end])/60.0 AS [Hours Worked] ";
            }

            sqlString += "FROM [REVINT].[HEALTHCARE\\eliprice].ed_employeeWorkTable ";
            if (cboEmployee.Text != "" || cboSeat.Text != "" || cboSpecialTag.Text != "" || (dtTPEnd.Text != "" && dtTPStart.Text != "") || (tmDayEnd.Value.ToString() != "" && tmDayStart.Value.ToString() != ""))
            {
                sqlString += "WHERE ";
                bool first = true;

                if (cboEmployee.Text != "")
                {
                    String[] firstLast = cboEmployee.Text.ToString().Split(null as string[], StringSplitOptions.RemoveEmptyEntries);
                    sqlString += "(firstName = '" + firstLast[0] + "' AND lastName = '" + firstLast [1] + "')";
                    first = false;
                }
                if (cboSeat.Text != "")
                {
                    if (!first)
                    {
                        sqlString += " AND ";
                    }
                    sqlString += "(seat = '" + cboSeat.Text + "')";
                }
                if (dtTPEnd.Text != "" && dtTPStart.Text != "") 
                {
                    if (!first)
                    {
                        sqlString += " AND ";
                    }
                    sqlString += "(start BETWEEN '" + dtTPStart + "' AND '" + dtTPEnd + "')";
                }
                /*if (tmDayEnd.Value != null && tmDayStart.Value != null)
                {
                    if (!first)
                    {
                        sqlString += " AND ";
                    }
                    sqlString += "";
                }*/
            }

            if (totalHours) 
            {
                sqlString += " GROUP BY lastName, firstName;";
            }

            setWindows(sqlString);
            
        }
        private void setWindows(String sqlString)
        {
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
                this.dtaReport.ItemsSource = dtable.DefaultView;
                this.dtaReport.CanUserAddRows = false;

                //Close connection
                dbConnection.Close();
            }
        }
    }
}
