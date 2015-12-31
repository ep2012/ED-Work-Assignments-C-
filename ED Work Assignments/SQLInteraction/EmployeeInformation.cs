using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public static class EmployeeInformationSQL
    {
        private static String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        public static void add(object firstName, object lastName, object role, object address1, object address2, object city, object state, object zip, object phone, object email, object healthcareID)
        {
            Roles roles = new Roles();

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].employee_insert(?,?,?,?,?,?,?,?,?,?,?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@FirstName", OdbcType.NVarChar, 50).Value = firstName;
                cmd.Parameters.Add("@LastName", OdbcType.NVarChar, 50).Value = lastName;
                cmd.Parameters.Add("@Role", OdbcType.Int).Value = roles.getID(role.ToString());
                cmd.Parameters.Add("@Address1", OdbcType.NVarChar, 100).Value = address1;
                cmd.Parameters.Add("@Address2", OdbcType.NVarChar, 100).Value = address2;
                cmd.Parameters.Add("@City", OdbcType.NVarChar, 50).Value = city;
                cmd.Parameters.Add("@State", OdbcType.NVarChar, 50).Value = state;
                cmd.Parameters.Add("@Zip", OdbcType.NVarChar, 50).Value = zip;
                cmd.Parameters.Add("@Phone", OdbcType.NVarChar, 50).Value = phone;
                cmd.Parameters.Add("@Email", OdbcType.NVarChar, 100).Value = email;
                cmd.Parameters.Add("@UserName", OdbcType.NVarChar, 50).Value = healthcareID;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }

            ChangeTrackerSQL.add("Added Employee: \n" +
                    "Name: " + firstName.ToString() + " " + lastName.ToString() + "\n" +
                    "Role: " + role.ToString() + "\n" +
                    "Address 1: " + address1.ToString() + "\n" +
                    "Address 2: " + address2.ToString() + "\n" +
                    "City: " + city.ToString() + "\n" +
                    "State: " + state.ToString() + "\n" +
                    "Zip: " + zip.ToString() + "\n" +
                    "Phone: " + phone.ToString() + "\n" +
                    "Email: " + email.ToString() + "\n" +
                    "Healthcare ID: " + healthcareID.ToString());

        }
        public static void update(object firstName, object lastName, object role, object address1, object address2, object city, object state, object zip, object phone, object email, object healthcareID, object id)
        {
            Roles roles = new Roles();

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].employee_update(?,?,?,?,?,?,?,?,?,?,?,?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@FirstName", OdbcType.NVarChar, 50).Value = firstName;
                cmd.Parameters.Add("@LastName", OdbcType.NVarChar, 50).Value = lastName;
                cmd.Parameters.Add("@Role", OdbcType.Int).Value = roles.getID(role.ToString());
                cmd.Parameters.Add("@Address1", OdbcType.NVarChar, 100).Value = address1;
                cmd.Parameters.Add("@Address2", OdbcType.NVarChar, 100).Value = address2;
                cmd.Parameters.Add("@City", OdbcType.NVarChar, 50).Value = city;
                cmd.Parameters.Add("@State", OdbcType.NVarChar, 50).Value = state;
                cmd.Parameters.Add("@Zip", OdbcType.NVarChar, 50).Value = zip;
                cmd.Parameters.Add("@Phone", OdbcType.NVarChar, 50).Value = phone;
                cmd.Parameters.Add("@Email", OdbcType.NVarChar, 100).Value = email;
                cmd.Parameters.Add("@UserName", OdbcType.NVarChar, 50).Value = healthcareID;
                cmd.Parameters.Add("@Id", OdbcType.Int).Value = id;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }

            ChangeTrackerSQL.add("Updated Employee: \n" +
                    "Name: " + firstName.ToString() + " " + lastName.ToString() + "\n" +
                    "Role: " + role.ToString() + "\n" +
                    "Address 1: " + address1.ToString() + "\n" +
                    "Address 2: " + address2.ToString() + "\n" +
                    "City: " + city.ToString() + "\n" +
                    "State: " + state.ToString() + "\n" +
                    "Zip: " + zip.ToString() + "\n" +
                    "Phone: " + phone.ToString() + "\n" +
                    "Email: " + email.ToString() + "\n" +
                    "Healthcare ID: " + healthcareID.ToString());
        }
        public static void updateSchedule(object sunday1time, object sunday1timeend, object sunday1day,
            object monday1time, object monday1timeend, object monday1day,
            object tuesday1time, object tuesday1timeend, object tuesday1day,
            object wednesday1time, object wednesday1timeend, object wednesday1day,
            object thursday1time, object thursday1timeend, object thursday1day,
            object friday1time, object friday1timeend, object friday1day,
            object saturday1time, object saturday1timeend, object saturday1day,
            object sunday2time, object sunday2timeend, object sunday2day,
            object monday2time, object monday2timeend, object monday2day,
            object tuesday2time, object tuesday2timeend, object tuesday2day,
            object wednesday2time, object wednesday2timeend, object wednesday2day,
            object thursday2time, object thursday2timeend, object thursday2day,
            object friday2time, object friday2timeend, object friday2day,
            object saturday2time, object saturday2timeend, object saturday2day, 
            object fulltime, 
            object id)
        {
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].employee_updateSchedule("+
                    "?,?,?,?,?,?,?,?,?,?,?," +
                    "?,?,?,?,?,?,?,?,?,?,?," + 
                    "?,?,?,?,?,?,?,?,?,?,?," +
                    "?,?,?,?,?,?,?,?,?,?,?"+
                    ")}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@sunday1time", OdbcType.DateTime).Value = sunday1time;
                cmd.Parameters.Add("@sunday1timeend", OdbcType.DateTime).Value = sunday1timeend;
                cmd.Parameters.Add("@sunday1day", OdbcType.Bit).Value = sunday1day;

                cmd.Parameters.Add("@monday1time", OdbcType.DateTime).Value = monday1time;
                cmd.Parameters.Add("@monday1timeend", OdbcType.DateTime).Value = monday1timeend;
                cmd.Parameters.Add("@monday1day", OdbcType.Bit).Value = monday1day;

                cmd.Parameters.Add("@tuesday1time", OdbcType.DateTime).Value = tuesday1time;
                cmd.Parameters.Add("@tuesday1timeend", OdbcType.DateTime).Value = tuesday1timeend;
                cmd.Parameters.Add("@tuesday1day", OdbcType.Bit).Value = tuesday1day;

                cmd.Parameters.Add("@wednesday1time", OdbcType.DateTime).Value = wednesday1time;
                cmd.Parameters.Add("@wednesday1timeend", OdbcType.DateTime).Value = wednesday1timeend;
                cmd.Parameters.Add("@wednesday1day", OdbcType.Bit).Value = wednesday1day;

                cmd.Parameters.Add("@thursday1time", OdbcType.DateTime).Value = thursday1time;
                cmd.Parameters.Add("@thursday1timeend", OdbcType.DateTime).Value = thursday1timeend;
                cmd.Parameters.Add("@thursday1day", OdbcType.Bit).Value = thursday1day;

                cmd.Parameters.Add("@friday1time", OdbcType.DateTime).Value = friday1time;
                cmd.Parameters.Add("@friday1timeend", OdbcType.DateTime).Value = friday1timeend;
                cmd.Parameters.Add("@friday1day", OdbcType.Bit).Value = friday1day;

                cmd.Parameters.Add("@saturday1time", OdbcType.DateTime).Value = saturday1time;
                cmd.Parameters.Add("@saturday1timeend", OdbcType.DateTime).Value = saturday1timeend;
                cmd.Parameters.Add("@saturday1day", OdbcType.Bit).Value = saturday1day;

                cmd.Parameters.Add("@sunday2time", OdbcType.DateTime).Value = sunday2time;
                cmd.Parameters.Add("@sunday2timeend", OdbcType.DateTime).Value = sunday2timeend;
                cmd.Parameters.Add("@sunday2day", OdbcType.Bit).Value = sunday2day;

                cmd.Parameters.Add("@monday2time", OdbcType.DateTime).Value = monday2time;
                cmd.Parameters.Add("@monday2timeend", OdbcType.DateTime).Value = monday2timeend;
                cmd.Parameters.Add("@monday2day", OdbcType.Bit).Value = monday2day;

                cmd.Parameters.Add("@tuesday2time", OdbcType.DateTime).Value = tuesday2time;
                cmd.Parameters.Add("@tuesday2timeend", OdbcType.DateTime).Value = tuesday2timeend;
                cmd.Parameters.Add("@tuesday2day", OdbcType.Bit).Value = tuesday2day;

                cmd.Parameters.Add("@wednesday2time", OdbcType.DateTime).Value = wednesday2time;
                cmd.Parameters.Add("@wednesday2timeend", OdbcType.DateTime).Value = wednesday2timeend;
                cmd.Parameters.Add("@wednesday2day", OdbcType.Bit).Value = wednesday2day;

                cmd.Parameters.Add("@thursday2time", OdbcType.DateTime).Value = thursday2time;
                cmd.Parameters.Add("@thursday2timeend", OdbcType.DateTime).Value = thursday2timeend;
                cmd.Parameters.Add("@thursday2day", OdbcType.Bit).Value = thursday2day;

                cmd.Parameters.Add("@friday2time", OdbcType.DateTime).Value = friday2time;
                cmd.Parameters.Add("@friday2timeend", OdbcType.DateTime).Value = friday2timeend;
                cmd.Parameters.Add("@friday2day", OdbcType.Bit).Value = friday2day;

                cmd.Parameters.Add("@saturday2time", OdbcType.DateTime).Value = saturday2time;
                cmd.Parameters.Add("@saturday2timeend", OdbcType.DateTime).Value = saturday2timeend;
                cmd.Parameters.Add("@saturday2day", OdbcType.Bit).Value = saturday2day;

                cmd.Parameters.Add("@fulltime", OdbcType.Bit).Value = fulltime;
                
                cmd.Parameters.Add("@Id", OdbcType.Int).Value = id;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }

            /*
            ChangeTrackerSQL.add("Updated Employee Schedule: \n" +
                    "Name: " + firstName.ToString() + " " + lastName.ToString() + "\n" +
                    "Role: " + "\n" +
                    "Address 1: " + address1.ToString() + "\n" +
                    "Address 2: " + address2.ToString() + "\n" +
                    "City: " + city.ToString() + "\n" +
                    "State: " + state.ToString() + "\n" +
                    "Zip: " + zip.ToString() + "\n" +
                    "Phone: " + phone.ToString() + "\n" +
                    "Email: " + email.ToString() + "\n" +
                    "Healthcare ID: " + healthcareID.ToString());
             */
        }
        public static void delete(object firstName, object lastName, object role, object address1, object address2, object city, object state, object zip, object phone, object email, object healthcareID, object id)
        {
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].delete_employee(?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@Id", OdbcType.Int).Value = id;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
            ChangeTrackerSQL.add("Deleted Employee: \n" +
                    "Name: " + firstName.ToString() + " " + lastName.ToString() + "\n" +
                    "Role: " + role.ToString() + "\n" +
                    "Address 1: " + address1.ToString() + "\n" +
                    "Address 2: " + address2.ToString() + "\n" +
                    "City: " + city.ToString() + "\n" +
                    "State: " + state.ToString() + "\n" +
                    "Zip: " + zip.ToString() + "\n" +
                    "Phone: " + phone.ToString() + "\n" +
                    "Email: " + email.ToString() + "\n" +
                    "Healthcare ID: " + healthcareID.ToString());
        }
    }

    public static class EmployeeScheduleSQL 
    {
        private static String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        public static void getSchedules(object id, out List<object> values)
        {
            values = new List<object>();
            new idMaker("SELECT FirstName, LastName "+
                ", sunday1time, sunday1timeend, sunday1day "+
                ", monday1time, monday1timeend, monday1day "+
                ", tuesday1time, tuesday1timeend, tuesday1day " +
                ", wednesday1time, wednesday1timeend, wednesday1day " +
                ", thursday1time, thursday1timeend, thursday1day " +
                ", friday1time, friday1timeend, friday1day " +
                ", saturday1time, saturday1timeend, saturday1day " +
                ", sunday2time, sunday2timeend, sunday2day " +
                ", monday2time, monday2timeend, monday2day " +
                ", tuesday2time, tuesday2timeend, tuesday2day " +
                ", wednesday2time, wednesday2timeend, wednesday2day " +
                ", thursday2time, thursday2timeend, thursday2day " +
                ", friday2time, friday2timeend, friday2day " +
                ", saturday2time, saturday2timeend, saturday2day " +
                "FROM [REVINT].[healthcare\\eliprice].[ED_Employees] A WHERE Id = " + id.ToString(), values);
        }
        public static void deleteClocking(object id)
        {
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_deleteWorkAssignment(?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@id", OdbcType.Int).Value = id.ToString();

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }

        public static void updateClocking(object employeeID, object seatID, object start, object end, object id)
        {

            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateWorkAssignment(?,?,?,?,?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@employee", OdbcType.Int).Value = employeeID;
                cmd.Parameters.Add("@seat", OdbcType.Int).Value = seatID;
                cmd.Parameters.Add("@start", OdbcType.DateTime).Value = start;
                cmd.Parameters.Add("@end", OdbcType.DateTime).Value = end;
                cmd.Parameters.Add("@id", OdbcType.Int).Value = id;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }

        public static void addClocking(object employeeID, object seatID, object start, object end)
        {
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_newWorkAssignment(?, ?, ?, ?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@employee", OdbcType.Int).Value = employeeID;
                cmd.Parameters.Add("@seat", OdbcType.Int).Value = seatID;
                cmd.Parameters.Add("@start", OdbcType.DateTime).Value = start;
                cmd.Parameters.Add("@end", OdbcType.DateTime).Value = end;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }
    }
}
