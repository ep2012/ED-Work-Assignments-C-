using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ED_Work_Assignments;

namespace EDWorkAssignmentsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ScheduleEmployeeTest()
        {
            Schedule schedule = new Schedule();
            TempScheduler.clear();
            DateTime day = DateTime.Parse("11/5/2015 00:00");
            int maxEmployeesAtATime = 11;
            //object employeeId = (object)"1";
            TimeSpan employeeTimeSpan = DateTime.Now.AddHours(6) - DateTime.Now;
            DateTime startTime = DateTime.Parse("11/5/2015 08:00");
            DateTime startTime2 = DateTime.Parse("11/5/2015 17:00");
            System.Collections.Generic.List<DateTime> startTimes = new System.Collections.Generic.List<DateTime>();

            startTimes.Add(startTime);
            startTimes.Add(startTime2);

            System.Collections.Generic.List<EmployeeShift> employeeShiftsList = new System.Collections.Generic.List<EmployeeShift>();
            int numEmployeesAtTime = 15;
            for (int i = 1; i < (numEmployeesAtTime + 1); i++)
            {
                createEmployeeShift((object)i, employeeTimeSpan, startTime, employeeShiftsList);
                createEmployeeShift((object)(i + numEmployeesAtTime), employeeTimeSpan, startTime2, employeeShiftsList);
            }
            ScheduleMaker scheduleMaker = new ScheduleMaker();

            scheduleMaker.scheduleDay(day, employeeShiftsList);

            for (DateTime start = startTime; start < startTime.Add(employeeTimeSpan); start = start.AddMinutes(30))
            {
                if (numEmployeesAtTime > maxEmployeesAtATime)
                {
                    Assert.AreEqual(expected: maxEmployeesAtATime, actual: schedule.getNumberEmployeesWorking(start, start.AddMinutes(30)), message: start.ToString());
                }
                else
                {
                    Assert.AreEqual(expected: numEmployeesAtTime, actual: schedule.getNumberEmployeesWorking(start, start.AddMinutes(30)), message: start.ToString());
                }
            }
        }

        public void createEmployeeShift(object employeeId,TimeSpan timeSpan, DateTime startTime, System.Collections.Generic.List<EmployeeShift> employeeShiftsList)
        {
            EmployeeShift employeeShift = new EmployeeShift(employeeId);

            Shift shift = new Shift();

            shift.shiftTimeSpan = timeSpan;
            shift.startTime = startTime;

            employeeShift.shifts.Add(shift);

            employeeShiftsList.Add(employeeShift);
        }
    }
}
