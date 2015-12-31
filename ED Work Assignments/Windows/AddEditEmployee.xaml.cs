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

        AssignmentType assignmentType;

        String addUser = "Add";
        String updateUser = "Update";
        Users users = new Users();
        Roles roles = new Roles();

        String name;
        String username;
        String id;

        public AddEditEmployee()
        {
            InitializeComponent();
            this.Title = "ED Add Employee";

            assignmentType = AssignmentType.New;

            btnAddUpdate.Content = addUser;
            btnDeleteEmployee.Visibility = Visibility.Hidden;
            btnEditSchedule.Visibility = Visibility.Hidden;

            lblAddEdit.Content = "Add Employee";

            addButtonBindings();
        }

        public AddEditEmployee(DataRowView row)
        {
            InitializeComponent();
            this.Title = "ED Edit Employee"; 

            name = row["First Name"].ToString() + " " + row["Last Name"].ToString();
            username = row["Healthcare ID"].ToString();

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

            cboRole.SelectedIndex = int.Parse(roles.getID(row["Title"].ToString())) - 1;

            id = row["Id"].ToString();

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
            if (checkEssentials())
            {
                if (assignmentType == AssignmentType.New)
                {
                    if (users.userNameCanBeCreated(txtFirstName.Text + " " + txtLastName.Text))
                    {
                        if (users.userIDCanBeCreated(txtHealthcareID.Text))
                        {
                            add();
                            openMain();
                        }
                        else
                        {
                            var dialogBox = MessageBox.Show("An employee with the Healthcare ID '" + txtHealthcareID.Text + "' already exists.", 
                                "Error Creating Employee", MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        var dialogBox = MessageBox.Show("An employee with the name '" + txtFirstName.Text + " " + txtLastName.Text + "' already exists.", 
                            "Error Creating Employee", MessageBoxButton.OK);
                        if (users.userIDCanBeCreated(txtHealthcareID.Text))
                        {
                            var dialogBox2 = MessageBox.Show("An employee with the Healthcare ID '" + txtHealthcareID.Text + "' already exists.", 
                                "Error Creating Employee", MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    bool canUpdate = true;
                    if (txtFirstName.Text + " " + txtLastName.Text != name)
                    {
                        canUpdate = users.userNameCanBeCreated(txtFirstName.Text + " " + txtLastName.Text);
                        if (!canUpdate)
                        {
                            var dialogBox = MessageBox.Show("An employee with the name '" + txtFirstName.Text + " " + txtLastName.Text + "' already exists.", "Error Updating Employee", MessageBoxButton.OK);

                            if (users.userIDCanBeCreated(txtHealthcareID.Text))
                            {
                                var dialogBox2 = MessageBox.Show("An employee with the Healthcare ID '" + txtHealthcareID.Text + "' already exists.", "Error Updating Employee", MessageBoxButton.OK);
                            }
                        }
                    }
                    if (txtHealthcareID.Text != username && canUpdate)
                    {
                        canUpdate = users.userIDCanBeCreated(txtHealthcareID.Text);
                        if (!canUpdate)
                        {
                            var dialogBox = MessageBox.Show("An employee with the Healthcare ID '" + txtHealthcareID.Text + "' already exists.", "Error Updating Employee", MessageBoxButton.OK);
                        }
                    }
                    if (canUpdate)
                    {
                        update();
                        openMain();
                    }
                }
            }
            else
            {
                var dialogBox = MessageBox.Show("The employee name and role fields are required.", "Error", MessageBoxButton.OK);
            }
        }

        private void openMain()
        {
            ManageEmployees win = new ManageEmployees(ManageEmployeeType.Information);

            win.Show();

            this.Close();
        }

        private void add()
        {
            EmployeeInformationSQL.add(txtFirstName.Text, txtLastName.Text, cboRole.Text, txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtState.Text, txtZip.Text, txtPhone.Text, txtEmail.Text, txtHealthcareID.Text);
        }

        private void update()
        {
            EmployeeInformationSQL.update(txtFirstName.Text, txtLastName.Text, cboRole.Text, txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtState.Text, txtZip.Text, txtPhone.Text, txtEmail.Text, txtHealthcareID.Text, id);
        }

        private bool checkEssentials()
        {
            if (txtFirstName.Text == "" || cboRole.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var dialogBox = MessageBox.Show("Employee '" + name + "' is about to be deleted.\n"+
                "Are you sure you would like to delete this employee?\n\n"+
                "This can only be undone by contacting the system administrator.", 
                "Delete Employee", MessageBoxButton.YesNo);
            if (dialogBox == MessageBoxResult.Yes)
            {
                id = users.getID(name);

                EmployeeInformationSQL.delete(txtFirstName.Text, txtLastName.Text, cboRole.Text, txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtState.Text, txtZip.Text, txtPhone.Text, txtEmail.Text, txtHealthcareID.Text, id);
                

                ManageEmployees win = new ManageEmployees(ManageEmployeeType.Information);
                win.Show();

                this.Close();
            }
        }

        private void btnEditSchedule_Click(object sender, RoutedEventArgs e)
        {
            EditEmployeeSchedule win = new EditEmployeeSchedule(id);
            win.Show();
            win.Left = Left;
            win.Top = Top;

            Close();
        }

    }
}
