using System;
using System.Collections.Generic;
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
    /// Interaction logic for NewTimeOff.xaml
    /// </summary>
    public partial class NewTimeOff : Window
    {
        public NewTimeOff()
        {
            InitializeComponent();

            Users users = new Users();

            Binding binding1 = new Binding();

            binding1.Source = users;
            cboEmployee.SetBinding(ListBox.ItemsSourceProperty, binding1);
            Binding binding2 = new Binding();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you would like take " + dtpStart.Text + " to " + dtpEnd.Text + " off for " + cboEmployee.Text + "?", "Requesting Time Off", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                if (checkIfValid())
                {
                    Users user = new Users();

                    (new TimeOff()).insertTimeOff(user.getID(cboEmployee.Text), dtpStart.Text, dtpEnd.Text);
                    Close();
                }
                else
                {
                    dialogResult = MessageBox.Show("Please fill out the employee, start, end time for the desired time off.", "Error requesting time off", MessageBoxButton.YesNo);
                }
            }
        }

        private bool checkIfValid()
        {
            if (dtpEnd.Text == null)
            {
                return false;
            }
            else if (dtpStart.Text == null)
            {
                return false;
            }
            else if (cboEmployee.SelectedIndex == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnTineOffRequests_Click(object sender, RoutedEventArgs e)
        {
            EmployeeTimeOff win = new EmployeeTimeOff();

            win.Left = Left;
            win.Top = Top;

            win.Show();

            Close();
        }
    }
}
