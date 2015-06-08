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
    public partial class NewAssignment : Window
    {
        public NewAssignment()
        {
            InitializeComponent();

            Users listData1 = new Users();
            Binding binding1 = new Binding();

            binding1.Source = listData1;
            cboEmployee.SetBinding(ListBox.ItemsSourceProperty, binding1);

            //cboEmployee.Items.Add("Eli Price");

            cboSeat.Items.Add("WOW1");
            cboSeat.Items.Add("WOW2");
            cboSeat.Items.Add("Check In");
            cboSeat.Items.Add("Check Out");
            cboSeat.Items.Add("POD 1/2");
            cboSeat.Items.Add("POD 3/4");
            cboSeat.Items.Add("Jet/Peds");
            cboSeat.Items.Add("iPad");

            cboSpecialTag.Items.Add("Manager");
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

                    cmd.CommandText = "{CALL ed_newWorkAssignment(?, ?, ?, ?, ?, ?)}";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = dbConnection;

                    String[] firstLast = cboEmployee.Text.ToString().Split(null as string[], StringSplitOptions.RemoveEmptyEntries);

                    cmd.Parameters.Add("@firstName", OdbcType.NVarChar, 100).Value = firstLast[0];
                    cmd.Parameters.Add("@lastName", OdbcType.NVarChar, 100).Value =  firstLast[1];
                    cmd.Parameters.Add("@seat", OdbcType.NVarChar, 100).Value = cboSeat.Text;
                    cmd.Parameters.Add("@start", OdbcType.DateTime).Value = dtpStart.Text;
                    cmd.Parameters.Add("@end", OdbcType.DateTime).Value = dtpEnd.Text;
                    cmd.Parameters.Add("@specialTag", OdbcType.NVarChar, 100).Value = cboSpecialTag.Text;

                    cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }
            }
        }
    }
}
