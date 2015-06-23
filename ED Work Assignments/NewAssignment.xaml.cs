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
    /// Interaction logic for NewAssignment.xaml
    /// </summary>
    /// 
    public partial class NewAssignment : Window
    {
        Users users = new Users();
        Seats seats = new Seats();

        enum AssignmentType { New, Update };

        AssignmentType assignmentType;

        MainWindow mainWindow;

        String newAssignment = "New Work Assignment";
        String updateAssignment = "Update Work Assignment";
        String previousRecordDetail;

        int id;

        public NewAssignment(MainWindow main)
        {
            InitializeComponent();

            mainWindow = main;

            setBindings();

            assignmentType = AssignmentType.New;

            btnDeleteAssignment.Visibility = Visibility.Hidden;

            lblWorkAssignment.Content = newAssignment;
        }

        public NewAssignment(MainWindow main, DataRowView row, String seat)
        {
            InitializeComponent();

            this.Title = "ED Update Work Assignment";

            mainWindow = main;

            setBindings();
            try
            {
                cboEmployee.Text = row["Employee"].ToString();

                cboSeat.Text = seat;

                dtpEnd.Value = DateTime.Parse(row["End Time"].ToString());
                dtpStart.Value = DateTime.Parse(row["Start Time"].ToString());

                assignmentType = AssignmentType.Update;

                id = int.Parse(row["Shift Id"].ToString());

                lblWorkAssignment.Content = updateAssignment;

                previousRecordDetail = cboEmployee.Text + ".\nStarting: " + dtpStart.Value + ".\nEnding: " + dtpEnd.Value + ".\nIn Seat " + cboSeat.Text + ".";
                this.Show();            
            }
            catch (Exception)
            { }
        }

        private void setBindings()
        {
            Binding binding1 = new Binding();

            binding1.Source = users;
            cboEmployee.SetBinding(ListBox.ItemsSourceProperty, binding1);
            Binding binding2 = new Binding();

            binding2.Source = seats;
            cboSeat.SetBinding(ListBox.ItemsSourceProperty, binding2);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validInputs())
            {
                if (assignmentType == AssignmentType.New)
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

                            cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_newWorkAssignment(?, ?, ?, ?)}";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Connection = dbConnection;

                            cmd.Parameters.Add("@employee", OdbcType.Int).Value = users.getID(cboEmployee.Text);
                            cmd.Parameters.Add("@seat", OdbcType.Int).Value = seats.getID(cboSeat.Text);
                            cmd.Parameters.Add("@start", OdbcType.DateTime).Value = dtpStart.Value;
                            cmd.Parameters.Add("@end", OdbcType.DateTime).Value = dtpEnd.Value;

                            cmd.ExecuteNonQuery();

                            dbConnection.Close();
                        }
                        using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                        {
                            //open OdbcConnection object
                            dbConnection.Open();

                            OdbcCommand cmd = new OdbcCommand();

                            cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateChangeTracker(?, ?)}";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Connection = dbConnection;

                            cmd.Parameters.Add("@username", OdbcType.NVarChar, 100).Value = Environment.UserName;
                            cmd.Parameters.Add("@notes", OdbcType.NVarChar, 4000).Value = "Created record for " + cboEmployee.Text + ".\nStarting: " + dtpStart.Value + ".\nEnding: " + dtpEnd.Value + ".\nIn Seat " + cboSeat.Text + ".";

                            cmd.ExecuteNonQuery();

                            dbConnection.Close();
                        }
                    }
                    
                }
                else
                {
                    String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
                    var dialogResult = MessageBox.Show("Are you sure you would like to update this work assignment?", "Updating Assignment", MessageBoxButton.YesNo);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                        {
                            //open OdbcConnection object
                            dbConnection.Open();

                            OdbcCommand cmd = new OdbcCommand();

                            cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateWorkAssignment(?, ?, ?, ?, ?)}";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Connection = dbConnection;

                            cmd.Parameters.Add("@employee", OdbcType.Int).Value = users.getID(cboEmployee.Text);
                            cmd.Parameters.Add("@seat", OdbcType.Int).Value = seats.getID(cboSeat.Text);
                            cmd.Parameters.Add("@start", OdbcType.DateTime).Value = dtpStart.Value;
                            cmd.Parameters.Add("@end", OdbcType.DateTime).Value = dtpEnd.Value;
                            cmd.Parameters.Add("@id", OdbcType.Int).Value = id.ToString();

                            cmd.ExecuteNonQuery();

                            dbConnection.Close();
                        }
                        using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                        {
                            //open OdbcConnection object
                            dbConnection.Open();

                            OdbcCommand cmd = new OdbcCommand();

                            cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateChangeTracker(?, ?)}";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Connection = dbConnection;

                            cmd.Parameters.Add("@username", OdbcType.NVarChar, 100).Value = Environment.UserName;
                            cmd.Parameters.Add("@notes", OdbcType.NVarChar, 4000).Value = "Updated record from:\nEmployee: " + previousRecordDetail + "\n\nTo:\nEmployee: " + cboEmployee.Text + ".\nStarting: " + dtpStart.Value + ".\nEnding: " + dtpEnd.Value + ".\nIn Seat " + cboSeat.Text + ".";

                            cmd.ExecuteNonQuery();

                            dbConnection.Close();
                        }
                    }
                }
                if (mainWindow.ShowActivated)
                {
                    mainWindow.update();
                }
                this.Close();
            }

        }
        private bool validInputs()
        {
            if (cboEmployee.Text == "" || cboSeat.Text == "" || dtpEnd.Value.ToString() == "" || dtpStart.Value.ToString() == "")
            {
                var dialogBox = MessageBox.Show("Please fill in all the required values.", "Invalid Input", MessageBoxButton.OK);

                return false;
            }
            else if (dtpEnd.Value < dtpStart.Value)
            {
                var dialogBox = MessageBox.Show("The end date must be after the start date.", "Invalid Input", MessageBoxButton.OK);

                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnManageEmployees_Click(object sender, RoutedEventArgs e)
        {
            ManageEmployees win = new ManageEmployees();

            win.Show();

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDeleteAssignment_Click(object sender, RoutedEventArgs e)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            var dialogResult = MessageBox.Show("Are you sure you would like to delete this work assignment?", "Deleting Assignment", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                {
                    //open OdbcConnection object
                    dbConnection.Open();

                    OdbcCommand cmd = new OdbcCommand();

                    cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_deleteWorkAssignment(?)}";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = dbConnection;

                    cmd.Parameters.Add("@id", OdbcType.Int).Value = id.ToString();

                    cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }

                using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                {
                    //open OdbcConnection object
                    dbConnection.Open();

                    OdbcCommand cmd = new OdbcCommand();

                    cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateChangeTracker(?, ?)}";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = dbConnection;

                    cmd.Parameters.Add("@username", OdbcType.NVarChar, 100).Value = Environment.UserName;
                    cmd.Parameters.Add("@notes", OdbcType.NVarChar, 4000).Value = "Deleted record:\nEmployee: " + previousRecordDetail;

                    cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }

                if (mainWindow.ShowActivated)
                {
                    mainWindow.update();
                }
                this.Close();
            }

        }
    }
}
