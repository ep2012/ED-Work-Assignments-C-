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
    /// Interaction logic for EditEmployeeSchedule.xaml
    /// </summary>
    public partial class EditEmployeeSchedule : Window
    {
        object id;
        List<object> scheduleValues;
        String employeeName;
        public EditEmployeeSchedule(object empid)
        {
            InitializeComponent();
            id = empid;
            scheduleValues = new List<object>();
            setWindow();
            Title = employeeName + "'s BiWeekly Schedule";
        }
        public void setWindow()
        {
            EmployeeScheduleSQL.getSchedules(id, out scheduleValues);

            employeeName = scheduleValues[0].ToString() + " " + scheduleValues[1].ToString();

            setTimeIfValid(scheduleValues[2].ToString(), tmSunday1Start);
            setTimeIfValid(scheduleValues[3].ToString(), tmSunday1End);

            setTimeIfValid(scheduleValues[5].ToString(), tmMonday1Start);
            setTimeIfValid(scheduleValues[6].ToString(), tmMonday1End);

            setTimeIfValid(scheduleValues[8].ToString(), tmTuesday1Start);
            setTimeIfValid(scheduleValues[9].ToString(), tmTuesday1End);

            setTimeIfValid(scheduleValues[11].ToString(), tmWednesday1Start);
            setTimeIfValid(scheduleValues[12].ToString(), tmWednesday1End);

            setTimeIfValid(scheduleValues[14].ToString(), tmThursday1Start);
            setTimeIfValid(scheduleValues[15].ToString(), tmThursday1End);

            setTimeIfValid(scheduleValues[17].ToString(), tmFriday1Start);
            setTimeIfValid(scheduleValues[18].ToString(), tmFriday1End);

            setTimeIfValid(scheduleValues[20].ToString(), tmSaturday1Start);
            setTimeIfValid(scheduleValues[21].ToString(), tmSaturday1End);

            setTimeIfValid(scheduleValues[23].ToString(), tmSunday2Start);
            setTimeIfValid(scheduleValues[24].ToString(), tmSunday2End);

            setTimeIfValid(scheduleValues[26].ToString(), tmMonday2Start);
            setTimeIfValid(scheduleValues[27].ToString(), tmMonday2End);

            setTimeIfValid(scheduleValues[29].ToString(), tmTuesday2Start);
            setTimeIfValid(scheduleValues[30].ToString(), tmTuesday2End);

            setTimeIfValid(scheduleValues[32].ToString(), tmWednesday2Start);
            setTimeIfValid(scheduleValues[33].ToString(), tmWednesday2End);

            setTimeIfValid(scheduleValues[35].ToString(), tmThursday2Start);
            setTimeIfValid(scheduleValues[36].ToString(), tmThursday2End);

            setTimeIfValid(scheduleValues[38].ToString(), tmFriday2Start);
            setTimeIfValid(scheduleValues[39].ToString(), tmFriday2End);

            setTimeIfValid(scheduleValues[41].ToString(), tmSaturday2Start);
            setTimeIfValid(scheduleValues[42].ToString(), tmSaturday2End);

            setWeek1EqualsWeek2();
            
        }

        private void setWeek1EqualsWeek2()
        {
            if (tmSunday1Start.Value == tmSunday2Start.Value
                            && tmSunday1End.Value == tmSunday2End.Value
                            && tmMonday1Start.Value == tmMonday2Start.Value
                            && tmMonday1End.Value == tmMonday2End.Value
                            && tmTuesday1Start.Value == tmTuesday2Start.Value
                            && tmTuesday1End.Value == tmTuesday2End.Value
                            && tmWednesday1Start.Value == tmWednesday2Start.Value
                            && tmWednesday1End.Value == tmWednesday2End.Value
                            && tmThursday1Start.Value == tmThursday2Start.Value
                            && tmThursday1End.Value == tmThursday2End.Value
                            && tmFriday1Start.Value == tmFriday2Start.Value
                            && tmFriday1End.Value == tmFriday2End.Value
                            && tmSaturday1Start.Value == tmSaturday2Start.Value
                            && tmSaturday1End.Value == tmSaturday2End.Value
                )
            {
                chkWeek1EqualsWeek2.IsChecked = true;
            }
            else
            {
                chkWeek1EqualsWeek2.IsChecked = false;
            }
        }
        private void setTimeIfValid(String time, Xceed.Wpf.Toolkit.TimePicker picker)
        {
            DateTime timeToSet = new DateTime();
            if (DateTime.TryParse(time, out timeToSet))
            {
                picker.Value = timeToSet;
            }
        }
    }
}
