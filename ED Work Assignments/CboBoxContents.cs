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
        
        object[] employeeUserNames = new object[10];
        object[] employeeFirstNames = new object[10];
        object[] employeeLastNames = new object[10];
        object[] employeeRoles = new object[10];
        object[] employeeEmployed = new object[10];
        object[] employeeIDs = new object[10];
        
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
                OdbcCommand commandFirstName = new OdbcCommand("SELECT FirstName FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionFirstName);
                OdbcCommand commandLastName = new OdbcCommand("SELECT LastName FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionLastName);
                OdbcCommand commandUserName = new OdbcCommand("SELECT UserName FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionUserName);
                OdbcCommand commandRole = new OdbcCommand("SELECT Role FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionRole);
                OdbcCommand commandEmployed = new OdbcCommand("SELECT CurrentlyEmployed FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionEmployed);
                OdbcCommand commandID = new OdbcCommand("SELECT Id FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionID);

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

                if(firstNameReader.Read() && lastNameReader.Read() && userNameReader.Read() && iDReader.Read() && roleReader.Read() && employedReader.Read())
                {
                    do {
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
                    } while (firstNameReader.Read() && lastNameReader.Read() && userNameReader.Read() && iDReader.Read() && roleReader.Read() && employedReader.Read());
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
                return employeeID[strName];
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
            String seatSQL = "SELECT Name FROM [REVINT].[dbo].[ED_Seats] ORDER BY Name";
            String seatIDSQL = "SELECT Id FROM [REVINT].[dbo].[ED_Seats] ORDER BY Name";

            new comboMaker(seatSQL, seatIDSQL, seatIDs, this);
        }
        public String getID(string strName)
        {
            if (!seatIDs.ContainsKey(strName))
            {
                return "-1";
            }
            else
                return seatIDs[strName];
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
                return titleIDs[strName];
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

                if (reader.Read() && reader2.Read())
                {
                    do
                    {
                        int numCols = reader.GetValues(obj);
                        numCols = reader2.GetValues(objID);

                        for (int i = 0; i < numCols; i++)
                        {
                            dictionary.Add(obj[i].ToString(), objID[i].ToString());
                            comboClass.Add(obj[i].ToString());
                        }
                    } while (reader.Read() && reader2.Read());
                }
            }

        }
    }
}
