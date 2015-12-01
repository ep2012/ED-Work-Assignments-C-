using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public static class ChangeTrackerSQL
    {
        static String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        public static void add(object notes)
        {
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateChangeTracker(?, ?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@username", OdbcType.NVarChar, 100).Value = Environment.UserName;
                cmd.Parameters.Add("@notes", OdbcType.NVarChar, 4000).Value = notes;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }
    }
}