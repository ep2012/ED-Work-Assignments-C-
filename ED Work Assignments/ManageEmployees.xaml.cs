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
    public partial class ManageEmployees : Window
    {
        public ManageEmployees()
        {
            InitializeComponent();
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            String sqlString = "SELECT [REVINT].[dbo].[ED_Employees].FirstName AS [First Name], [REVINT].[dbo].[ED_Employees].LastName AS [Last Name] , [REVINT].[dbo].[ED_Roles].Title, [REVINT].[dbo].[ED_Employees].[Address1] AS [Address 1], [REVINT].[dbo].[ED_Employees].[Address2] AS [Address 2] ,[REVINT].[dbo].[ED_Employees].[City],[REVINT].[dbo].[ED_Employees].[State],[REVINT].[dbo].[ED_Employees].[Zip],[REVINT].[dbo].[ED_Employees].[Phone],[REVINT].[dbo].[ED_Employees].[Email],[REVINT].[dbo].[ED_Employees].[UserName] AS [Healthcare ID] "+
                "FROM [REVINT].[dbo].[ED_Employees] "+
                "JOIN [REVINT].[dbo].[ED_Roles] "+
                "On [REVINT].[dbo].[ED_Roles].Id = [REVINT].[dbo].[ED_Employees].Role "+
                "ORDER BY [REVINT].[dbo].[ED_Employees].FirstName,[REVINT].[dbo].[ED_Employees].LastName";
            
            //create an OdbcConnection object and connect it to the data source.
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
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
        }

        private void dtaEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddEditEmployee win = new AddEditEmployee((DataRowView)dtaEmployee.SelectedValue);

            win.Show();
            this.Close();
        }

    }
}
