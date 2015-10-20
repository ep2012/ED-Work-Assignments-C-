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

                    if ((day - scheduleStartTime).TotalDays % 14 < 7)
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

        public void scheduleInBestWorkstation(DateTime start, DateTime end, int employee)
        {
            if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
            {
                scheduleEmployee(employee, start, end, 1);
            }
            else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
            {
                scheduleEmployee(employee, start, end, 5);
            }
            else if (isStationOpen(start, end, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, end, 6);
            }

            //else if !POD 3/4 filled, fill it
            else if (isStationOpen(start, end, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, end, 7);
            }
            else if (isStationOpen(start, end, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, end, 3);
            }
            else if (isStationOpen(start, end, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, end, 6);
            }
            else if (isStationOpen(start, end, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, end, 7);
            }
            else if (isStationOpen(start, end, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, end, 3);
            }
            else if (isStationOpen(start, end, 4, 2) && !didEmployeeWorkAtStationLast(start, 4, employee))
            {
                scheduleEmployee(employee, start, end, 4);
            }
            //else if !iPad filled, fill it
            else if (isStationOpen(start, end, 2, 1))
            {
                scheduleEmployee(employee, start, end, 2);
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

            if (isStationOpen(start, end, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, end, 6);
            }
            else if (isStationOpen(start, end, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, end, 7);
            }
            else if (isStationOpen(start, end, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, end, 3);
            }
            else if (isStationOpen(start, end, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
            {
                scheduleEmployee(employee, start, end, 6);
            }
            else if (isStationOpen(start, end, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
            {
                scheduleEmployee(employee, start, end, 7);
            }
            else if (isStationOpen(start, end, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
            {
                scheduleEmployee(employee, start, end, 3);
            }
            else if (isStationOpen(start, end, 4, 2))
            {
                scheduleEmployee(employee, start, end, 4);
            }
            //else if !iPad filled, fill it
            else if (isStationOpen(start, end, 2, 1))
            {
                scheduleEmployee(employee, start, end, 2);
            }
            //else if (isStationOpenAtBeginning(start, end, out endTime, 2, 1))
            //{
            //    scheduleEmployee(employee, start, endTime, 2);
            //}
        }
        public void scheduleCheckInCheckOut(DateTime start, DateTime end, int employee)
        {
            if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
            {
                scheduleEmployee(employee, start, end, 1);
            }
            else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
            {
                scheduleEmployee(employee, start, end, 5);
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
            return whoWorks(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start, end)).ToList();
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

        public void scheduleSupervisors(DateTime date, String day)
        {
            List<SupervisorClocking> dayList = new List<SupervisorClocking>();
            List<SupervisorClocking> nightList = new List<SupervisorClocking>();
            String sqlStringNight = @"SELECT A.[Id], cast(A." + day + @"time as time), cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (A." + day + "day = 'True') AND (A.Role = 3) AND (A." + day + @"timeend IS NOT NULL) ORDER BY NEWID();";
            String sqlStringDay = @"SELECT A.[Id], cast(A." + day + @"time as time), cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (A.Role = 3) AND (A." + day + @"timeend IS NOT NULL) ORDER BY NEWID();";

            new idMaker(sqlStringDay, dayList);
            new idMaker(sqlStringNight, nightList);

            dayList = dayList.Except(nightList).ToList();

            foreach (SupervisorClocking clocking in dayList)
            {
                scheduleEmployee(Convert.ToInt32(clocking.id), DateTime.Parse(date.ToShortDateString() + " " + clocking.start.ToString()), DateTime.Parse(date.ToShortDateString() + " " + clocking.end.ToString()), 9);
            }
            foreach (SupervisorClocking clocking in nightList)
            {
                scheduleEmployee(Convert.ToInt32(clocking.id), DateTime.Parse(date.ToShortDateString() + " " + clocking.start.ToString()), DateTime.Parse(date.AddDays(1).ToShortDateString() + " " + clocking.end.ToString()), 9);
            }
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
            String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) GROUP BY B.[Id];";
            String sqlString2 = "SELECT B.[Id] FROM [REVINT].[dbo].[ED_Shifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) GROUP BY B.[Id];";
            new idMaker(sqlString, checkerList);
            new idMaker(sqlString, checkerList2);

            return checkerList.Union(checkerList2).ToList();
        }
        private void clearDay(DateTime day)
        {

        }
    }
}