using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public class EmployeeScheduleCreator
    {
        Schedule schedule = new Schedule();
        Random random = new Random();

        public EmployeeScheduleCreator(DateTime day, List<EmployeeShift> employeeShifts)
        {
            DateTime startTime = day;
            DateTime goaldate = startTime.AddHours(36);
            DateTime endTime;
            
            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 14));

                schedule.scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.Check);
                schedule.scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.PartCheck);

                //increment time
                startTime = startTime.AddMinutes(30);
            }
            
            
            //reset startTime
            startTime = day;

            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 14));

                schedule.scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.Fill);
                schedule.scheduleWhoWorks(startTime, endTime, employeeShifts, SchedulingType.Part);

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
