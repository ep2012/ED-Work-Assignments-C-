using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
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
}
