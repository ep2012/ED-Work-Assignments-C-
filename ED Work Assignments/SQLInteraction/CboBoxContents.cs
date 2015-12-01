using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public class Users : ObservableCollection<string>
    {
        Dictionary<String, String> employeeNames = new Dictionary<String, String>();
        Dictionary<String, String> employeeID = new Dictionary<String, String>();
        Dictionary<String, String> employeeRole = new Dictionary<String, String>();
        Dictionary<String, int> employeeComboBox = new Dictionary<String, int>();

        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
        
        object[] employeeUserNames = new object[1];
        object[] employeeFirstNames = new object[1];
        object[] employeeLastNames = new object[1];
        object[] employeeRoles = new object[1];
        object[] employeeEmployed = new object[1];
        object[] employeeIDs = new object[1];
        
        OdbcConnection connectionFirstName;
        OdbcConnection connectionLastName;
        OdbcConnection connectionUserName;
        OdbcConnection connectionRole;
        OdbcConnection connectionEmployed;
        OdbcConnection connectionID;

        public Users()
        {
            connectionLastName = new OdbcConnection(cxnString);
            connectionUserName = new OdbcConnection(cxnString);
            connectionRole = new OdbcConnection(cxnString);
            connectionEmployed = new OdbcConnection(cxnString);
            connectionID = new OdbcConnection(cxnString);

            int num = 0;

            using (connectionFirstName = new OdbcConnection(cxnString))
            {
                OdbcCommand commandFirstName = new OdbcCommand("SELECT FirstName FROM [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] ORDER BY FirstName, LastName", connectionFirstName);
                OdbcCommand commandLastName = new OdbcCommand("SELECT LastName FROM [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] ORDER BY FirstName, LastName", connectionLastName);
                OdbcCommand commandUserName = new OdbcCommand("SELECT UserName FROM [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] ORDER BY FirstName, LastName", connectionUserName);
                OdbcCommand commandRole = new OdbcCommand("SELECT Role FROM [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] ORDER BY FirstName, LastName", connectionRole);
                OdbcCommand commandEmployed = new OdbcCommand("SELECT CurrentlyEmployed FROM [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] ORDER BY FirstName, LastName", connectionEmployed);
                OdbcCommand commandID = new OdbcCommand("SELECT Id FROM [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] ORDER BY FirstName, LastName", connectionID);

                connectionFirstName.Open();
                connectionLastName.Open();
                connectionUserName.Open();
                connectionRole.Open();
                connectionEmployed.Open();
                connectionID.Open();

                OdbcDataReader firstNameReader = commandFirstName.ExecuteReader();
                OdbcDataReader lastNameReader = commandLastName.ExecuteReader();
                OdbcDataReader userNameReader = commandUserName.ExecuteReader();
                OdbcDataReader roleReader = commandRole.ExecuteReader();
                OdbcDataReader employedReader = commandEmployed.ExecuteReader();
                OdbcDataReader iDReader = commandID.ExecuteReader();

                while (firstNameReader.Read() && lastNameReader.Read() && userNameReader.Read() && iDReader.Read() && roleReader.Read() && employedReader.Read())
                {
                    int numCols = firstNameReader.GetValues(employeeFirstNames);
                    numCols = lastNameReader.GetValues(employeeLastNames);
                    numCols = userNameReader.GetValues(employeeUserNames);
                    numCols = roleReader.GetValues(employeeRoles);
                    numCols = employedReader.GetValues(employeeEmployed);
                    numCols = iDReader.GetValues(employeeIDs);

                    for (int i = 0; i < numCols; i++)
                    {
                        if (!employeeNames.ContainsKey(employeeUserNames[i].ToString()))
                        {
                            employeeNames.Add(employeeUserNames[i].ToString(), employeeFirstNames[i].ToString() + " " + employeeLastNames[i].ToString());
                        }
                        if (!employeeID.ContainsKey(employeeFirstNames[i].ToString() + " " + employeeLastNames[i].ToString()))
                        {
                            employeeID.Add(employeeFirstNames[i].ToString() + " " + employeeLastNames[i].ToString(), employeeIDs[i].ToString());
                        }
                        if (!employeeRole.ContainsKey(employeeUserNames[i].ToString()))
                        {
                            employeeRole.Add(employeeUserNames[i].ToString(), employeeRoles[i].ToString());
                        }
                        if (employeeEmployed[i].ToString().ToLower() == "true")
                        {
                            Add(employeeFirstNames[i].ToString() + " " + employeeLastNames[i].ToString());
                            employeeComboBox.Add(employeeFirstNames[i].ToString() + " " + employeeLastNames[i].ToString(), num);
                            num++;
                        }
                    }
                }                 

                connectionFirstName.Close();
                connectionLastName.Close();
                connectionUserName.Close();
                connectionRole.Close();
                connectionEmployed.Close();
                connectionID.Close();
            }
        }

        public String getName(string strUserName)
        {
            if (!employeeNames.ContainsKey(strUserName))
                return "Guest Employee";
            else
                return employeeNames[strUserName];
        }
        public String getID(string strName)
        {
            if (!employeeID.ContainsKey(strName))
            {
                //
                //
                //
                //Dialog Box
                //
                //
                //

                return "-1";
            }
            else
            {
                return employeeID[strName];
            }
        }
        public String getNameFromId(String id)
        {
            if (!employeeID.ContainsValue(id))
            {
                return "Unknown Username";
            }
            else
            {
                return employeeID.FirstOrDefault(x => x.Value.Contains(id)).Key;
            }
        }
        public int getComboIndex(String strName)
        {
            return employeeComboBox[strName]; 
        }

        public bool isAdmin(String userID)
        {
            if (employeeRole[userID] == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool userNameCanBeCreated(String userName)
        {
            if (employeeID.ContainsKey(userName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool userIDCanBeCreated(String userID)
        {
            if (employeeNames.ContainsKey(userID) && userID != "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public class Seats : ObservableCollection<string>
    {
        Dictionary<String, String> seatIDs = new Dictionary<String, String>();

        public Seats()
        {
            String seatSQL = "SELECT Id, Name FROM [REVINT].[dbo].[ED_Seats] ORDER BY Name";
            //String seatIDSQL = "SELECT Id FROM [REVINT].[dbo].[ED_Seats] ORDER BY Name";

            new newcomboMaker(seatSQL, seatIDs, this);
        }
        public String getID(string strName)
        {
            if (!seatIDs.ContainsKey(strName))
            {
                return "-1";
            }
            else
            {
                return seatIDs[strName];
            }
        }
    }
    public class BindingMaker : ObservableCollection<string>
    {
        Dictionary<String, String> seatIDs = new Dictionary<String, String>();

        public BindingMaker(String sqlString)
        {
            new bindingMaker(sqlString, seatIDs, this);
        }
    }
    public class Roles : ObservableCollection<string>
    {
        Dictionary<String, String> titleIDs = new Dictionary<String, String>();

        public Roles()
        {
            String titleSQL = "SELECT Title FROM [REVINT].[dbo].[ED_Roles] ORDER BY Id";
            String titleIDSQL = "SELECT Id FROM [REVINT].[dbo].[ED_Roles] ORDER BY Id";

            new comboMaker(titleSQL, titleIDSQL, titleIDs, this);
        }
        public String getID(string strName)
        {
            if (!titleIDs.ContainsKey(strName))
            {
                return "-1";
            }
            else
            {
                return titleIDs[strName];
            }
        }
    }
    public class newcomboMaker
    {
        public newcomboMaker(String sqlCmd, Dictionary<String, String> dictionary, ObservableCollection<string> comboClass)
        {
            object[] obj = new object[10];

            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            using (OdbcConnection connection = new OdbcConnection(cxnString))
            {
                OdbcCommand command = new OdbcCommand(sqlCmd, connection);

                connection.Open();

                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int numCols = reader.GetValues(obj);

                    if (numCols >= 2)
                    {
                        dictionary.Add(obj[1].ToString(), obj[0].ToString());
                        comboClass.Add(obj[1].ToString());
                    }
                }
            }
        }
    }

    public class comboMaker
    {
        public comboMaker(String sqlCmd, String sqlCmdID, Dictionary<String, String> dictionary, ObservableCollection<string> comboClass)
        {
            object[] obj = new object[10];
            object[] objID = new object[10];

            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            OdbcConnection connection = new OdbcConnection(cxnString);

            using (OdbcConnection connectionID = new OdbcConnection(cxnString))
            {
                OdbcCommand command = new OdbcCommand(sqlCmd, connection);
                OdbcCommand commandID = new OdbcCommand(sqlCmdID, connectionID);

                connection.Open();
                connectionID.Open();

                OdbcDataReader reader = command.ExecuteReader();
                OdbcDataReader reader2 = commandID.ExecuteReader();

                while (reader.Read() && reader2.Read())
                {
                    int numCols = reader.GetValues(obj);
                    numCols = reader2.GetValues(objID);

                    for (int i = 0; i < numCols; i++)
                    {
                        dictionary.Add(obj[i].ToString(), objID[i].ToString());
                        comboClass.Add(obj[i].ToString());
                    }
                }
            }
        }
    }
    public class bindingMaker
    {
        public bindingMaker(String sqlCmd, Dictionary<String, String> dictionary, ObservableCollection<string> comboClass)
        {
            object[] obj = new object[10];

            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";


            using (OdbcConnection connection = new OdbcConnection(cxnString))
            {
                OdbcCommand command = new OdbcCommand(sqlCmd, connection);

                connection.Open();

                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int numCols = reader.GetValues(obj);

                    for (int i = 0; i < numCols; i++)
                    {
                        comboClass.Add(obj[i].ToString());
                    }
                }
            }
        }
    }
}
