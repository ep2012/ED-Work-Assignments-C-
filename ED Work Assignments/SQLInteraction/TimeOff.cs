using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public static class TimeOffSQL
    {
        public static void insertTimeOffRequest(object employeeId, object startTime, object endTime)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].[ED_insertTimeOffRequest](?, ?, ?, ?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@EmployeeId", OdbcType.Int).Value = employeeId;
                cmd.Parameters.Add("@StartTime", OdbcType.DateTime).Value = startTime;
                cmd.Parameters.Add("@EndTime", OdbcType.DateTime).Value = endTime;
                cmd.Parameters.Add("@DateTimeStamp", OdbcType.DateTime).Value = DateTime.Now;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
            ChangeTrackerSQL.add("Requested time off " + startTime.ToString() + " until " + endTime.ToString() + "\nRequested: " + DateTime.Now.ToString());
        }
        public static void insertTimeOff(object employeeId, object startTime, object endTime)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].[ED_insertTimeOff](?, ?, ?, ?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@EmployeeId", OdbcType.Int).Value = employeeId;
                cmd.Parameters.Add("@StartTime", OdbcType.DateTime).Value = startTime;
                cmd.Parameters.Add("@EndTime", OdbcType.DateTime).Value = endTime;
                cmd.Parameters.Add("@DateTimeStamp", OdbcType.DateTime).Value = DateTime.Now;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
            ChangeTrackerSQL.add("Added time off " + startTime.ToString() + " until " + endTime.ToString() + " for EmployeeId " + employeeId.ToString() + "\nAdded: " + DateTime.Now.ToString());
        }
        public static void acceptTimeOffRequest(object id)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].[ED_acceptTimeOffRequest](?, ?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@Id", OdbcType.Int).Value = id;
                cmd.Parameters.Add("@DateTimeStamp", OdbcType.DateTime).Value = DateTime.Now;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
            ChangeTrackerSQL.add("Accepted time off request - " + DateTime.Now);
        }
    }
}
