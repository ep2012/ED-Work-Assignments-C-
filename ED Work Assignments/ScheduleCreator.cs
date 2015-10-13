using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ED_Work_Assignments
{
    //1 = Check In & POD 1/2
    //2 = Check Out & POD 3/4
    //3 = All others Excluding Supervising
    //if !1 filled, fill 1. If not 2 filled, fill 2. Else give to random 3.
    public class ScheduleMaker
    {
        public ScheduleMaker(DateTime start, DateTime end)
        {
            if (start == null || end == null)
            {
                return;
            }
            else
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

                    if ((day - scheduleStartTime).TotalDays%14 < 7)
                        schedulestr += "1";
                    else
                        schedulestr += "2";

                    scheduleDay(day, schedulestr);
                    day = day.AddDays(1);

                    progBar.updateProg(Convert.ToInt32((day - start).TotalDays));
                    Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                              new Action(delegate { }));
                }
            }
        }
        public void scheduleDay(DateTime day, String dateStr)
        {
            //new SupervisorScheduleCreator(day, dateStr);
            new EmployeeScheduleCreator(day, dateStr);
        }
    }



    public class Schedule
    {
        TempScheduler tempScheduler = new TempScheduler();
        Users users = new Users();
        Random random = new Random();
        public Schedule()
        {

        }
        public void scheduleSupervisor(DateTime start, DateTime end, int employee)
        {
            scheduleEmployee(employee, start, end, 9);
        }
        public void scheduleInBestWorkstation(DateTime start, DateTime end, int employee)
        {
            DateTime endTime = end;
            if((end-start).TotalMinutes > 150)
            {
                endTime = start.AddMinutes(30 * random.Next(2, 5));
            }
            if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
            {
                scheduleEmployee(employee, start, end, 1);
            }
            else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
            {
                scheduleEmployee(employee, start, end, 5);
            }
            else if (isStationOpen(start, endTime, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, endTime, 6);
            }

            //else if !POD 3/4 filled, fill it
            else if (isStationOpen(start, endTime, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, endTime, 7);
            }    
            else if (isStationOpen(start, endTime, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, endTime, 3);
            }
            else if (isStationOpen(start, endTime, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, endTime, 6);
            }
            else if (isStationOpen(start, endTime, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, endTime, 7);
            }
            else if (isStationOpen(start, endTime, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, endTime, 3);
            }
            else if (isStationOpen(start, endTime, 4, 2) && !didEmployeeWorkAtStationLast(start, 4, employee))
            {
                scheduleEmployee(employee, start, endTime, 4);
            }
            //else if !iPad filled, fill it
            else if (isStationOpen(start, endTime, 2, 1))
            {
                scheduleEmployee(employee, start, endTime, 2);
            }
            //else if (isStationOpenAtBeginning(start, end, out endTime, 2, 1))
            //{
            //    scheduleEmployee(employee, start, endTime, 2);
            //}
            else
            {
                //System.Windows.MessageBox.Show("Overstaffed " + start);
            }
        }
        public void putHoleInHighestPriorityStation(DateTime time)
        {
            int station = getHighestOpenStation(time);
            if (station != -1)
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                getOpenShiftRange(time, station, out start, out end);
                tempScheduler.insertBlank(start, end, station);
            }
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
        public void scheduleInBestWorkstation2(DateTime start, DateTime end, int employee)
        {
            DateTime endTime = start.AddMinutes(30 * random.Next(2,5));

            if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
            {
                scheduleEmployee(employee, start, end, 1);
            }
            else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
            {
                scheduleEmployee(employee, start, end, 5);
            }
            else if (isStationOpen(start, endTime, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, endTime, 6);
            }
            else if (isStationOpen(start, endTime, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, endTime, 7);
            }
            else if (isStationOpen(start, endTime, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, endTime, 3);
            }
            else if (isStationOpen(start, endTime, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, endTime, 6);
            }
            else if (isStationOpen(start, endTime, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, endTime, 7);
            }
            else if (isStationOpen(start, endTime, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, endTime, 3);
            }
            else if (isStationOpen(start, endTime, 4, 2))
            {
                scheduleEmployee(employee, start, endTime, 4);
            }
            //else if !iPad filled, fill it
            else if (isStationOpen(start, endTime, 2, 1))
            {
                scheduleEmployee(employee, start, endTime, 2);
            }
            //else if (isStationOpenAtBeginning(start, end, out endTime, 2, 1))
            //{
            //    scheduleEmployee(employee, start, endTime, 2);
            //}
            else if (end < DateTime.Today.AddDays(7))
            {
                //System.Windows.MessageBox.Show("Overstaffed " + start + " Employee " + users.getNameFromId(employee.ToString()));
            }

        }
        private int GenerateDigit(int num)
        {
            return random.Next(num);
        }

        private bool isStationOpen(DateTime start, DateTime end, int station, int num)
        {
            List<object> checkerList = new List<object>();
            //String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";

            new idMaker(sqlString, checkerList);
            return checkerList.Count < num;
        }
        private bool didEmployeeWorkAtStationLast(DateTime start, int station, int employee)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE A.[EndShift] = '" + start + "' AND A.Seat = " + station + " AND A.[Employee] = " + employee + ";";

            new idMaker(sqlString, checkerList);
            return checkerList.Count > 0;
        }

        private bool isStationOpenAtBeginning(DateTime start, DateTime end, out DateTime endTime, int station, int num)
        {
            List<object> checkerList = new List<object>();
            List<object> returnList = new List<object>();
            endTime = new DateTime();

            String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE (((A.[StartShift] <= '" + start + "') AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + " GROUP BY B.[Id];";
            String returnString = @"SELECT A.[StartShift] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + " ORDER BY A.[EndShift];";

            new idMaker(sqlString, checkerList);
            new idMaker(returnString, returnList);
            if (returnList.Count > 0)
                endTime = DateTime.Parse(returnList[0].ToString());
            return checkerList.Count < num;
        }
         
        private void scheduleEmployee(int employee, DateTime start, DateTime end, int station)
        {
            tempScheduler.insert(employee, start, end, station);
        }

        private List<object> whoHasVacation(DateTime start, DateTime end)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[EmployeeId] FROM [REVINT].[HEALTHCARE\eliprice].[ED_TimeOff] A WHERE A.[StartTime] <= '" + start + "' AND A.[EndTime] >= '" + end + "';";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }

        public List<object> whoCanWork(DateTime start, DateTime end, String day)
        {
            return whoWorks(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start,end)).ToList();
        }
        public List<object> whoCanWorkDay2(DateTime start, DateTime end, String day)
        {
            return whoWorksDay2(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start, end)).ToList();
        }

        
        public List<OffClocking> whoCanWorkOnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> returnList = whoWorksOnlyStart(start, end, day);

            foreach (object employee in (whoAlreadyWorks(start, end).Union(whoHasVacation(start, end))).ToList())
            {
                foreach (OffClocking clocking in returnList.ToList())
                {
                    if (clocking.id.ToString() == employee.ToString())
                    {
                        returnList.Remove(clocking);
                    }
                }
            }
            return returnList;
        }


        public List<OffClocking> whoCanWorkDay2OnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> returnList = whoWorksDay2OnlyStart(start, end, day);

            foreach (object employee in (whoAlreadyWorks(start, end).Union(whoHasVacation(start, end))).ToList())
            {
                foreach (OffClocking clocking in returnList.ToList())
                {
                    if (clocking.id.ToString() == employee.ToString())
                    {
                        returnList.Remove(clocking);
                    }
                }
            }
            return returnList;
        }
        
        public List<object> whoWorks(DateTime start, DateTime end, String day)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time) AND cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time)) OR (cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time) AND (A." + day + "day = 'True'))) AND (A.Role = 2 OR A.Role = 3) ORDER BY NEWID();";
            //String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time)) AND ((cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time)) OR (cast(A." + day + "timeend as time) BETWEEN cast('" + end.ToString("HH:mm") + "' as time) AND cast('" + start.ToString("HH:mm") + "' as time)) OR (A." + day + "day = 'True'))) AND (A.Role = 2 OR A.Role = 3) ORDER BY NEWID();";

            String sqlString2 = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time) AND cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time)) OR (cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 2 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            if (checkerList.Count > 11)
            {
                checkerList = checkerList;
            }
            return checkerList;
        }
        public List<object> whoWorksDay2(DateTime start, DateTime end, String day)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time) AND (A." + day + "day = 'True'))) AND (A.Role = 2 OR A.Role = 3) ORDER BY NEWID();";
            String sqlString2 = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 2 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }

        public List<OffClocking> whoWorksOnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) > cast('" + start + "' as time)) AND (cast(A." + day + "timeend as time) < cast('" + end + "' as time))) AND (A.Role = 2 OR A.Role = 3) AND NOT (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<OffClocking> whoWorksDay2OnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) > cast('" + start + "' as time)) AND (cast(A." + day + "timeend as time) < cast('" + end + "' as time))) AND (A.Role = 2 OR A.Role = 3) AND (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }

        public List<object> whoAlreadyWorks(DateTime start, DateTime end)
        {
            List<object> checkerList = new List<object>();
            List<object> checkerList2 = new List<object>();
            String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE (A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "')) GROUP BY B.[Id];";
            String sqlString2 = "SELECT B.[Id] FROM [REVINT].[dbo].[ED_Shifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE (A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "')) GROUP BY B.[Id];";
            new idMaker(sqlString, checkerList);
            new idMaker(sqlString, checkerList2);
            
            return checkerList.Union(checkerList2).ToList();
        }
        private void clearDay(DateTime day)
        {

        }
    }
    


    public class EmployeeScheduleCreator
    {
        Schedule schedule = new Schedule();
        Random random = new Random();

        public EmployeeScheduleCreator(DateTime day, String dayStr)
        {
            DateTime goaldate = day.AddHours(36);
            DateTime startTime = day;
            //DateTime endTime = startTime.AddHours(5);
            DateTime endTime = startTime.AddMinutes(30 * random.Next(8, 12));

            while(startTime < goaldate)
            {
                List<object> employeesThatCanWork = new List<object>();

                List<OffClocking> employeesThatCanWorkFirstPart = new List<OffClocking>();

                //get employees that can work
                if ((endTime - day).TotalHours < 24)
                {
                    employeesThatCanWork = schedule.whoCanWork(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkOnlyStart(startTime, endTime, dayStr);

                }
                else
                {
                    employeesThatCanWork = schedule.whoCanWorkDay2(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkDay2OnlyStart(startTime, endTime, dayStr);
                }
                
                //schedule employees

                foreach (OffClocking clockings in employeesThatCanWorkFirstPart)
                {
                    DateTime end = DateTime.Parse(endTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(startTime, end, Convert.ToInt32(clockings.id));
                }

                
                foreach (object employee in employeesThatCanWork)
                {
                    //DateTime end = startTime.AddMinutes(30 * random.Next(4, 8));
                    schedule.scheduleInBestWorkstation2(startTime,endTime, Convert.ToInt32(employee));
                }
                //increment times
                startTime = startTime.AddMinutes(30);
                //endTime = startTime.AddHours(5);
                endTime = startTime.AddMinutes(30 * random.Next(8, 12));
            }
            /*
            String sqlString = "WITH StaffingReport AS (SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement]" +
                    ", COUNT([REVINT].[dbo].[ED_Shifts].Id) AS [Staffed]" +
                    "FROM [REVINT].[dbo].[ED_Staffing] " +
                    "LEFT JOIN [REVINT].[dbo].[ED_Shifts] " +
                    "ON [REVINT].[dbo].[ED_Staffing].TimeSlot BETWEEN CAST([REVINT].[dbo].[ED_Shifts].StartShift AS time) AND CAST([REVINT].[dbo].[ED_Shifts].EndShift AS time)";

            sqlString += "AND [REVINT].[dbo].[ED_Shifts].StartShift >'" + day + "'AND [REVINT].[dbo].[ED_Shifts].EndShift <'" + day.AddDays(1) + "'";
            
            sqlString += " GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing";
            sqlString += " UNION ALL";
            sqlString += " SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement], COUNT([REVINT].[dbo].[ED_Shifts].Id) AS [Staffed]";
            sqlString += " FROM [REVINT].[dbo].[ED_Staffing] LEFT JOIN [REVINT].[dbo].[ED_Shifts] ON ([REVINT].[dbo].[ED_Staffing].TimeSlot = '00:00:00' ";
            sqlString += " AND (CAST([REVINT].[dbo].[ED_Shifts].EndShift AS date) > CAST([REVINT].[dbo].[ED_Shifts].StartShift AS date) AND [REVINT].[dbo].[ED_Shifts].StartShift >'" + day + "' AND [REVINT].[dbo].[ED_Shifts].StartShift <'" + day.AddDays(1) + "'))";
            sqlString += " GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing";
            sqlString += @" UNION ALL SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement], COUNT(SM.Id) AS [Staffed] FROM [REVINT].[dbo].[ED_Staffing] LEFT JOIN [REVINT].[HEALTHCARE\eliprice].ED_ScheduleMakerShifts SM ON [REVINT].[dbo].[ED_Staffing].TimeSlot BETWEEN CAST(SM.StartShift AS time) AND CAST(SM.EndShift AS time) AND SM.StartShift >'" + day + "' AND SM.EndShift <'" + day.AddDays(1) + "' GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing";
            sqlString += @" UNION ALL SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement], COUNT(SM.Id) AS [Staffed] FROM [REVINT].[dbo].[ED_Staffing] LEFT JOIN [REVINT].[HEALTHCARE\eliprice].ED_ScheduleMakerShifts SM ON ([REVINT].[dbo].[ED_Staffing].TimeSlot = '00:00:00'  AND (CAST(SM.EndShift AS date) > CAST(SM.StartShift AS date) AND SM.StartShift >'" + day + "' AND SM.StartShift <'" + day.AddDays(1) + "')) GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing)";
            sqlString += " SELECT [Time Slot], [Minimum Staffing Requirement], SUM ([Staffed]) AS [Amount Staffed]" +
                ", CASE WHEN SUM([Staffed]) < [Minimum Staffing Requirement] THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                "  FROM StaffingReport WHERE [Staffed] >= [Minimum Staffing Requirement] GROUP BY [Time Slot], [Minimum Staffing Requirement] ORDER BY [Time Slot]";
            */

            String sqlString = "WITH StaffingReport AS (SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                //", CASE WHEN COUNT(Shifts.Id) < Staffing.MinStaffing THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            " LEFT JOIN [REVINT].[dbo].[ED_Shifts] Shifts" +
                            " ON CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing" +
                            " UNION ALL" +
                            " SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                //", CASE WHEN COUNT(Shifts.Id) < Staffing.MinStaffing THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            @" LEFT JOIN [REVINT].[healthcare\eliprice].[ED_ScheduleMakerShifts] Shifts" +
                            " ON CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing)";
            sqlString += " SELECT [Time Slot], [Minimum Staffing Requirement], SUM ([Amount Staffed]) AS [Amount Staffed]" +
                //", CASE WHEN SUM([Staffed]) < [Minimum Staffing Requirement] THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                    "  FROM StaffingReport WHERE [Amount Staffed] >= [Minimum Staffing Requirement] GROUP BY [Time Slot], [Minimum Staffing Requirement] ORDER BY [Time Slot]";

            List<object> fullStaffing = new List<object>();
            new idMaker(sqlString, fullStaffing, "");

            List<object> minStaffingSlots = new List<object>();
            sqlString = "SELECT TimeSlot FROM [REVINT].[dbo].[ED_Staffing]";

            new idMaker(sqlString, minStaffingSlots, "");

            List<object> minStaffingList = minStaffingSlots.Except(fullStaffing).ToList();

            while (minStaffingList.Count > 0)
            {
                foreach (object date in minStaffingList)
                {
                    schedule.putHoleInHighestPriorityStation(DateTime.Parse(day.ToShortDateString() + " " + date.ToString()));
                }

                sqlString = "WITH StaffingReport AS (SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                            //", CASE WHEN COUNT(Shifts.Id) < Staffing.MinStaffing THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            " LEFT JOIN [REVINT].[dbo].[ED_Shifts] Shifts" +
                            " ON CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing" +
                            " UNION ALL" +
                            " SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                            //", CASE WHEN COUNT(Shifts.Id) < Staffing.MinStaffing THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            @" LEFT JOIN [REVINT].[healthcare\eliprice].[ED_ScheduleMakerShifts] Shifts" +
                            " ON CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing)";
                sqlString += " SELECT [Time Slot], [Minimum Staffing Requirement], SUM ([Amount Staffed]) AS [Amount Staffed]" +
                        //", CASE WHEN SUM([Staffed]) < [Minimum Staffing Requirement] THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                        "  FROM StaffingReport WHERE [Amount Staffed] >= [Minimum Staffing Requirement] GROUP BY [Time Slot], [Minimum Staffing Requirement] ORDER BY [Time Slot]";
                /*
                sqlString = "WITH StaffingReport AS (SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement]" +
                    ", COUNT([REVINT].[dbo].[ED_Shifts].Id) AS [Staffed]" +
                    "FROM [REVINT].[dbo].[ED_Staffing] " +
                    "LEFT JOIN [REVINT].[dbo].[ED_Shifts] " +
                    "ON [REVINT].[dbo].[ED_Staffing].TimeSlot BETWEEN CAST([REVINT].[dbo].[ED_Shifts].StartShift AS time) AND CAST([REVINT].[dbo].[ED_Shifts].EndShift AS time)";

                sqlString += "AND [REVINT].[dbo].[ED_Shifts].StartShift >'" + day + "'AND [REVINT].[dbo].[ED_Shifts].EndShift <'" + day.AddDays(1) + "'";

                sqlString += " GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing";
                sqlString += " UNION ALL";
                sqlString += " SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement], COUNT([REVINT].[dbo].[ED_Shifts].Id) AS [Staffed]";
                sqlString += " FROM [REVINT].[dbo].[ED_Staffing] LEFT JOIN [REVINT].[dbo].[ED_Shifts] ON ([REVINT].[dbo].[ED_Staffing].TimeSlot = '00:00:00' ";
                sqlString += " AND (CAST([REVINT].[dbo].[ED_Shifts].EndShift AS date) > CAST([REVINT].[dbo].[ED_Shifts].StartShift AS date) AND [REVINT].[dbo].[ED_Shifts].StartShift >'" + day + "' AND [REVINT].[dbo].[ED_Shifts].StartShift <'" + day.AddDays(1) + "'))";
                sqlString += " GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing";
                sqlString += @" UNION ALL SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement], COUNT(SM.Id) AS [Staffed] FROM [REVINT].[dbo].[ED_Staffing] LEFT JOIN [REVINT].[HEALTHCARE\eliprice].ED_ScheduleMakerShifts SM ON [REVINT].[dbo].[ED_Staffing].TimeSlot BETWEEN CAST(SM.StartShift AS time) AND CAST(SM.EndShift AS time) AND SM.StartShift >'" + day + "' AND SM.EndShift <'" + day.AddDays(1) + "' GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing";
                sqlString += @" UNION ALL SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement], COUNT(SM.Id) AS [Staffed] FROM [REVINT].[dbo].[ED_Staffing] LEFT JOIN [REVINT].[HEALTHCARE\eliprice].ED_ScheduleMakerShifts SM ON ([REVINT].[dbo].[ED_Staffing].TimeSlot = '00:00:00'  AND (CAST(SM.EndShift AS date) > CAST(SM.StartShift AS date) AND SM.StartShift >'" + day + "' AND SM.StartShift <'" + day.AddDays(1) + "')) GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing)";
                sqlString += " SELECT [Time Slot], [Minimum Staffing Requirement], SUM ([Staffed]) AS [Amount Staffed]" +
                    ", CASE WHEN SUM([Staffed]) < [Minimum Staffing Requirement] THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                    "  FROM StaffingReport WHERE [Staffed] >= [Minimum Staffing Requirement] GROUP BY [Time Slot], [Minimum Staffing Requirement] ORDER BY [Time Slot]";
                */

                fullStaffing = new List<object>();
                new idMaker(sqlString, fullStaffing, "");

                minStaffingList = minStaffingSlots.Except(fullStaffing).ToList();
                 
            }

        }

    }
}
public class idMaker
{
    public idMaker(String sqlCmdID, List<object> ids)
    {
        object[] objID = new object[40];

        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        using (OdbcConnection connectionID = new OdbcConnection(cxnString))
        {
            OdbcCommand commandID = new OdbcCommand(sqlCmdID, connectionID);

            connectionID.Open();

            OdbcDataReader reader = commandID.ExecuteReader();

            while (reader.Read())
            {
                int numCols = reader.GetValues(objID);

                for (int i = 0; i < numCols; i++)
                {
                    ids.Add((object)objID[i].ToString().ToUpper());
                }
            }
        }
    }
    public idMaker(String sqlCmdID, List<object> ids, String text)
    {
        object[] objID = new object[40];

        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        using (OdbcConnection connectionID = new OdbcConnection(cxnString))
        {
            OdbcCommand commandID = new OdbcCommand(sqlCmdID, connectionID);

            connectionID.Open();

            OdbcDataReader reader = commandID.ExecuteReader();

            while (reader.Read())
            {
                int numCols = reader.GetValues(objID);

                if (numCols > 0)
                    ids.Add((object)objID[0].ToString().ToUpper());

            }
        }
    }
    public idMaker(String sqlCmdID, List<OffClocking> ids)
    {
        object[] objID = new object[40];

        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        using (OdbcConnection connectionID = new OdbcConnection(cxnString))
        {
            OdbcCommand commandID = new OdbcCommand(sqlCmdID, connectionID);

            connectionID.Open();

            OdbcDataReader reader = commandID.ExecuteReader();

            while (reader.Read())
            {
                int numCols = reader.GetValues(objID);

                if (numCols == 2)
                {
                    OffClocking offClocking = new OffClocking();
                    offClocking.id = (object)objID[0].ToString();
                    offClocking.date = (object)objID[1].ToString();
                    ids.Add(offClocking);
                }
                else if (numCols == 1)
                {
                    OffClocking offClocking = new OffClocking();
                    offClocking.id = (object)objID[0].ToString();
                    ids.Add(offClocking);
                }
            }
        }
    }
}
public struct OffClocking
{
    public object id;
    public object date;
}