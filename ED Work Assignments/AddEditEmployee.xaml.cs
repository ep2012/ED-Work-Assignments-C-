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
    /// Interaction logic for AddEditEmployee.xaml
    /// </summary>
    public partial class AddEditEmployee : Window
    {
        enum AdminStatus { Admin, GeneralUser };
        enum AssignmentType { New, Update };

        AdminStatus userAdminStatus;
        AssignmentType assignmentType;

        String makeAdmin = "Make Admin";
        String removeAdmin = "Remove From Admins";

        String addUser = "Add";
        String updateUser = "Update";
        Users users = new Users();
        Roles roles = new Roles();

        String name;

        public AddEditEmployee()
        {
            InitializeComponent();

            assignmentType = AssignmentType.New;

            userAdminStatus = AdminStatus.GeneralUser;

            btnAddUpdate.Content = addUser;
            btnDeleteEmployee.Visibility = Visibility.Hidden;

            lblAddEdit.Content = "Add Employee";

            addButtonBindings();
        }

        public AddEditEmployee(DataRowView row)
        {
            InitializeComponent();

            name = row["First Name"].ToString() + " " + row["Last Name"].ToString();

            txtFirstName.Text = row["First Name"].ToString();
            txtLastName.Text = row["Last Name"].ToString();
            txtAddress1.Text = row["Address 1"].ToString();
            txtAddress2.Text = row["Address 2"].ToString();
            txtCity.Text = row["City"].ToString();
            txtState.Text = row["State"].ToString();
            txtZip.Text = row["Zip"].ToString();
            txtPhone.Text = row["Phone"].ToString();
            txtEmail.Text = row["Email"].ToString();
            txtHealthcareID.Text = row["Healthcare ID"].ToString();

            assignmentType = AssignmentType.Update;

            btnAddUpdate.Content = updateUser;

            lblAddEdit.Content = "Edit Employee";

            addButtonBindings();
        }

        private void addButtonBindings()
        {
            Binding binding1 = new Binding();

            binding1.Source = roles;
            cboRole.SetBinding(ListBox.ItemsSourceProperty, binding1);
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (assignmentType == AssignmentType.New)
            {
                add();
            }
            else
            {
                update();
            }

            ManageEmployees win = new ManageEmployees();

            win.Show();

            this.Close();
        }

        private void add()
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].employee_insert(?,?,?,?,?,?,?,?,?,?,?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@FirstName", OdbcType.NVarChar, 50).Value = txtFirstName.Text;
                cmd.Parameters.Add("@LastName", OdbcType.NVarChar, 50).Value = txtLastName.Text;
                cmd.Parameters.Add("@Role", OdbcType.Int).Value = roles.getID(cboRole.Text);
                cmd.Parameters.Add("@Address1", OdbcType.NVarChar, 100).Value = txtAddress1.Text;
                cmd.Parameters.Add("@Address2", OdbcType.NVarChar, 100).Value = txtAddress2.Text;
                cmd.Parameters.Add("@City", OdbcType.NVarChar, 50).Value = txtCity.Text;
                cmd.Parameters.Add("@State", OdbcType.NVarChar, 50).Value = txtState.Text;
                cmd.Parameters.Add("@Zip", OdbcType.NVarChar, 50).Value = txtZip.Text;
                cmd.Parameters.Add("@Phone", OdbcType.NVarChar, 50).Value = txtPhone.Text;
                cmd.Parameters.Add("@Email", OdbcType.NVarChar, 100).Value = txtEmail.Text;
                cmd.Parameters.Add("@UserName", OdbcType.NVarChar, 50).Value = txtHealthcareID.Text;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }

        private void update()
        {

        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var dialogBox = MessageBox.Show("Are you sure you would like to delete this employee?\n\nEmployee '" + name + "' is about to be deleted.", "Delete Employee", MessageBoxButton.YesNo);
            if (dialogBox == MessageBoxResult.Yes)
            {
                String id = users.getID(name);

                String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

                using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                {
                    //open OdbcConnection object
                    dbConnection.Open();

                    OdbcCommand cmd = new OdbcCommand();

                    cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].delete_employee(?)}";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = dbConnection;

                    cmd.Parameters.Add("@Id", OdbcType.Int).Value = id;

                    cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }
                
                ManageEmployees win = new ManageEmployees();
                win.Show();

                this.Close();
            }
        }

    }
}
