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
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressBar : Window
    {

        private int lastUsedRow;

        public ProgressBar(int lastRow)
        {
            InitializeComponent();
            lastUsedRow = lastRow;

            progBar.Maximum = lastRow;
            progBar.Value = 0;

            this.Show();
        }

        public void updateProg(int row)
        {
            progBar.Value = row;
            lblProgress.Content = "Progress: " + row.ToString() + "/" + lastUsedRow.ToString();

            if (progBar.Maximum == row)
            {
                Close();
            }
        }
    }
}
