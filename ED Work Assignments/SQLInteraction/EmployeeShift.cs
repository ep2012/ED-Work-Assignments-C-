using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    
    public class EmployeeShift
    {
        public List<Shift> shifts;
        public object employee;

        public EmployeeShift(object emp)
        {
            employee = emp;
            shifts = new List<Shift>();
        }
        
        public static void EmployeeShiftCreator(String sqlCmdIDStartEndDay, List<EmployeeShift> comprehensiveEmployeeShifts, DateTime date)
        {
            object[] objID = new object[40];

            using (OdbcConnection connectionID = new OdbcConnection(Connection.cxnString))
            {
                OdbcCommand commandID = new OdbcCommand(sqlCmdIDStartEndDay, connectionID);

                connectionID.Open();

                OdbcDataReader reader = commandID.ExecuteReader();

                while (reader.Read())
                {
                    int numCols = reader.GetValues(objID);

                    if (numCols == 4)
                    {
                        EmployeeShift employeeShift = new EmployeeShift(((object)objID[0]));
                        Shift shift = new Shift();

                        bool isTrue = false; 
                        Boolean.TryParse(((object)objID[3]).ToString(), out isTrue);

                        if (isTrue)
                        {
                            DateTime start = DateTime.Parse(date.ToShortDateString() + " " +((object)objID[1]).ToString());
                            DateTime end = DateTime.Parse(date.AddDays(1).ToShortDateString() + " " + ((object)objID[2]).ToString());
                            shift.shiftTimeSpan = end.Subtract(start);
                            shift.startTime = start;
                        }
                        else
                        {
                            DateTime start = DateTime.Parse(date.ToShortDateString() + " " + ((object)objID[1]).ToString());
                            DateTime end = DateTime.Parse(date.ToShortDateString() + " " + ((object)objID[2]).ToString());
                            shift.shiftTimeSpan = end.Subtract(start);
                            shift.startTime = start;
                        }

                        employeeShift.shifts.Add(shift);
                        removeAlreadyWorkedAndVacation(employeeShift, date);
                        comprehensiveEmployeeShifts.Add(employeeShift);

                    }
                }
            }
        }
        private static void removeAlreadyWorkedAndVacation(EmployeeShift employeeShift, DateTime date)
        {

            //remove already worked shifts
            String strShifts = "SELECT StartShift, EndShift FROM REVINT.dbo.ED_Shifts " +
                "WHERE Employee = '" + employeeShift.employee + "' AND StartShift >= '" + date.ToShortDateString() + "' AND EndShift <= '" + date.AddDays(2).ToShortDateString() + "' AND Seat <> 9;";
            removeMachine(strShifts, employeeShift);

            //remove vacation time
            String strVacation = @"SELECT StartTime, EndTime FROM REVINT.[healthcare\eliprice].ED_TimeOff " +
                "WHERE EmployeeId = '" + employeeShift.employee + "' AND StartTime >= '" + date.ToShortDateString() + "' AND EndTime <= '" + date.AddDays(2).ToShortDateString() + "';";
            removeMachine(strVacation, employeeShift);

        }

        private static void removeMachine(String str, EmployeeShift employeeShift)
        {
            object[] objID = new object[50];

            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection connectionID = new OdbcConnection(cxnString))
            {
                OdbcCommand commandID = new OdbcCommand(str, connectionID);

                connectionID.Open();

                OdbcDataReader reader = commandID.ExecuteReader();

                while (reader.Read())
                {
                    int numCols = reader.GetValues(objID);

                    if (numCols >= 2)
                    {
                        DateTime start = new DateTime();
                        DateTime end = new DateTime();
                        if (DateTime.TryParse(objID[0].ToString(), out start) && DateTime.TryParse(objID[1].ToString(), out end))
                        {
                            tryRemoveShift(employeeShift, start, end);

                            if(employeeShift.shifts.Count > 0)
                            {
                                employeeShift = employeeShift;
                            }
                        }
                    }
                }
            }
        }
        private static void tryRemoveShift(EmployeeShift employeeShift, DateTime start, DateTime end)
        {
            foreach (Shift shift in employeeShift.shifts.ToList())
            {
                //starts at beginning, ends in middle
                // | ><
                if ((start == shift.startTime && end <= shift.startTime.Add(shift.shiftTimeSpan)))
                {
                    TimeSpan newTimeSpan = shift.startTime.Add(shift.shiftTimeSpan).Subtract(end);

                    Shift newShift = new Shift();

                    newShift.shiftTimeSpan = newTimeSpan;

                    newShift.startTime = end;

                    if (newTimeSpan.Milliseconds != 0)
                    {
                        employeeShift.shifts.Add(newShift);
                    }
                    employeeShift.shifts.Remove(shift);
                }
                //starts in middle, ends at end
                // >< |
                else if ((start > shift.startTime && end == shift.startTime.Add(shift.shiftTimeSpan)))
                {
                    TimeSpan newTimeSpan = start.Subtract(shift.startTime);

                    Shift newShift = new Shift();

                    newShift.shiftTimeSpan = newTimeSpan;

                    newShift.startTime = shift.startTime;

                    if (newTimeSpan.Ticks != 0)
                    {
                        employeeShift.shifts.Add(newShift);
                    }
                    employeeShift.shifts.Remove(shift);
                }
                //lies within timespan
                // >< ><
                else if ((start > shift.startTime && end < shift.startTime.Add(shift.shiftTimeSpan)))
                {
                    TimeSpan newTimeSpan = start.Subtract(shift.startTime);

                    Shift newShift = new Shift();

                    newShift.shiftTimeSpan = newTimeSpan;

                    newShift.startTime = shift.startTime;

                    if (newTimeSpan.Ticks != 0)
                    {
                        employeeShift.shifts.Add(newShift);
                    }

                    TimeSpan newTimeSpan2 = shift.startTime.Add(shift.shiftTimeSpan).Subtract(end);

                    Shift newShift2 = new Shift();

                    newShift2.shiftTimeSpan = newTimeSpan2;

                    newShift2.startTime = end;

                    if (newTimeSpan2.Ticks != 0)
                    {
                        employeeShift.shifts.Add(newShift2);
                    }

                    employeeShift.shifts.Remove(shift);
                }
                //starts before
                // < ><
                else if ((start < shift.startTime && end >= shift.startTime))
                {
                    TimeSpan newTimeSpan = shift.startTime.Add(shift.shiftTimeSpan).Subtract(end);

                    Shift newShift = new Shift();

                    newShift.shiftTimeSpan = newTimeSpan;

                    newShift.startTime = end;

                    if (newTimeSpan.Ticks != 0)
                    {
                        employeeShift.shifts.Add(newShift);
                    }
                    employeeShift.shifts.Remove(shift);
                }
                //ends after finished
                // >< >
                else if ((start <= shift.startTime.Add(shift.shiftTimeSpan) && end > shift.startTime.Add(shift.shiftTimeSpan)))
                {
                    TimeSpan newTimeSpan = start.Subtract(shift.startTime);

                    Shift newShift = new Shift();

                    newShift.shiftTimeSpan = newTimeSpan;

                    newShift.startTime = shift.startTime;

                    if (newTimeSpan.Ticks != 0)
                    {
                        employeeShift.shifts.Add(newShift);
                    }
                    employeeShift.shifts.Remove(shift);
                }
            }
        }
    }   
}
