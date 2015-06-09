using System;
using System.Collections.Generic;
using System.Data;
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
        public AddEditEmployee()
        {
            InitializeComponent();
        }

        public AddEditEmployee(DataRowView row)
        {
            InitializeComponent();

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
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add()
        {

        }

        private void update()
        {

        }
    }
}
