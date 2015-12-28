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

        DateTime start;
        DateTime end;

        String employee;

        object seat;

        int id;

        public NewAssignment(MainWindow main)
        {
            InitializeComponent();

            mainWindow = main;

            setBindings();

            assignmentType = AssignmentType.New;

            lblWorkAssignment.Content = newAssignment;
        }

        public NewAssignment(MainWindow main, DataRowView row, String seat)
        {
            InitializeComponent();

            if (!users.isAdmin(Environment.UserName))
            {
                this.Close();
            }
            else
            {
                this.Title = "ED Update Work Assignment";

                mainWindow = main;

                setBindings();

                cboEmployee.Text = row["Employee"].ToString();
                employee = row["Employee"].ToString();

                cboSeat.Text = seat;

                this.seat = seat;

                dtpEnd.Value = DateTime.Parse(row["End Time"].ToString());
                dtpStart.Value = DateTime.Parse(row["Start Time"].ToString());

                start = DateTime.Parse(row["Start Time"].ToString());
                end = DateTime.Parse(row["End Time"].ToString());

                assignmentType = AssignmentType.Update;

                id = int.Parse(row["Shift Id"].ToString());

                lblWorkAssignment.Content = updateAssignment;

                previousRecordDetail = cboEmployee.Text + ".\nStarting: " + dtpStart.Value + ".\nEnding: " + dtpEnd.Value + ".\nIn Seat " + cboSeat.Text + ".";
            }
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
            save(users.getID(cboEmployee.Text), seats.getID(cboSeat.Text));
        }
        public void save(String employeeID, String seatID)
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

                            cmd.Parameters.Add("@employee", OdbcType.Int).Value = employeeID;
                            cmd.Parameters.Add("@seat", OdbcType.Int).Value = seatID;
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

        private void btnMarkAsAbsent_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you would like to mark "+employee+" as absent?", "Marking "+employee+" as absent", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                //EmployeeScheduleSQL.deleteClocking(id);
                var dialogResult2 = MessageBox.Show("Would you like the schedule to automatically fill the hole?", "Fill hole automatically?", MessageBoxButton.YesNo);

                if (dialogResult2 == MessageBoxResult.Yes)
                {
                    (new ScheduleMaker()).markAsAbsent(start, end, seats.getID(seat.ToString()), id);
                }
            }
        }
    }
}
