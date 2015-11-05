using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public enum SchedulingType {Check, Fill, Part, PartCheck};
    public class Schedule
    {
        Users users = new Users();
        Random random = new Random();
        public Schedule()
        {

        }
        public void scheduleWhoWorks(DateTime start, DateTime end, List<EmployeeShift> employeeShifts, SchedulingType type)
        {
            TimeSpan timeSpan = end.Subtract(start);
            if (type == SchedulingType.Part || type == SchedulingType.PartCheck)
            {
                foreach (EmployeeShift employeeShift in employeeShifts.ToList())
                {
                    foreach (Shift shift in employeeShift.shifts.ToList())
                    {

                        if (shift.startTime == start)
                        {
                            scheduleInBestWorkstation(start, start.Add(shift.shiftTimeSpan), employeeShift, type);
                        }
                    }
                }
            }
            else
            {
                foreach (EmployeeShift employeeShift in employeeShifts.ToList())
                {
                    foreach (Shift shift in employeeShift.shifts.ToList())
                    {

                        if (shift.shiftTimeSpan >= timeSpan && shift.startTime <= start && shift.startTime.Add(shift.shiftTimeSpan) >= end)
                        {
                            scheduleInBestWorkstation(start, end, employeeShift, type);
                        }
                    }
                }
            }
        }

        public void scheduleInBestWorkstation(DateTime start, DateTime end, EmployeeShift employeeShift, SchedulingType type)
        {
            int employee = Convert.ToInt32(employeeShift.employee);
            if (type == SchedulingType.Check)
            {
                if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 1);
                }
                else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 5);
                }
            }
            else if (type == SchedulingType.Fill)
            {
                if (isStationOpen(start, end, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 6);
                }
                else if (isStationOpen(start, end, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 7);
                }
                else if (isStationOpen(start, end, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 3);
                }
                else if (isStationOpen(start, end, 4, 1) && !didEmployeeWorkAtStationLast(start, 4, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 4);
                }
                else if (isStationOpen(start, end, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 6);
                }
                else if (isStationOpen(start, end, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 7);
                }
                else if (isStationOpen(start, end, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 3);
                }
                else if (isStationOpen(start, end, 4, 2))
                {
                    scheduleEmployee(employeeShift, start, end, 4);
                }
                else if (isStationOpen(start, end, 2, 1))
                {
                    scheduleEmployee(employeeShift, start, end, 2);
                }
                else
                {
                    scheduleEmployee(employeeShift, start, start.AddMinutes(30), 10);
                }
            }
            else if (type == SchedulingType.Part || type == SchedulingType.PartCheck)
            {
                if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 1);
                }
                else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 5);
                }
                else if (type == SchedulingType.Part)
                {
                    if (isStationOpen(start, end, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
                    {
                        scheduleEmployee(employeeShift, start, end, 6);
                    }
                    else if (isStationOpen(start, end, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
                    {
                        scheduleEmployee(employeeShift, start, end, 7);
                    }
                    else if (isStationOpen(start, end, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
                    {
                        scheduleEmployee(employeeShift, start, end, 3);
                    }
                    else if (isStationOpen(start, end, 4, 1) && !didEmployeeWorkAtStationLast(start, 4, employee))
                    {
                        scheduleEmployee(employeeShift, start, end, 4);
                    }
                    else if (isStationOpen(start, end, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
                    {
                        scheduleEmployee(employeeShift, start, end, 6);
                    }
                    else if (isStationOpen(start, end, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
                    {
                        scheduleEmployee(employeeShift, start, end, 7);
                    }
                    else if (isStationOpen(start, end, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
                    {
                        scheduleEmployee(employeeShift, start, end, 3);
                    }
                    else if (isStationOpen(start, end, 4, 2))
                    {
                        scheduleEmployee(employeeShift, start, end, 4);
                    }
                    else if (isStationOpen(start, end, 2, 1))
                    {
                        scheduleEmployee(employeeShift, start, end, 2);
                    }
                    else
                    {
                        scheduleEmployee(employeeShift, start, start.AddMinutes(30), 10);
                    }
                }
            }
            
        }

        public bool isStationOpen(DateTime start, DateTime end, int station, int num)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";

            new idMaker(sqlString, checkerList);
            return checkerList.Count < num;
        }

        public int getNumberEmployeesWorking(DateTime start, DateTime end)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A WHERE A.[StartShift] <= '" + start + "' AND A.[EndShift] >= '" + end + "' AND NOT A.Seat = 9 AND NOT A.Seat = 10;";

            new idMaker(sqlString, checkerList);
            return checkerList.Count;
        }
        private bool didEmployeeWorkAtStationLast(DateTime start, int station, int employee)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE A.[EndShift] = '" + start + "' AND A.Seat = " + station + " AND A.[Employee] = " + employee + ";";

            new idMaker(sqlString, checkerList);
            return checkerList.Count > 0;
        }
        public void scheduleEmployee(EmployeeShift employeeShift, DateTime start, DateTime end, int station)
        {
            TimeSpan timeSpan = end.Subtract(start);
            TempScheduler.insert(Convert.ToInt32(employeeShift.employee), start, end, station);

            foreach (Shift shift in employeeShift.shifts.ToList())
            {

                if (shift.shiftTimeSpan >= timeSpan && shift.startTime == start)
                {
                    if (shift.shiftTimeSpan > timeSpan)
                    {
                        Shift newShift = new Shift();
                        newShift.startTime = end;
                        newShift.shiftTimeSpan = shift.shiftTimeSpan.Subtract(timeSpan);
                        employeeShift.shifts.Add(newShift);
                    }
                     
                    employeeShift.shifts.Remove(shift);
                }
                else if (shift.startTime < start && start.Add(timeSpan) <= shift.startTime.Add(shift.shiftTimeSpan))
                {
                    Shift newShift = new Shift();
                    newShift.startTime = shift.startTime;
                    TimeSpan newTimeSpan = start.Subtract(shift.startTime);
                    newShift.shiftTimeSpan = newTimeSpan;

                    employeeShift.shifts.Add(newShift);

                    if (shift.startTime.Add(shift.shiftTimeSpan) > end)
                    {
                        Shift newEndShift = new Shift();
                        newEndShift.startTime = end;
                        newEndShift.shiftTimeSpan = shift.shiftTimeSpan.Subtract(newTimeSpan).Subtract(end.Subtract(start));
                        employeeShift.shifts.Add(newEndShift);
                    }
                    
                    employeeShift.shifts.Remove(shift);
                }
            }
        }

        /*
        private List<object> whoHasVacation(DateTime start, DateTime end)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[EmployeeId] FROM [REVINT].[HEALTHCARE\eliprice].[ED_TimeOff] A WHERE A.[StartTime] <= '" + start + "' AND A.[EndTime] >= '" + end + "';";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        */

        private int getHighestOpenStation(DateTime time)
        {
            if (isStationOpen(time, time, 1, 1))
            {
                return 1;
            }
            else if (isStationOpen(time, time, 5, 1))
            {
                return 5;
            }
            else if (isStationOpen(time, time, 6, 2))
            {
                return 6;
            }
            else if (isStationOpen(time, time, 7, 2))
            {
                return 7;
            }
            else if (isStationOpen(time, time, 3, 2))
            {
                return 3;
            }
            else if (isStationOpen(time, time, 4, 2))
            {
                return 4;
            }
            else if (isStationOpen(time, time, 2, 1))
            {
                return 2;
            }
            return -1;
        }
        private void getOpenShiftRange(DateTime time, int station, out DateTime start, out DateTime end)
        {
            List<object> checkerList = new List<object>();
            List<object> checkerList2 = new List<object>();
            String sqlString = @"SELECT A.[StartShift] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A WHERE A.[StartShift] > '" + time + "' AND A.Seat = " + station + " ORDER BY A.[StartShift];";
            String sqlString2 = @"SELECT A.[EndShift] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A WHERE A.[EndShift] < '" + time + "' AND A.Seat = " + station + " ORDER BY A.[EndShift] DESC;";

            new idMaker(sqlString, checkerList);
            new idMaker(sqlString2, checkerList2);

            if (checkerList2.Count > 0)
                start = DateTime.Parse(checkerList2[0].ToString());
            else
                start = time.AddMinutes(-121);

            if (checkerList.Count > 0)
                end = DateTime.Parse(checkerList[0].ToString());
            else
                end = time.AddMinutes(119);
        }
        public void putHoleInHighestPriorityStation(DateTime time)
        {
            int station = getHighestOpenStation(time);
            if (station != -1)
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                getOpenShiftRange(time, station, out start, out end);
                TempScheduler.insertBlank(start, end, station);
            }
        }
    }
}