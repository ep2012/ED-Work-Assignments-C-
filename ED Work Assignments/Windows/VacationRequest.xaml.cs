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
    /// Interaction logic for VacationRequest.xaml
    /// </summary>
    public partial class VacationRequest : Window
    {
        public VacationRequest()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you would like request " + dtpStart.Text + " to " + dtpEnd.Text + " off?", "Requesting Time Off", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                if (checkIfValid())
                {
                    Users user = new Users();
                    String id = user.getID(user.getName(Environment.UserName));
                    if (!id.Equals("-1"))
                    {
                        TimeOffSQL.insertTimeOffRequest(id, dtpStart.Text, dtpEnd.Text);
                        Close();
                    }
                    else
                    {
                        dialogResult = MessageBox.Show("There was an error with your username. Please contact a supervisor or system administrator for assistance.", "Error requesting time off", MessageBoxButton.YesNo);
                    }
                }
                else
                {
                    dialogResult = MessageBox.Show("Please fill out the start and end time for your desired time off.","Error requesting time off", MessageBoxButton.YesNo);

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
            else
            {
                return true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
