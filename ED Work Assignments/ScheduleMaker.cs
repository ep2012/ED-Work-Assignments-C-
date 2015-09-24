using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    /*
    public class EmployeeScheduleCreator2
    {
        public EmployeeScheduleCreator2(DateTime day)
        {
            String sqlString = "SELECT [REVINT].[dbo].[ED_Roles].Title ,[REVINT].[dbo].[ED_Employees].[Id] FROM [REVINT].[dbo].[ED_Employees] JOIN [REVINT].[dbo].[ED_Roles] ON [REVINT].[dbo].[ED_Roles].Id = [REVINT].[dbo].[ED_Employees].Role WHERE [REVINT].[dbo].[ED_Employees].CurrentlyEmployed = 'true' AND [REVINT].[dbo].[ED_Roles].Title = 'Employee' ORDER BY NEWID();";
            //Loop over all 24 hours of the day for check in and pod 1/2 and make sure that there is someone there 24 hours a day, selecting a random person with an open shift to fill it

            //Loop over all remaining employee work hours and allocate to level 2 (if not full) or level 3 if full
        }
        private void scheduleDay(int employee, String startTime, String endTime, String day)
        {
            DateTime start;
            DateTime end;
            if (!DateTime.TryParse(day + startTime, out start) || !DateTime.TryParse(day + endTime, out end))
            {
                return;
            }
            else
            {
                if (start == end)
                {
                    return;
                }
                //break up and schedule

            }
        }
        private void clearDay(DateTime day)
        {

        }
    }
    public class SupervisorScheduleCreator2
    {
        public SupervisorScheduleCreator2(DateTime day)
        {
            List<object> supervisors = new List<object>();
            String sqlString = "SELECT [REVINT].[dbo].[ED_Roles].Title ,[REVINT].[dbo].[ED_Employees].[Id] FROM [REVINT].[dbo].[ED_Employees] JOIN [REVINT].[dbo].[ED_Roles] ON [REVINT].[dbo].[ED_Roles].Id = [REVINT].[dbo].[ED_Employees].Role WHERE [REVINT].[dbo].[ED_Employees].CurrentlyEmployed = 'true' AND [REVINT].[dbo].[ED_Roles].Title = 'Supervisor' ORDER BY NEWID();";
            new idMaker(sqlString, supervisors);
            //foreach employee on each day breakup and schedule
        }
        private void breakUpDayAndSchedule(int employee, String startTime, String endTime, String day)
        {
            DateTime start;
            DateTime end;
            if (!DateTime.TryParse(day + startTime, out start) || !DateTime.TryParse(day + endTime, out end))
            {
                return;
            }
            else
            {
                /*
                if (start == end)
                {
                    return;
                }
                //First add all hours to supervisor schedule before scheduling
                schedule.scheduleSupervisor(employee, start, end);
                //break up and schedule
                DateTime tempStart = start;
                if (tempStart.Subtract(end).TotalHours > 3)
                {
                    while (tempStart.Subtract(end).TotalHours > 3)
                    {
                        DateTime tempEnd = tempStart.AddHours(3);

                        schedule.scheduleClocking(employee, tempStart, tempEnd);
                        tempStart = tempEnd;
                    }
                    schedule.scheduleClocking(employee, tempStart, end);
                }
                else
                {
                    schedule.scheduleClocking(employee, start, end);
                }
                 */
    /*
            }
        }
    }

    //1 = Check In & POD 1/2
    //2 = Check Out & POD 3/4
    //3 = All others Excluding Supervising
    //if !1 filled, fill 1. If not 2 filled, fill 2. Else give to random 3.
    public class Schedule2
    {

        public Schedule2(DateTime start, DateTime end)
        {
            for (DateTime day = start; day < end; day.AddDays(1))
            {
                scheduleDay(day);
            }
        }
        public void scheduleDay(DateTime day)
        {
            new SupervisorScheduleCreator(day);
            new EmployeeScheduleCreator(day);
        }
        public int getBestWorkStation(int employee, DateTime start, DateTime end)
        {
            //if !check in filled and employee didn't work in station last, return that 

            //else if !POD 1/2 filled and employee didn't work in station last, return that

            //else if !check out filled and employee didn't work in station last, return that

            //else if !POD 3/4 filled and employee didn't work in station last, return that

            //else return random 3 level workstation

            return 0;
        }
        public void scheduleSupervisor(int employee, DateTime start, DateTime end)
        {

        }
        public bool isEmployeeFreeAndWorking(int employee, DateTime start, DateTime end)
        {
            //adjust to make free and working
            List<object> checkerList = new List<object>();
            String sqlString = "SELECT [REVINT].[dbo].[ED_Shifts].[Id] FROM [REVINT].[dbo].[ED_Shifts] JOIN [REVINT].[dbo].[ED_Employees] ON [REVINT].[dbo].[ED_Shifts].[Employee] = [REVINT].[dbo].[ED_Employees].[Id] WHERE [REVINT].[dbo].[ED_Employees].[Id] = " + employee + " AND [REVINT].[dbo].[ED_Shifts].[StartShift] < '" + start + "' AND [REVINT].[dbo].[ED_Shifts].[EndShift] > '" + end + "';";
            new idMaker(sqlString, checkerList);
            if (checkerList.Count == 0)
                return false;
            else
                return true;
        }
        public bool doesEmployeeWorkAtStationLast(int employee, int station, DateTime start, DateTime end)
        {
            return false;
        }
        public List<object> whoWorks(DateTime start, DateTime end)
        {
            List<object> checkerList = new List<object>();
            String sqlString = "SELECT [REVINT].[dbo].[ED_Employees].[Id] FROM [REVINT].[dbo].[ED_Shifts] JOIN [REVINT].[dbo].[ED_Employees] ON [REVINT].[dbo].[ED_Shifts].[Employee] = [REVINT].[dbo].[ED_Employees].[Id] WHERE [REVINT].[dbo].[ED_Shifts].[StartShift] < '" + start + "' AND [REVINT].[dbo].[ED_Shifts].[EndShift] > '" + end + "' GROUP BY [REVINT].[dbo].[ED_Employees].[Id];";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
    }

    public class BestWorkStation2
    {
        public int workstation;
        private int employee;
        public BestWorkStation2(int emp)
        {
            workstation = -1;
            employee = emp;
        }
        public int getWorkStation(DateTime start, DateTime end)
        {
            int lastWorkStation = getLastWorkedStation(start);

            return -1;
        }
        private int getLastWorkedStation(DateTime endTime)
        {
            return -1;
        }
    }
}*/
}