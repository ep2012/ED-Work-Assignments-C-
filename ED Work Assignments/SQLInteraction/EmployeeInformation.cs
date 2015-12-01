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
        static String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

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

    }
}
