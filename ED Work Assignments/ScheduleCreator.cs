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

                    if ((day - start).TotalDays%14 < 7)
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
            DateTime endTime = new DateTime();
            //if !check in filled, fill it
            if (isStationOpen(start, end, 1, 1))
            {
                scheduleEmployee(employee, start, end, 1);
            }
            /*else if (isStationOpenAtBeginning(start, end, out endTime, 1, 1))
            {
                scheduleEmployee(employee, start, endTime, 1);
            }*/
            //else if !check out filled, fill it
            else if (isStationOpen(start, end, 5, 1))
            {
                scheduleEmployee(employee, start, end, 5);
            }
            /*else if (isStationOpenAtBeginning(start, end, out endTime, 5, 1))
            {
                scheduleEmployee(employee, start, endTime, 5);
            }*/
            //else if !POD 1/2 filled, fill it
            else if (isStationOpen(start, end, 6, 2))
            {
                scheduleEmployee(employee, start, end, 6);
            }
            /*else if (isStationOpenAtBeginning(start, end, out endTime, 6, 2))
            {
                scheduleEmployee(employee, start, endTime, 6);
            }*/
            //else if !POD 3/4 filled, fill it
            else if (isStationOpen(start, end, 7, 2))
            {
                scheduleEmployee(employee, start, end, 7);
            }
            /*else if (isStationOpenAtBeginning(start, end, out endTime, 7, 2))
            {
                scheduleEmployee(employee, start, endTime, 7);
            }*/
            //else if !WOW 1/2 filled, fill it
            else if (isStationOpen(start, end, 3, 2))
            {
                scheduleEmployee(employee, start, end, 3);
            }
            //else if (isStationOpenAtBeginning(start, end, out endTime, 3, 2))
            //{
            //    scheduleEmployee(employee, start, endTime, 3);
            //}
            //else if !WOW 3/4 filled, fill it
            else if (isStationOpen(start, end, 4, 2))
            {
                scheduleEmployee(employee, start, end, 4);
            }
            //else if (isStationOpenAtBeginning(start, end, out endTime, 4, 2))
            //{
            //    scheduleEmployee(employee, start, endTime, 4);
            //}
            /*
             * else if (isStationOpen(start, end, 8, 1) && start >= **StartTime**)
            {
                scheduleEmployee(employee, start, end, 8);
            }
             */
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
        private int GenerateDigit(int num)
        {
            return random.Next(num);
        }

        private bool isStationOpen(DateTime start, DateTime end, int station, int num)
        {
            List<object> checkerList = new List<object>();
            //String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";
            String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";

            new idMaker(sqlString, checkerList);
            return checkerList.Count < num;
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
        private List<OffClocking> whoHasVacation(DateTime start, DateTime end, int nothing)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
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
                foreach (OffClocking clocking in returnList)
                {
                    if (clocking.id == employee)
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
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "timeend as time) > cast('" + start + "' as time) AND cast(A." + day + "timeend as time) < cast('" + end + "' as time) AND (A.Role = 2 OR A.Role = 3) AND NOT (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<OffClocking> whoWorksDay2OnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "timeend as time) > cast('" + start + "' as time) AND cast(A." + day + "timeend as time) < cast('" + end + "' as time) AND (A.Role = 2 OR A.Role = 3) AND (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }

        public List<object> whoAlreadyWorks(DateTime start, DateTime end)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE (A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "')) GROUP BY B.[Id];";

            new idMaker(sqlString, checkerList);
            
            return checkerList;
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
            DateTime endTime = startTime.AddHours(2);

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

                foreach(object employee in employeesThatCanWork)
                {
                    DateTime end = startTime.AddMinutes(30 * random.Next(2, 4));
                    schedule.scheduleInBestWorkstation(startTime, end, Convert.ToInt32(employee));
                }

                //increment times
                startTime = startTime.AddMinutes(30);
                endTime = startTime.AddHours(2);
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