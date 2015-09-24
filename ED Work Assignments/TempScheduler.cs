using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public class TempScheduler
    {
        Users users = new Users();
        Seats seats = new Seats();

        public void insert(int employee, DateTime start, DateTime end, int station)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_newTempWorkAssignment(?, ?, ?, ?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@employee", OdbcType.Int).Value = employee.ToString();
                cmd.Parameters.Add("@seat", OdbcType.Int).Value = station.ToString();
                cmd.Parameters.Add("@start", OdbcType.DateTime).Value = start;
                cmd.Parameters.Add("@end", OdbcType.DateTime).Value = end;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }
        public void clear()
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_deleteTempWorkAssignments}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }
    }
}
