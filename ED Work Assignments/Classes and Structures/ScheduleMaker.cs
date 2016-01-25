using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ED_Work_Assignments
{
    public class ScheduleMaker
    {
        private Random random = new Random();
        private Users users = new Users();
        private static Random rng = new Random();
        private TimeSpan JetPedsStart = TimeSpan.Parse("12:00");

        //For Unit Testing and marking as absent
        public ScheduleMaker()
        {

        }

        public ScheduleMaker(DateTime start, DateTime end, SchedulingMode schedulingMode)
        {
            if (start != null && end != null)
            {
                DateTime scheduleStartTime = new DateTime();
                scheduleStartTime = DateTime.Parse("9/27/2015");
                DateTime day = start;
                int proglength = Convert.ToInt32((end - start).TotalDays);
                ProgressBar progBar = new ProgressBar(proglength);

                progBar.updateProg(Convert.ToInt32((day - start).TotalDays));
                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                          new Action(delegate { }));

                while (day < end)
                {
                    String schedulestr = "";
                    if (day.DayOfWeek == DayOfWeek.Sunday)
                        schedulestr = "sunday";
                    else if (day.DayOfWeek == DayOfWeek.Monday)
                        schedulestr = "monday";
                    else if (day.DayOfWeek == DayOfWeek.Tuesday)
                        schedulestr = "tuesday";
                    else if (day.DayOfWeek == DayOfWeek.Wednesday)
                        schedulestr = "wednesday";
                    else if (day.DayOfWeek == DayOfWeek.Thursday)
                        schedulestr = "thursday";
                    else if (day.DayOfWeek == DayOfWeek.Friday)
                        schedulestr = "friday";
                    else if (day.DayOfWeek == DayOfWeek.Saturday)
                        schedulestr = "saturday";

                    if ((day - scheduleStartTime).TotalDays % 14 < 7)
                        schedulestr += "1";
                    else
                        schedulestr += "2";

                    scheduleSupervisors(day, schedulestr);

                    List<EmployeeShift> employeeShifts = new List<EmployeeShift>();

                    EmployeeShift.EmployeeShiftCreator("SELECT ID, cast(" + schedulestr + "time as time), cast(" + schedulestr + "timeend as time), " + schedulestr + "day FROM REVINT.[healthcare\\eliprice].ED_Employees WHERE " + schedulestr + "timeend IS NOT NULL AND " + schedulestr + "timeend IS NOT NULL ORDER BY NEWID()", employeeShifts, day);


                    if (schedulingMode == SchedulingMode.Type1)
                    {
                        createSchedule(day, employeeShifts);
                    }
                    else
                    {
                        createSchedule2(day, employeeShifts);
                    }
                    day = day.AddDays(1);

                    progBar.updateProg(Convert.ToInt32((day - start).TotalDays));
                    Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                        new Action(
                            delegate {
                            }
                        )
                    );
                }
            }
        }

        public void createSchedule(DateTime day, List<EmployeeShift> employeeShifts)
        {
            DateTime startTime = day;
            DateTime goaldate = startTime.AddHours(36);
            DateTime endTime;
            
            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 14));

                scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.Check);
                scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.PartCheck);

                //increment time
                startTime = startTime.AddMinutes(30);
            }
            
            
            //reset startTime
            startTime = day;

            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 14));

                scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.Fill);
                scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.Part);

                //increment time
                startTime = startTime.AddMinutes(30);
            }

            doMinStaffing(day);
        }

        public void createSchedule2(DateTime day, List<EmployeeShift> employeeShifts)
        {
            DateTime startTime = day;
            DateTime goaldate = startTime.AddHours(36);
            DateTime endTime;

            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 14));

                scheduleWhoWorks2(startTime, endTime, employeeShifts, SchedulingType.Check);

                //increment time
                startTime = startTime.AddMinutes(30);
            }


            //reset startTime
            startTime = day;

            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 14));

                scheduleWhoWorks2(startTime, endTime, employeeShifts, SchedulingType.Fill);

                //increment time
                startTime = startTime.AddMinutes(30);
            }

            doMinStaffing(day);
        }

        public void doMinStaffing(DateTime day)
        {
            String sqlStringFullStaffing = "WITH StaffingReport AS (SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            " LEFT JOIN [REVINT].[dbo].[ED_Shifts] Shifts" +
                            " ON NOT Shifts.Seat = 9 AND CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing" +
                            " UNION ALL" +
                            " SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            @" LEFT JOIN [REVINT].[healthcare\eliprice].[ED_ScheduleMakerShifts] Shifts" +
                            " ON NOT Shifts.Seat = 9 AND CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing)" +
                            " SELECT [Time Slot], [Minimum Staffing Requirement], SUM ([Amount Staffed]) AS [Amount Staffed]" +
                            "  FROM StaffingReport WHERE [Amount Staffed] >= [Minimum Staffing Requirement] GROUP BY [Time Slot], [Minimum Staffing Requirement] ORDER BY [Time Slot]";

            List<object> fullStaffing = new List<object>();
            new idMaker(sqlStringFullStaffing, fullStaffing, "");

            List<object> minStaffingSlots = new List<object>();
            String sqlString = "SELECT TimeSlot FROM [REVINT].[dbo].[ED_Staffing]";

            new idMaker(sqlString, minStaffingSlots, "");

            List<object> minStaffingList = minStaffingSlots.Except(fullStaffing).ToList();

            while (minStaffingList.Count > 0)
            {
                foreach (object date in minStaffingList)
                {
                    putHoleInHighestPriorityStation(DateTime.Parse(day.ToShortDateString() + " " + date.ToString()));
                }

                minStaffingSlots = new List<object>();
                new idMaker(sqlString, minStaffingSlots, "");

                fullStaffing = new List<object>();
                new idMaker(sqlStringFullStaffing, fullStaffing, "");

                minStaffingList = minStaffingSlots.Except(fullStaffing).ToList();
            }
        }

        public void scheduleSupervisors(DateTime date, String day)
        {
            List<SupervisorClocking> dayList = new List<SupervisorClocking>();
            List<SupervisorClocking> nightList = new List<SupervisorClocking>();
            String sqlStringNight = @"SELECT A.[Id], cast(A." + day + @"time as time), cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (A." + day + "day = 'True') AND (A.Role = 3 OR A.Role = 3) AND (A." + day + @"timeend IS NOT NULL) ORDER BY NEWID();";
            String sqlStringDay = @"SELECT A.[Id], cast(A." + day + @"time as time), cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (A.Role = 3) AND (A." + day + @"timeend IS NOT NULL) ORDER BY NEWID();";

            new idMaker(sqlStringDay, dayList);
            new idMaker(sqlStringNight, nightList);

            dayList = dayList.Except(nightList).ToList();

            foreach (SupervisorClocking clocking in dayList)
            {
                TempSchedulerSQL.insert(Convert.ToInt32(clocking.id), DateTime.Parse(date.ToShortDateString() + " " + clocking.start.ToString()), DateTime.Parse(date.ToShortDateString() + " " + clocking.end.ToString()), 9);
            }
            foreach (SupervisorClocking clocking in nightList)
            {
                TempSchedulerSQL.insert(Convert.ToInt32(clocking.id), DateTime.Parse(date.ToShortDateString() + " " + clocking.start.ToString()), DateTime.Parse(date.AddDays(1).ToShortDateString() + " " + clocking.end.ToString()), 9);
            }
        }

        public void scheduleWhoWorks(DateTime start, DateTime end, List<EmployeeShift> employeeShifts, SchedulingType type)
        {
            TimeSpan timeSpan = end.Subtract(start);

            employeeShifts = employeeShifts.OrderBy(item => rng.Next()).ToList();

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
                        else if (shift.startTime < start && shift.startTime.Add(shift.shiftTimeSpan) > start && type == SchedulingType.Check)
                        {
                            scheduleInBestWorkstation(start, shift.startTime.Add(shift.shiftTimeSpan), employeeShift, type);
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

        public void scheduleWhoWorks2(DateTime start, DateTime end, List<EmployeeShift> employeeShifts, SchedulingType type)
        {
            TimeSpan timeSpan = end.Subtract(start);

            double shiftLenfth = 4;

            employeeShifts = employeeShifts.OrderBy(item => rng.Next()).ToList();

            foreach (EmployeeShift employeeShift in employeeShifts.ToList())
            {
                foreach (Shift shift in employeeShift.shifts.ToList())
                {
                    //Length of half of the remaining shift
                    double halfShiftSpan = (new TimeSpan(shift.shiftTimeSpan.Ticks / 2).Minutes)/60;
                    if (shift.startTime == start && halfShiftSpan > shiftLenfth)
                    {
                        scheduleInBestWorkstation(start, start.AddHours(halfShiftSpan), employeeShift, type);
                    }
                    else if (shift.startTime == start)
                    {
                        scheduleInBestWorkstation(start, shift.startTime.Add(shift.shiftTimeSpan), employeeShift, type);
                    }
                    /*
                    else if (shift.startTime < start && shift.startTime.Add(shift.shiftTimeSpan) > start && type == SchedulingType.Check)
                    {
                        //timespan, from parameter start time half way through the end of the shift
                        TimeSpan newSpan = new TimeSpan(shift.startTime.Add(shift.shiftTimeSpan).Subtract(start).Ticks / 2);
                        if (newSpan.Hours > shiftLenfth)
                        {
                            scheduleInBestWorkstation(start, start.AddHours(newSpan.Hours), employeeShift, type);
                        }
                        else if (shift.startTime.Add(shift.shiftTimeSpan).Subtract(start) > TimeSpan.Parse("00:30"))
                        {
                            scheduleInBestWorkstation(start, shift.startTime.Add(shift.shiftTimeSpan), employeeShift, type);
                        }
                    }
                    */
                }
                if (type == SchedulingType.Check)
                {
                    foreach(Shift shift in employeeShift.shifts.ToList())
                    {
                        if (shift.startTime < start && shift.startTime.Add(shift.shiftTimeSpan) > start)
                        {
                            //timespan, from parameter start time half way through the end of the shift
                            TimeSpan newSpan = new TimeSpan(shift.startTime.Add(shift.shiftTimeSpan).Subtract(start).Ticks / 2);
                            if (newSpan.Hours > shiftLenfth)
                            {
                                scheduleInBestWorkstation(start, start.AddHours(newSpan.Hours), employeeShift, type);
                            }
                            else if (shift.startTime.Add(shift.shiftTimeSpan).Subtract(start) > TimeSpan.Parse("00:30"))
                            {
                                scheduleInBestWorkstation(start, shift.startTime.Add(shift.shiftTimeSpan), employeeShift, type);
                            }
                        }
                    }
                }
            }
        }
        public void justScheduleThem(DateTime start, DateTime end, EmployeeShift employeeShift)
        {
            if (isStationOpen(start, end, 1, 1))
            {
                scheduleEmployee(employeeShift, start, end, 1);
            }
            else if (isStationOpen(start, end, 5, 1))
            {
                scheduleEmployee(employeeShift, start, end, 5);
            }
            else if (isStationOpen(start, end, 3, 1))
            {
                scheduleEmployee(employeeShift, start, end, 3);
            }
            else if (isStationOpen(start, end, 4, 1))
            {
                scheduleEmployee(employeeShift, start, end, 4);
            }
            else if (isStationOpen(start, end, 6, 1))
            {
                scheduleEmployee(employeeShift, start, end, 6);
            }
            else if (isStationOpen(start, end, 7, 1))
            {
                scheduleEmployee(employeeShift, start, end, 7);
            }
            else if (isStationOpen(start, end, 3, 2))
            {
                scheduleEmployee(employeeShift, start, end, 3);
            }
            else if (isStationOpen(start, end, 4, 2))
            {
                scheduleEmployee(employeeShift, start, end, 4);
            }
            else if (isStationOpen(start, end, 6, 2))
            {
                scheduleEmployee(employeeShift, start, end, 6);
            }
            else if (isStationOpen(start, end, 7, 2))
            {
                scheduleEmployee(employeeShift, start, end, 7);
            }
            else if (isStationOpen(start, end, 2, 1))
            {
                scheduleEmployee(employeeShift, start, end, 2);
            }
            else if (isStationOpen(start, end, 8, 1) && (start.TimeOfDay >= JetPedsStart))
            {
                if (end.Date > start.Date)
                {
                    scheduleEmployee(employeeShift, start, DateTime.Parse(end.ToShortDateString() + " 00:00"), 8);
                }
                else
                {
                    scheduleEmployee(employeeShift, start, end, 8);
                }
            }
            else
            {
                scheduleEmployee(employeeShift, start, start.AddMinutes(30), 10);
            }
        }

        public void scheduleInBestWorkstation(DateTime start, DateTime end, EmployeeShift employeeShift, SchedulingType type)
        {
            int employee = Convert.ToInt32(employeeShift.employee);
            if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
            {
                scheduleEmployee(employeeShift, start, end, 1);
            }
            else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
            {
                scheduleEmployee(employeeShift, start, end, 5);
            }
            else if (type == SchedulingType.Part || type == SchedulingType.Fill)
            {
                if (isStationOpen(start, end, 4, 1) && !didEmployeeWorkAtStationLast(start, 4, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 4);
                }
                else if (isStationOpen(start, end, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 3);
                }
                else if (isStationOpen(start, end, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 7);
                }
                else if (isStationOpen(start, end, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 6);
                }
                else if (isStationOpen(start, end, 4, 2) && !didEmployeeWorkAtStationLast(start, 4, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 4);
                }
                else if (isStationOpen(start, end, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 3);
                }
                else if (isStationOpen(start, end, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 7);
                }
                else if (isStationOpen(start, end, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 6);
                }
                else if (isStationOpen(start, end, 8, 1) && (start.TimeOfDay >= JetPedsStart) && !didEmployeeWorkAtStationLast(start, 8, employee))
                {
                    if (end.Date > start.Date)
                    {
                        scheduleEmployee(employeeShift, start, DateTime.Parse(end.ToShortDateString() + " 00:00"), 8);
                    }
                    else
                    {
                        scheduleEmployee(employeeShift, start, end, 8);
                    }
                }
                else if (isStationOpen(start, end, 2, 1) && !didEmployeeWorkAtStationLast(start, 2, employee))
                {
                    scheduleEmployee(employeeShift, start, end, 2);
                }
                else
                {
                    justScheduleThem(start, end, employeeShift);
                }
            }
        }

        public bool isStationOpen(DateTime start, DateTime end, int station, int num)
        {
            List<object> checkerList = new List<object>();
            List<object> checkerList2 = new List<object>();

            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";
            String sqlString2 = @"SELECT A.[Id] FROM [REVINT].[dbo].[ED_Shifts] A WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";

            new idMaker(sqlString, checkerList);
            new idMaker(sqlString2, checkerList2);

            return (checkerList.Count + checkerList2.Count) < num;
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
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[HEALTHCARE\eliprice].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE A.[EndShift] = '" + start + "' AND A.Seat = " + station + " AND A.[Employee] = " + employee + ";";

            new idMaker(sqlString, checkerList);
            return checkerList.Count > 0;
        }
        public void scheduleEmployee(EmployeeShift employeeShift, DateTime start, DateTime end, int station)
        {
            TimeSpan timeSpan = end.Subtract(start);
            TempSchedulerSQL.insert(Convert.ToInt32(employeeShift.employee), start, end, station);

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
            else if (isStationOpen(time, time, 4, 2))
            {
                return 4;
            }
            else if (isStationOpen(time, time, 3, 2))
            {
                return 3;
            }
            else if (isStationOpen(time, time, 7, 2))
            {
                return 7;
            }
            else if (isStationOpen(time, time, 6, 2))
            {
                return 6;
            }
            /*
            else if (isStationOpen(time, time, 8, 1))
            {
                return 8;
            }
            */
            else if (isStationOpen(time, time, 2, 1))
            {
                return 2;
            }
            return -1;
        }
        private int getLowestFilledStation(DateTime time)
        {
            if (!isStationOpen(time, time, 2, 1))
            {
                return 2;
            }
            else if (!isStationOpen(time, time, 8, 1))
            {
                return 8;
            }
            else if (!isStationOpen(time, time, 4, 2))
            {
                return 4;
            }
            else if (!isStationOpen(time, time, 6, 2))
            {
                return 6;
            }
            else if (!isStationOpen(time, time, 7, 2))
            {
                return 7;
            }
            else if (!isStationOpen(time, time, 3, 2))
            {
                return 3;
            }
            else if (!isStationOpen(time, time, 4, 1))
            {
                return 4;
            }
            else if (!isStationOpen(time, time, 6, 1))
            {
                return 6;
            }
            else if (!isStationOpen(time, time, 7, 1))
            {
                return 7;
            }
            else if (!isStationOpen(time, time, 3, 1))
            {
                return 3;
            }
            else if (!isStationOpen(time, time, 5, 1))
            {
                return 5;
            }
            else if (!isStationOpen(time, time, 1, 1))
            {
                return 1;
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
                TempSchedulerSQL.insertBlank(start, end, station);
            }
        }
        public void markAsAbsent(DateTime start, DateTime end, object station, object id, List <MarkAsAbsentShift> deletedShifts, List <MarkAsAbsentShift> newShifts)
        {
            String stationstr = station.ToString();

            List<object> employeeName = new List<object>();

            String sqlString = @"SELECT A.Employee FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[Id] = " + id + ";";

            new idMaker(sqlString, employeeName);
            if (employeeName.Count > 0)
            {
                sqlString = @"SELECT A.FirstName + ' ' + A.LastName FROM [REVINT].[healthcare\eliprice].[ED_Employees] A WHERE A.[Id] = " + employeeName[0] + ";";
                employeeName = new List<object>();
                new idMaker(sqlString, employeeName);
                MarkAsAbsentShift deletedShift = new MarkAsAbsentShift();
                if (employeeName.Count > 0)
                {
                    deletedShift.Name = employeeName[0];
                    
                    sqlString = @"SELECT A.Name FROM [REVINT].[dbo].[ED_Seats] A WHERE A.[Id] = " + station + ";";
                    List<object> seatName = new List<object>();

                    new idMaker(sqlString, seatName);
                    if (seatName.Count > 0)
                    {
                        deletedShift.Start = start;
                        deletedShift.End = end;
                        deletedShift.Seat = seatName[0];

                        deletedShifts.Add(deletedShift);
                    }
                }
            }

            //Delete clocking
            EmployeeScheduleSQL.deleteClocking(id);

            if (stationstr == "1" || stationstr == "5")
            {
                DateTime startout;
                while (start < end)
                {
                    bumpUpToFillStation(start, end, station, out startout, deletedShifts, newShifts);
                    start = startout;
                }
            }
            doMinStaffing(start);
            doMinStaffing(end);
        }

        private void bumpUpToFillStation(DateTime start, DateTime end, object station, out DateTime startout, List<MarkAsAbsentShift> deletedShifts, List<MarkAsAbsentShift> newShifts)
        {
            startout = new DateTime();
            int oldstation = getLowestFilledStation(start);
            if (oldstation == -1)
            {
                startout = start.AddMinutes(30);
            }
            else
            {
                bumpFromStationToAnother(oldstation, station, start, end, out startout, deletedShifts, newShifts);
            }
            
        }

        private void bumpFromStationToAnother(object oldstation, object newstation, DateTime start, DateTime end, out DateTime startout, List<MarkAsAbsentShift> deletedShifts, List<MarkAsAbsentShift> newShifts)
        {
            //base case

            List<object> checkerEmployeeList = new List<object>();
            List<object> checkerShiftList = new List<object>();
            List<object> checkerStartTimes = new List<object>();
            List<object> checkerIds = new List<object>();


            String sqlString = "SELECT A.Employee FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[StartShift] <= '" + start + "' AND A.[EndShift] > '" + start + "' AND A.Seat = '" + oldstation + "' ORDER BY A.[EndShift];";
            String sqlString2 = "SELECT A.[EndShift] FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[StartShift] <= '" + start + "' AND A.[EndShift] > '" + start + "' AND A.Seat = '" + oldstation + "' ORDER BY A.[EndShift];";
            String sqlString3 = "SELECT A.[StartShift] FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[StartShift] <= '" + start + "' AND A.[EndShift] > '" + start + "' AND A.Seat = '" + oldstation + "' ORDER BY A.[EndShift];";
            String sqlString4 = "SELECT A.[Id] FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[StartShift] <= '" + start + "' AND A.[EndShift] > '" + start + "' AND A.Seat = '" + oldstation + "' ORDER BY A.[EndShift];";


            new idMaker(sqlString, checkerEmployeeList);
            new idMaker(sqlString2, checkerShiftList);
            new idMaker(sqlString3, checkerStartTimes);
            new idMaker(sqlString4, checkerIds);

            if (checkerEmployeeList.Count > 0)
            {
                if (checkerEmployeeList[0] != null && checkerShiftList[0] != null)
                {
                    //recover beginning of shift
                    DateTime previousShiftStartTime = DateTime.Parse(checkerStartTimes[0].ToString());
                    if (previousShiftStartTime < start)
                    {
                        
                        //add clocking to added shifts
                        List<object> employeeName = new List<object>();

                        
                        String sqlEmployeeString = @"SELECT A.FirstName + ' ' + A.LastName FROM [REVINT].[healthcare\eliprice].[ED_Employees] A WHERE A.[Id] = " + checkerEmployeeList[0] + ";";
                        employeeName = new List<object>();
                        new idMaker(sqlEmployeeString, employeeName);
                        MarkAsAbsentShift addedShift = new MarkAsAbsentShift();
                        if (employeeName.Count > 0)
                        {
                            addedShift.Name = employeeName[0];

                            sqlEmployeeString = @"SELECT A.Name FROM [REVINT].[dbo].[ED_Seats] A WHERE A.[Id] = " + oldstation + ";";
                            List<object> seatName = new List<object>();

                            new idMaker(sqlEmployeeString, seatName);
                            if (seatName.Count > 0)
                            {
                                addedShift.Start = previousShiftStartTime;
                                addedShift.End = start;
                                addedShift.Seat = seatName[0];

                                newShifts.Add(addedShift);
                            }
                        }

                    }

                    EmployeeScheduleSQL.addClocking(checkerEmployeeList[0], oldstation, previousShiftStartTime, start);

                    DateTime previousShiftEndTime = DateTime.Parse(checkerShiftList[0].ToString());

                    //change center of shift and recover end shift
                    if (previousShiftEndTime > end)
                    {
                        //add clocking to added shifts
                        List<object> employeeName = new List<object>();

                        String sqlEmployeeString = @"SELECT A.FirstName + ' ' + A.LastName FROM [REVINT].[healthcare\eliprice].[ED_Employees] A WHERE A.[Id] = " + checkerEmployeeList[0] + ";";
                        
                        employeeName = new List<object>();
                        new idMaker(sqlEmployeeString, employeeName);
                        MarkAsAbsentShift addedShift = new MarkAsAbsentShift();
                        MarkAsAbsentShift addedShift2 = new MarkAsAbsentShift();

                        if (employeeName.Count > 0)
                        {
                            addedShift.Name = employeeName[0];
                            addedShift2.Name = employeeName[0];

                            sqlEmployeeString = @"SELECT A.Name FROM [REVINT].[dbo].[ED_Seats] A WHERE A.[Id] = " + newstation + ";";
                            List<object> seatName = new List<object>();

                            new idMaker(sqlEmployeeString, seatName);
                            if (seatName.Count > 0)
                            {
                                addedShift.Start = start;
                                addedShift.End = end;
                                addedShift.Seat = seatName[0];

                                newShifts.Add(addedShift);
                            }

                            sqlEmployeeString = @"SELECT A.Name FROM [REVINT].[dbo].[ED_Seats] A WHERE A.[Id] = " + oldstation + ";";
                            seatName = new List<object>();

                            new idMaker(sqlEmployeeString, seatName);
                            if (seatName.Count > 0)
                            {
                                addedShift.Start = end;
                                addedShift.End = previousShiftEndTime;
                                addedShift.Seat = seatName[0];

                                newShifts.Add(addedShift);
                            }
                        }

                        EmployeeScheduleSQL.addClocking(checkerEmployeeList[0], newstation, start, end);
                        EmployeeScheduleSQL.addClocking(checkerEmployeeList[0], oldstation, end, previousShiftEndTime);
                        startout = end;
                    }
                    else
                    {
                        //add clocking to added shifts
                        List<object> employeeName = new List<object>();

                        String sqlEmployeeString = @"SELECT A.FirstName + ' ' + A.LastName FROM [REVINT].[healthcare\eliprice].[ED_Employees] A WHERE A.[Id] = " + checkerEmployeeList[0] + ";";
                        employeeName = new List<object>();
                        new idMaker(sqlEmployeeString, employeeName);
                        MarkAsAbsentShift addedShift = new MarkAsAbsentShift();

                        if (employeeName.Count > 0)
                        {
                            addedShift.Name = employeeName[0];

                            sqlEmployeeString = @"SELECT A.Name FROM [REVINT].[dbo].[ED_Seats] A WHERE A.[Id] = " + newstation + ";";
                            List<object> seatName = new List<object>();

                            new idMaker(sqlEmployeeString, seatName);
                            if (seatName.Count > 0)
                            {
                                addedShift.Start = start;
                                addedShift.End = previousShiftEndTime;
                                addedShift.Seat = seatName[0];

                                newShifts.Add(addedShift);
                            }
                        }

                        EmployeeScheduleSQL.addClocking(checkerEmployeeList[0], newstation, start, previousShiftEndTime);
                        startout = previousShiftEndTime;
                    }

                    //Add deleted clocking to deleted shifts
                    List<object> employeeName2 = new List<object>();

                    String sqlEmployeeString2 = @"SELECT A.Employee FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[Id] = " + checkerIds[0] + ";";

                    new idMaker(sqlEmployeeString2, employeeName2);
                    if (employeeName2.Count > 0)
                    {
                        sqlEmployeeString2 = @"SELECT A.FirstName + ' ' + A.LastName FROM [REVINT].[healthcare\eliprice].[ED_Employees] A WHERE A.[Id] = " + employeeName2[0] + ";";
                        employeeName2 = new List<object>();
                        new idMaker(sqlEmployeeString2, employeeName2);
                        MarkAsAbsentShift deletedShift = new MarkAsAbsentShift();
                        if (employeeName2.Count > 0)
                        {
                            deletedShift.Name = employeeName2[0];

                            sqlEmployeeString2 = @"SELECT A.Seat FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[Id] = " + checkerIds[0] + ";";

                            List<object> seatName = new List<object>();

                            new idMaker(sqlEmployeeString2, seatName);

                            if (seatName.Count > 0)
                            {
                                sqlEmployeeString2 = @"SELECT A.Name FROM [REVINT].[dbo].[ED_Seats] A WHERE A.[Id] = " + seatName[0] + ";";
                                seatName = new List<object>();

                                new idMaker(sqlEmployeeString2, seatName);
                                if (seatName.Count > 0)
                                {
                                    deletedShift.Seat = seatName[0];

                                    sqlEmployeeString2 = @"SELECT A.StartShift FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[Id] = " + checkerIds[0] + ";";

                                    employeeName2 = new List<object>();
                                    new idMaker(sqlEmployeeString2, employeeName2);
                                    if (employeeName2.Count > 0)
                                    {
                                        deletedShift.Start = employeeName2[0];

                                        sqlEmployeeString2 = @"SELECT A.EndShift FROM [REVINT].[dbo].[ED_Shifts] A WHERE A.[Id] = " + checkerIds[0] + ";";

                                        employeeName2 = new List<object>();
                                        new idMaker(sqlEmployeeString2, employeeName2);
                                        if (employeeName2.Count > 0)
                                        {
                                            deletedShift.End = employeeName2[0];

                                            deletedShifts.Add(deletedShift);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //delete old clocking
                    EmployeeScheduleSQL.deleteClocking(checkerIds[0]);
                }
                else
                {
                    startout = start.AddMinutes(30);
                }
            }
            else
            {
                startout = start.AddMinutes(30);
            }
        }
    }
}