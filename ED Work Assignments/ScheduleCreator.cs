using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                }
            }
        }
        public void scheduleDay(DateTime day, String dateStr)
        {
            new SupervisorScheduleCreator(day, dateStr);
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

        public void scheduleEndPiece(DateTime start, DateTime end, int employee)
        {
            if (GenerateDigit(2) == 1)
            {
                if (GenerateDigit(2) == 1)
                {
                    if (GenerateDigit(3) == 1)
                    {
                        scheduleEmployee(employee, start, end, 2);
                    }
                    else
                    {
                        scheduleEmployee(employee, start, end, 1);
                    }
                }
                else
                {
                    if (GenerateDigit(3) == 1)
                    {
                        scheduleEmployee(employee, start, end, 8);
                    }
                    else
                    {
                        scheduleEmployee(employee, start, end, 5);
                    }
                }
            }
            else
            {
                if (GenerateDigit(2) == 1)
                {
                    if (GenerateDigit(3) == 1)
                    {
                        scheduleEmployee(employee, start, end, 3);
                    }
                    else
                    {
                        scheduleEmployee(employee, start, end, 6);
                    }
                }
                else
                {
                    if (GenerateDigit(3) == 1)
                    {
                        scheduleEmployee(employee, start, end, 4);
                    }
                    else
                    {
                        scheduleEmployee(employee, start, end, 7);
                    }
                }
            }
        }
        public void scheduleInBestWorkstation(DateTime start, DateTime end, int employee)
        {
            //if !check in filled, fill it
            if (isStationOpen(start, end, 1))
            {
                scheduleEmployee(employee, start, end, 1);
            }
            //else if !POD 1/2 filled, fill it
            else if (isStationOpen(start, end, 6))
            {
                scheduleEmployee(employee, start, end, 6);
            }
            //else if !check out filled, fill it
            else if (isStationOpen(start, end, 5))
            {
                scheduleEmployee(employee, start, end, 5);
            }
            //else if !POD 3/4 filled, fill it
            else if (isStationOpen(start, end, 7))
            {
                scheduleEmployee(employee, start, end, 7);
            }
            //else return random 3 level workstation
            else 
            { 
                if (GenerateDigit(2) == 1)
                {
                    if (GenerateDigit(2) == 1)
                    {
                        if (GenerateDigit(3) == 1)
                        {
                            scheduleEmployee(employee, start, end, 2);
                        }
                        else
                        {
                            scheduleEmployee(employee, start, end, 1);
                        }
                    }
                    else
                    {
                        if (GenerateDigit(3) == 1)
                        {
                            scheduleEmployee(employee, start, end, 8);
                        }
                        else
                        {
                            scheduleEmployee(employee, start, end, 5);
                        }
                    }
                }
                else
                {
                    if (GenerateDigit(2) == 1)
                    {
                        if (GenerateDigit(3) == 1)
                        {
                            scheduleEmployee(employee, start, end, 3);
                        }
                        else
                        {
                            scheduleEmployee(employee, start, end, 6);
                        }
                    }
                    else
                    {
                        if (GenerateDigit(3) == 1)
                        {
                            scheduleEmployee(employee, start, end, 4);
                        }
                        else
                        {
                            scheduleEmployee(employee, start, end, 7);
                        }
                    }
                }
            }
        }
        private int GenerateDigit(int num)
        {
            return random.Next(num);
        }

        private bool isStationOpen(DateTime start, DateTime end, int station)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE A.[StartShift] <= '" + start + "' AND A.[EndShift] >= '" + end + "' AND A.Seat = " + station + " GROUP BY B.[Id];";
            new idMaker(sqlString, checkerList);
            return checkerList.Count == 0;
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
        public List<object> whoCanWorkSupervisor(DateTime start, DateTime end, String day)
        {
            return whoWorksSupervisor(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start, end)).ToList();
        }
        public List<object> whoCanWorkDay2(DateTime start, DateTime end, String day)
        {
            return whoWorksDay2(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start, end)).ToList();
        }
        public List<object> whoCanWorkSupervisorDay2(DateTime start, DateTime end, String day)
        {
            return whoWorksSupervisorDay2(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start, end)).ToList();
        }
        
        public List<OffClocking> whoCanWorkOnlyStart(DateTime start, DateTime end, String day)
        {
            return whoWorksOnlyStart(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }
        public List<OffClocking> whoCanWorkSupervisorOnlyStart(DateTime start, DateTime end, String day)
        {
            return whoWorksSupervisorOnlyStart(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }

        public List<OffClocking> whoCanWorkOnlyEnd(DateTime start, DateTime end, String day)
        {
            return whoWorksOnlyEnd(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }
        public List<OffClocking> whoCanWorkSupervisorOnlyEnd(DateTime start, DateTime end, String day)
        {
            return whoWorksSupervisorOnlyEnd(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }
        public List<OffClocking> whoCanWorkDay2OnlyStart(DateTime start, DateTime end, String day)
        {
            return whoWorksDay2OnlyStart(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }
        public List<OffClocking> whoCanWorkSupervisorDay2OnlyStart(DateTime start, DateTime end, String day)
        {
            return whoWorksSupervisorDay2OnlyStart(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }
        
        public List<object> whoWorks(DateTime start, DateTime end, String day)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "time as time) <= cast('" + start + "' as time) AND cast(A." + day + "timeend as time) >= cast('" + end + "' as time)) OR (cast(A." + day + "time as time) <= cast('" + start + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 2 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<object> whoWorksSupervisor(DateTime start, DateTime end, String day)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "time as time) <= cast('" + start + "' as time) AND cast(A." + day + "timeend as time) >= cast('" + end + "' as time)) OR (cast(A." + day + "time as time) <= cast('" + start + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 3 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<object> whoWorksDay2(DateTime start, DateTime end, String day)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 2 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<object> whoWorksSupervisorDay2(DateTime start, DateTime end, String day)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 3 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }

        public List<OffClocking> whoWorksOnlyEnd(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"time as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "time as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 2 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<OffClocking> whoWorksSupervisorOnlyEnd(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"time as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "time as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 3 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }


        public List<OffClocking> whoWorksOnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "timeend as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 2 AND NOT (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<OffClocking> whoWorksSupervisorOnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "timeend as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 3 AND NOT (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<OffClocking> whoWorksDay2OnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "timeend as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 2 AND (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<OffClocking> whoWorksSupervisorDay2OnlyStart(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "timeend as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 3 AND (A." + day + "day = 'True') ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }


        /*public List<object> whoWorksDay2OnlyStart(DateTime start, DateTime end, String day, List<object> days)
{
    List<object> checkerList = new List<object>();
    String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 2 ORDER BY NEWID();";
    new idMaker(sqlString, checkerList, days);
    return checkerList;
}
public List<object> whoWorksSupervisorDay2OnlyStart(DateTime start, DateTime end, String day, List<object> days)
{
    List<object> checkerList = new List<object>();
    String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 3 ORDER BY NEWID();";
    new idMaker(sqlString, checkerList, days);
    return checkerList;
}*/
        public bool doesEmployeeWorkAtStationLast(int employee, int station, DateTime start, DateTime end)
        {
            return false;
        }
        public List<object> whoAlreadyWorks(DateTime start, DateTime end)
        {
            List<object> checkerList = new List<object>();
            String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE A.[StartShift] <= '" + start + "' AND A.[EndShift] >= '" + end + "' GROUP BY B.[Id];";
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
        public EmployeeScheduleCreator(DateTime day, String dayStr)
        {
            DateTime goaldate = day.AddHours(36);
            DateTime startTime = day;
            DateTime endTime = day.AddHours(2);
            while(endTime < goaldate)
            {
                List<object> employeesThatCanWork = new List<object>();
                List<OffClocking> employeesThatCanWorkLastPart = new List<OffClocking>();
                List<OffClocking> employeesThatCanWorkFirstPart = new List<OffClocking>();

                //get employees that can work
                if ((startTime - day).TotalHours < 24)
                {
                    employeesThatCanWork = schedule.whoCanWork(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkOnlyStart(startTime, endTime, dayStr);
                    employeesThatCanWorkLastPart = schedule.whoCanWorkOnlyEnd(startTime, endTime, dayStr);
                }
                else
                {
                    employeesThatCanWork = schedule.whoCanWorkDay2(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkDay2OnlyStart(startTime, endTime, dayStr);
                }
                
                //schedule employees
                foreach(object employee in employeesThatCanWork)
                {
                    schedule.scheduleInBestWorkstation(startTime, endTime,Convert.ToInt32(employee));
                }
                foreach (OffClocking clockings in employeesThatCanWorkFirstPart)
                {
                    DateTime end = DateTime.Parse(endTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(startTime, end, Convert.ToInt32(clockings.id));
                }
                foreach (OffClocking clockings in employeesThatCanWorkLastPart)
                {
                    DateTime start = DateTime.Parse(startTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(start, endTime, Convert.ToInt32(clockings.id));
                }
                

                //increment times
                startTime = endTime;
                endTime = endTime.AddHours(2);
            }
        }
    }



    public class SupervisorScheduleCreator
    {
        Schedule schedule = new Schedule();

        public SupervisorScheduleCreator(DateTime day, String dayStr)
        {
            DateTime goaldate = day.AddDays(1);
            DateTime startTime = day;
            DateTime endTime = day.AddHours(2);
            while (endTime < goaldate)
            {
                List<object> employeesThatCanWork = new List<object>();
                List<OffClocking> employeesThatCanWorkLastPart = new List<OffClocking>();
                List<OffClocking> employeesThatCanWorkFirstPart = new List<OffClocking>();

                //get employees that can work
                if ((startTime - day).TotalHours < 24)
                {
                    employeesThatCanWork = schedule.whoCanWorkSupervisor(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkSupervisorOnlyStart(startTime, endTime, dayStr);
                    employeesThatCanWorkLastPart = schedule.whoCanWorkSupervisorOnlyEnd(startTime, endTime, dayStr);
                }
                else
                {
                    employeesThatCanWork = schedule.whoCanWorkSupervisorDay2(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkSupervisorDay2OnlyStart(startTime, endTime, dayStr);
                }

                //schedule employees
                foreach (object employee in employeesThatCanWork)
                {
                    schedule.scheduleInBestWorkstation(startTime, endTime, Convert.ToInt32(employee));
                    schedule.scheduleSupervisor(startTime, endTime, Convert.ToInt32(employee));
                }
                foreach (OffClocking clockings in employeesThatCanWorkFirstPart)
                {
                    DateTime end = DateTime.Parse(endTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(startTime, end, Convert.ToInt32(clockings.id));
                    schedule.scheduleSupervisor(startTime, end, Convert.ToInt32(clockings.id));
                }
                foreach (OffClocking clockings in employeesThatCanWorkLastPart)
                {
                    DateTime start = DateTime.Parse(startTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(start, endTime, Convert.ToInt32(clockings.id));
                    schedule.scheduleSupervisor(start, endTime, Convert.ToInt32(clockings.id));
                }


                //increment times
                startTime = endTime;
                endTime = endTime.AddHours(2);
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