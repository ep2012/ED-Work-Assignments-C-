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
    /// Interaction logic for ManageEmployees.xaml
    /// </summary>
    /// 
    public enum ManageEmployeeType {Schedule, Information};

    public partial class ManageEmployees : Window
    {
        ManageEmployeeType windowType;

        public ManageEmployees(ManageEmployeeType type)
        {
            InitializeComponent();
            windowType = type;

            setWindow();
        }
        private void setWindow()
        {
            String sqlString;
            if (windowType == ManageEmployeeType.Information)
            {
                btnViewOther.Content = "View Employee Schedules";
                sqlString = "SELECT A.FirstName AS [First Name], A.LastName AS [Last Name] , B.Title, " +
                    "A.[Address1] AS [Address 1], A.[Address2] AS [Address 2] ,A.[City], A.[State],A.[Zip],A.[Phone],A.[Email],A.[UserName] AS [Healthcare ID],A.[Id]  " +
                    @"FROM [REVINT].[healthcare\eliprice].[ED_Employees] A " +
                    "JOIN [REVINT].[dbo].[ED_Roles] B " +
                    "ON B.Id = A.Role " +
                    "WHERE A.CurrentlyEmployed = 'true' " +
                    "ORDER BY A.FirstName,A.LastName";


            }
            else
            {
                btnViewOther.Content = "View Employee Information";
                sqlString = "SELECT A.FirstName AS [First Name], A.LastName AS [Last Name] , B.Title," +
                    "A.fulltime AS [Is Full Time?], "+
                    "CONVERT(VARCHAR(5), A.[sunday1time], 108) AS [Sunday1 Start], CONVERT(VARCHAR(5), A.[sunday1timeend], 108) AS [Sunday1 End], A.sunday1day AS [Sunday1 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[monday1time], 108) AS [Monday1 Start], CONVERT(VARCHAR(5), A.[monday1timeend], 108) AS [Monday1 End], A.monday1day AS [Monday1 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[tuesday1time], 108) AS [Tuesday1 Start], CONVERT(VARCHAR(5), A.[tuesday1timeend], 108) AS [Tuesday1 End], A.tuesday1day AS [Tuesday1 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[wednesday1time], 108) AS [Wednesday1 Start], CONVERT(VARCHAR(5), A.[wednesday1timeend], 108) AS [Wednesday1 End], A.wednesday1day AS [Wednesday1 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[thursday1time], 108) AS [Thursday1 Start], CONVERT(VARCHAR(5), A.[thursday1timeend], 108) AS [Thursday1 End], A.thursday1day AS [Thursday1 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[friday1time], 108) AS [Friday1 Start], CONVERT(VARCHAR(5), A.[friday1timeend], 108) AS [Friday1 End], A.friday1day AS [Friday1 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[saturday1time], 108) AS [Saturday1 Start], CONVERT(VARCHAR(5), A.[saturday1timeend], 108) AS [Saturday1 End], A.saturday1day AS [Saturday1 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[sunday2time], 108) AS [Sunday2 Start], CONVERT(VARCHAR(5), A.[sunday2timeend], 108) AS [Sunday2 End], A.sunday2day AS [Sunday2 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[monday2time], 108) AS [Monday2 Start], CONVERT(VARCHAR(5), A.[monday2timeend], 108) AS [Monday2 End], A.monday2day AS [Monday2 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[tuesday2time], 108) AS [Tuesday2 Start], CONVERT(VARCHAR(5), A.[tuesday2timeend], 108) AS [Tuesday2 End], A.tuesday2day AS [Tuesday2 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[wednesday2time], 108) AS [Wednesday2 Start], CONVERT(VARCHAR(5), A.[wednesday2timeend], 108) AS [Wednesday2 End], A.wednesday2day AS [Wednesday2 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[thursday2time], 108) AS [Thursday2 Start], CONVERT(VARCHAR(5), A.[thursday2timeend], 108) AS [Thursday2 End], A.thursday2day AS [Thursday2 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[friday2time], 108) AS [Friday2 Start], CONVERT(VARCHAR(5), A.[friday2timeend], 108) AS [Friday2 End], A.friday2day AS [Friday2 Overnight?], " +
                    "CONVERT(VARCHAR(5), A.[saturday2time], 108) AS [Saturday2 Start], CONVERT(VARCHAR(5), A.[saturday2timeend], 108) AS [Saturday2 End], A.saturday2day AS [Saturday2 Overnight?], " +
                    "A.[Id]" +
                    @"FROM [REVINT].[healthcare\eliprice].[ED_Employees] A " +
                    "JOIN [REVINT].[dbo].[ED_Roles] B " +
                    "ON B.Id = A.Role " +
                    "WHERE A.CurrentlyEmployed = 'true' " +
                    "ORDER BY A.FirstName,A.LastName";
            }
            //create an OdbcConnection object and connect it to the data source.
            using (OdbcConnection dbConnection = new OdbcConnection(Connection.cxnString))
            {
                dbConnection.Open();
                OdbcDataAdapter dadapter = new OdbcDataAdapter(sqlString, dbConnection);

                //Create a table and fill it with the data from the adapter
                DataTable dtable = new DataTable();
                dadapter.Fill(dtable);

                this.dtaEmployee.ItemsSource = dtable.DefaultView;
                this.dtaEmployee.CanUserAddRows = false;

                dbConnection.Close();
            }
        }

        private void btnNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEditEmployee win = new AddEditEmployee();
            
            win.Show();
            this.Close();
        }

        private void dtaEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (windowType == ManageEmployeeType.Information)
            {
                AddEditEmployee win = new AddEditEmployee((DataRowView)dtaEmployee.SelectedValue);

                win.Left = Left;
                win.Top = Top;

                win.Show();
                this.Close();
            }
            else
            {
                EditEmployeeSchedule win = new EditEmployeeSchedule(((DataRowView)dtaEmployee.SelectedValue)["Id"].ToString());

                win.Left = Left;
                win.Top = Top;

                win.Show();
                this.Close();
            }
        }

        private void btnViewOther_Click(object sender, RoutedEventArgs e)
        {
            if (windowType == ManageEmployeeType.Information)
            {
                windowType = ManageEmployeeType.Schedule;
                setWindow();
            }
            else
            {
                windowType = ManageEmployeeType.Information;
                setWindow();
            }
        }

    }
}
