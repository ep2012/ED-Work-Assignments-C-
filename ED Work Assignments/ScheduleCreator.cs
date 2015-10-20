using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ED_Work_Assignments
{
    public class EmployeeScheduleCreator
    {
        Schedule schedule = new Schedule();
        Random random = new Random();

        public EmployeeScheduleCreator(DateTime day, String dayStr)
        {
            DateTime goaldate = day.AddHours(36);
            DateTime startTime = day;
            DateTime endTime;

            schedule.scheduleSupervisors(day, dayStr);

            List<object> employeesThatCanWork = new List<object>();

            List<OffClocking> employeesThatCanWorkFirstPart = new List<OffClocking>();

            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 12));

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

                foreach (object employee in employeesThatCanWork)
                {
                    schedule.scheduleCheckInCheckOut(startTime, endTime, Convert.ToInt32(employee));
                }
                

                foreach (OffClocking clockings in employeesThatCanWorkFirstPart)
                {
                    DateTime end = DateTime.Parse(endTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleCheckInCheckOut(startTime, end, Convert.ToInt32(clockings.id));
                }

                //increment time
                startTime = startTime.AddMinutes(30);
            }

            startTime = day;
            while(startTime < goaldate)
            {
                endTime = startTime.AddMinutes(30 * random.Next(10, 12));

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
                    schedule.scheduleInBestWorkstation2(startTime,endTime, Convert.ToInt32(employee));
                }
                //increment time
                startTime = startTime.AddMinutes(30);
            }

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
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing)"+ 
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
                    schedule.putHoleInHighestPriorityStation(DateTime.Parse(day.ToShortDateString() + " " + date.ToString()));
                }

                minStaffingSlots = new List<object>();
                new idMaker(sqlString, minStaffingSlots, "");

                fullStaffing = new List<object>();
                new idMaker(sqlStringFullStaffing, fullStaffing, "");

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
    public idMaker(String sqlCmdID, List<SupervisorClocking> ids)
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

                if (numCols == 3)
                {
                    SupervisorClocking clocking = new SupervisorClocking();
                    clocking.id = (object)objID[0].ToString();
                    clocking.start = (object)objID[1].ToString();
                    clocking.end = (object)objID[2].ToString();
                    ids.Add(clocking);
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
public struct SupervisorClocking
{
    public object id;
    public object start;
    public object end;
}