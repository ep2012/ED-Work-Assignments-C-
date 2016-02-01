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

            //employeeName = scheduleValues[0].ToString().First() + String.Join("", scheduleValues[0].ToString().Skip(1)).ToLower() + " " + scheduleValues[1].ToString().First() + String.Join("", scheduleValues[1].ToString().Skip(1)).ToLower();
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

            //setWeek1EqualsWeek2();
            setOvernightChecks();

            calculateWeek1Hours();
            calculateWeek2Hours();
        }

        /*
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
        */

        private void setOvernightChecks()
        {
            setOvernightCheckBox(tmSunday1Start, tmSunday1End, chkSunday1Overnight);
            setOvernightCheckBox(tmMonday1Start, tmMonday1End, chkMonday1Overnight);
            setOvernightCheckBox(tmTuesday1Start, tmTuesday1End, chkTuesday1Overnight);
            setOvernightCheckBox(tmWednesday1Start, tmWednesday1End, chkWednesday1Overnight);
            setOvernightCheckBox(tmThursday1Start, tmThursday1End, chkThursday1Overnight);
            setOvernightCheckBox(tmFriday1Start, tmFriday1End, chkFriday1Overnight);
            setOvernightCheckBox(tmSaturday1Start, tmSaturday1End, chkSaturday1Overnight);

            setOvernightCheckBox(tmSunday2Start, tmSunday2End, chkSunday2Overnight);
            setOvernightCheckBox(tmMonday2Start, tmMonday2End, chkMonday2Overnight);
            setOvernightCheckBox(tmTuesday2Start, tmTuesday2End, chkTuesday2Overnight);
            setOvernightCheckBox(tmWednesday2Start, tmWednesday2End, chkWednesday2Overnight);
            setOvernightCheckBox(tmThursday2Start, tmThursday2End, chkThursday2Overnight);
            setOvernightCheckBox(tmFriday2Start, tmFriday2End, chkFriday2Overnight);
            setOvernightCheckBox(tmSaturday2Start, tmSaturday2End, chkSaturday2Overnight);
        }
        private void setTimeIfValid(String time, Xceed.Wpf.Toolkit.TimePicker picker)
        {
            DateTime timeToSet = new DateTime();
            if (DateTime.TryParse(time, out timeToSet))
            {
                picker.Value = timeToSet;
            }
        }
        private void calculateWeek1Hours()
        {
            double numHours = 0.0;
            numHours += getNumHoursWorkedInDay(tmSunday1Start, tmSunday1End);
            numHours += getNumHoursWorkedInDay(tmMonday1Start, tmMonday1End);
            numHours += getNumHoursWorkedInDay(tmTuesday1Start, tmTuesday1End);
            numHours += getNumHoursWorkedInDay(tmWednesday1Start, tmWednesday1End);
            numHours += getNumHoursWorkedInDay(tmThursday1Start, tmThursday1End);
            numHours += getNumHoursWorkedInDay(tmFriday1Start, tmFriday1End);
            numHours += getNumHoursWorkedInDay(tmSaturday1Start, tmSaturday1End);

            lblWeek1Hours.Content = "Week 1 Hours: " + numHours;
        }
        private void calculateWeek2Hours()
        {
            double numHours = 0.0;
            numHours += getNumHoursWorkedInDay(tmSunday2Start, tmSunday2End);
            numHours += getNumHoursWorkedInDay(tmMonday2Start, tmMonday2End);
            numHours += getNumHoursWorkedInDay(tmTuesday2Start, tmTuesday2End);
            numHours += getNumHoursWorkedInDay(tmWednesday2Start, tmWednesday2End);
            numHours += getNumHoursWorkedInDay(tmThursday2Start, tmThursday2End);
            numHours += getNumHoursWorkedInDay(tmFriday2Start, tmFriday2End);
            numHours += getNumHoursWorkedInDay(tmSaturday2Start, tmSaturday2End);

            lblWeek2Hours.Content = "Week 2 Hours: " + numHours;
        }
        private double getNumHoursWorkedInDay(Xceed.Wpf.Toolkit.TimePicker startTP, Xceed.Wpf.Toolkit.TimePicker endTP)
        {
            DateTime start;
            DateTime end;
            if (DateTime.TryParse(startTP.Value.ToString(), out start) && DateTime.TryParse(endTP.Value.ToString(), out end))
            {
                DateTime starttime = DateTime.Parse(DateTime.Today.ToShortDateString() + " " + start.TimeOfDay);

                if (start.TimeOfDay > end.TimeOfDay)
                {
                    DateTime endtime = DateTime.Parse(DateTime.Today.AddDays(1).ToShortDateString() + " " + end.TimeOfDay);
                    return endtime.Subtract(starttime).TotalMinutes / 60;
                }
                else
                {
                    DateTime endtime = DateTime.Parse(DateTime.Today.ToShortDateString() + " " + end.TimeOfDay);
                    return endtime.Subtract(starttime).TotalMinutes / 60;
                }
            }
            else
            {
                return 0.0;
            }
        }

        private void setOvernightCheckBox(Xceed.Wpf.Toolkit.TimePicker startTP, Xceed.Wpf.Toolkit.TimePicker endTP, CheckBox overnightChk)
        {
            DateTime start;
            DateTime end;
            if (DateTime.TryParse(startTP.Value.ToString(), out start) && DateTime.TryParse(endTP.Value.ToString(), out end))
            {
                overnightChk.IsChecked = start.TimeOfDay > end.TimeOfDay;
            }
        }

        private void btnSaveSchedule_Click(object sender, RoutedEventArgs e)
        {
            EmployeeInformationSQL.updateSchedule(tmSunday1Start.Value, tmSunday1End.Value, chkSunday1Overnight.IsChecked,
                tmMonday1Start.Value, tmMonday1End.Value, chkMonday1Overnight.IsChecked,
                tmTuesday1Start.Value, tmTuesday1End.Value, chkTuesday1Overnight.IsChecked,
                tmWednesday1Start.Value, tmWednesday1End.Value, chkWednesday1Overnight.IsChecked,
                tmThursday1Start.Value, tmThursday1End.Value, chkThursday1Overnight.IsChecked,
                tmFriday1Start.Value, tmFriday1End.Value, chkFriday1Overnight.IsChecked,
                tmSaturday1Start.Value, tmSaturday1End.Value, chkSaturday1Overnight.IsChecked,
                tmSunday2Start.Value, tmSunday2End.Value, chkSunday2Overnight.IsChecked,
                tmMonday2Start.Value, tmMonday2End.Value, chkMonday2Overnight.IsChecked,
                tmTuesday2Start.Value, tmTuesday2End.Value, chkTuesday2Overnight.IsChecked,
                tmWednesday2Start.Value, tmWednesday2End.Value, chkWednesday2Overnight.IsChecked,
                tmThursday2Start.Value, tmThursday2End.Value, chkThursday2Overnight.IsChecked,
                tmFriday2Start.Value, tmFriday2End.Value, chkFriday2Overnight.IsChecked,
                tmSaturday2Start.Value, tmSaturday2End.Value, chkSaturday2Overnight.IsChecked, 
                isFullTime(), 
                id);

            ManageEmployees win = new ManageEmployees(ManageEmployeeType.Schedule);

            win.Left = Left;
            win.Top = Top;

            win.Show();

            this.Close();
            
        }
        private Boolean isFullTime()
        {
            double numHours = 0.0;
            numHours += getNumHoursWorkedInDay(tmSunday1Start, tmSunday1End);
            numHours += getNumHoursWorkedInDay(tmMonday1Start, tmMonday1End);
            numHours += getNumHoursWorkedInDay(tmTuesday1Start, tmTuesday1End);
            numHours += getNumHoursWorkedInDay(tmWednesday1Start, tmWednesday1End);
            numHours += getNumHoursWorkedInDay(tmThursday1Start, tmThursday1End);
            numHours += getNumHoursWorkedInDay(tmFriday1Start, tmFriday1End);
            numHours += getNumHoursWorkedInDay(tmSaturday1Start, tmSaturday1End);
            numHours += getNumHoursWorkedInDay(tmSunday2Start, tmSunday2End);
            numHours += getNumHoursWorkedInDay(tmMonday2Start, tmMonday2End);
            numHours += getNumHoursWorkedInDay(tmTuesday2Start, tmTuesday2End);
            numHours += getNumHoursWorkedInDay(tmWednesday2Start, tmWednesday2End);
            numHours += getNumHoursWorkedInDay(tmThursday2Start, tmThursday2End);
            numHours += getNumHoursWorkedInDay(tmFriday2Start, tmFriday2End);
            numHours += getNumHoursWorkedInDay(tmSaturday2Start, tmSaturday2End);

            return numHours > 72;
        }
        private void Week1_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            calculateWeek1Hours();
        }

        private void Week2_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            calculateWeek2Hours();
        }

        private void btnGetHours_Click(object sender, RoutedEventArgs e)
        {
            calculateWeek1Hours();
            calculateWeek2Hours();
        }
    }
}
