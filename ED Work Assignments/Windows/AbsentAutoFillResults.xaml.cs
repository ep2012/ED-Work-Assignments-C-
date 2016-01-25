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
    /// Interaction logic for AbsentAutoFillResults.xaml
    /// </summary>
    public partial class AbsentAutoFillResults : Window
    {
        public AbsentAutoFillResults(List <MarkAsAbsentShift> deletedShifts, List <MarkAsAbsentShift> newShifts)
        {
            InitializeComponent();

            DataTable dtNamedStaff = new DataTable();

            dtNamedStaff.Columns.Add(new DataColumn("Full Name", typeof(string)));
            dtNamedStaff.Columns.Add(new DataColumn("Seat", typeof(string)));
            dtNamedStaff.Columns.Add(new DataColumn("Start", typeof(string)));
            dtNamedStaff.Columns.Add(new DataColumn("End", typeof(string)));

            foreach (MarkAsAbsentShift shift in newShifts)
            {
                dtNamedStaff.Rows.Add(new object[] { shift.Name, shift.Seat, shift.Start, shift.End });
            }

            dtaNewShifts.ItemsSource = dtNamedStaff.DefaultView;
            dtaNewShifts.CanUserAddRows = false;
            dtaNewShifts.IsReadOnly = true;

            DataTable dtNamedStaff2 = new DataTable();

            dtNamedStaff2.Columns.Add(new DataColumn("Full Name", typeof(string)));
            dtNamedStaff2.Columns.Add(new DataColumn("Seat", typeof(string)));
            dtNamedStaff2.Columns.Add(new DataColumn("Start", typeof(string)));
            dtNamedStaff2.Columns.Add(new DataColumn("End", typeof(string)));

            foreach (MarkAsAbsentShift shift in deletedShifts)
            {
                dtNamedStaff2.Rows.Add(new object[] { shift.Name, shift.Seat, shift.Start, shift.End });
            }
            dtaDeletedShifts.ItemsSource = dtNamedStaff2.DefaultView;
            dtaDeletedShifts.CanUserAddRows = false;
            dtaDeletedShifts.IsReadOnly = true;
        }
    }
}
