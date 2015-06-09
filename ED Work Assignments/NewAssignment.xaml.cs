using System;
using System.Collections.Generic;
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
    /// Interaction logic for NewAssignment.xaml
    /// </summary>
    /// 
    public partial class NewAssignment : Window
    {
        Users users = new Users();
        Seats seats = new Seats();

        public NewAssignment()
        {
            InitializeComponent();

            Binding binding1 = new Binding();

            binding1.Source = users;
            cboEmployee.SetBinding(ListBox.ItemsSourceProperty, binding1);


            Binding binding2 = new Binding();

            binding2.Source = seats;
            cboSeat.SetBinding(ListBox.ItemsSourceProperty, binding2);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            var dialogResult = MessageBox.Show("Are you sure you would like to add this work assignment?", "Inserting into database", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                {
                    //open OdbcConnection object
                    dbConnection.Open();

                    OdbcCommand cmd = new OdbcCommand();

                    cmd.CommandText = "{CALL ed_newWorkAssignment(?, ?, ?, ?)}";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = dbConnection;

                    cmd.Parameters.Add("@employee", OdbcType.Int).Value = users.getID(cboEmployee.Text);
                    cmd.Parameters.Add("@seat", OdbcType.Int).Value = seats.getID(cboSeat.Text);
                    cmd.Parameters.Add("@start", OdbcType.DateTime).Value = dtpStart.Value;
                    cmd.Parameters.Add("@end", OdbcType.DateTime).Value = dtpEnd.Value;

                    cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }
            }

            this.Close();
        }

        private void btnManageEmployees_Click(object sender, RoutedEventArgs e)
        {
            ManageEmployees win = new ManageEmployees();

            win.Show();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
