using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    
    public class EmployeeShift
    {
        public List<Shift> shifts;
        public object employee;

        public EmployeeShift(object emp)
        {
            employee = emp;
            shifts = new List<Shift>();
        }
        
        public static void EmployeeShiftCreator(String sqlCmdIDStartEndDay, List<EmployeeShift> comprehensiveEmployeeShifts, DateTime date)
        {
            object[] objID = new object[40];
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection connectionID = new OdbcConnection(cxnString))
            {
                OdbcCommand commandID = new OdbcCommand(sqlCmdIDStartEndDay, connectionID);

                connectionID.Open();

                OdbcDataReader reader = commandID.ExecuteReader();

                while (reader.Read())
                {
                    int numCols = reader.GetValues(objID);

                    if (numCols == 4)
                    {
                        EmployeeShift employeeShift = new EmployeeShift(((object)objID[0]));
                        Shift shift = new Shift();

                        bool isTrue = false; 
                        Boolean.TryParse(((object)objID[3]).ToString(), out isTrue);

                        if (isTrue)
                        {
                            DateTime start = DateTime.Parse(date.ToShortDateString() + " " +((object)objID[1]).ToString());
                            DateTime end = DateTime.Parse(date.AddDays(1).ToShortDateString() + " " + ((object)objID[2]).ToString());
                            shift.shiftTimeSpan = end.Subtract(start);
                            shift.startTime = start;
                        }
                        else
                        {
                            DateTime start = DateTime.Parse(date.ToShortDateString() + " " + ((object)objID[1]).ToString());
                            DateTime end = DateTime.Parse(date.ToShortDateString() + " " + ((object)objID[2]).ToString());
                            shift.shiftTimeSpan = end.Subtract(start);
                            shift.startTime = start;
                        }

                        employeeShift.shifts.Add(shift);
                        comprehensiveEmployeeShifts.Add(employeeShift);

                    }
                }
            }
        }
    }   
}
