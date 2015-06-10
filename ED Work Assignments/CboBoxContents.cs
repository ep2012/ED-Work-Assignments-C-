﻿using System;
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

        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
        
        object[] employeeUserNames = new object[10];
        object[] employeeFirstNames = new object[10];
        object[] employeeLastNames = new object[10];
        object[] employeeRoles = new object[10];
        object[] employeeIDs = new object[10];
        
        OdbcConnection connectionFirstName;
        OdbcConnection connectionLastName;
        OdbcConnection connectionUserName;
        OdbcConnection connectionRole;
        OdbcConnection connectionID;

        public Users()
        {
            connectionLastName = new OdbcConnection(cxnString);
            connectionUserName = new OdbcConnection(cxnString);
            connectionRole = new OdbcConnection(cxnString);
            connectionID = new OdbcConnection(cxnString);

            using (connectionFirstName = new OdbcConnection(cxnString))
            {
                OdbcCommand commandFirstName = new OdbcCommand("SELECT FirstName FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionFirstName);
                OdbcCommand commandLastName = new OdbcCommand("SELECT LastName FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionLastName);
                OdbcCommand commandUserName = new OdbcCommand("SELECT UserName FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionUserName);
                OdbcCommand commandRole = new OdbcCommand("SELECT Role FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionRole);
                OdbcCommand commandID = new OdbcCommand("SELECT Id FROM [REVINT].[dbo].[ED_Employees] ORDER BY FirstName, LastName", connectionID);

                connectionFirstName.Open();
                connectionLastName.Open();
                connectionUserName.Open();
                connectionRole.Open();
                connectionID.Open();

                OdbcDataReader firstNameReader = commandFirstName.ExecuteReader();
                OdbcDataReader lastNameReader = commandLastName.ExecuteReader();
                OdbcDataReader userNameReader = commandUserName.ExecuteReader();
                OdbcDataReader roleReader = commandRole.ExecuteReader();
                OdbcDataReader iDReader = commandID.ExecuteReader();

                if(firstNameReader.Read() == true && lastNameReader.Read() == true && userNameReader.Read() == true && iDReader.Read() == true && roleReader.Read() == true)
                {
                    do {
                        int numCols = firstNameReader.GetValues(employeeFirstNames);
                        numCols = lastNameReader.GetValues(employeeLastNames);
                        numCols = userNameReader.GetValues(employeeUserNames);
                        numCols = roleReader.GetValues(employeeRoles);
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
                            Add(employeeFirstNames[i].ToString() + " " + employeeLastNames[i].ToString());
                        }
                    } while (firstNameReader.Read() == true && lastNameReader.Read() == true && userNameReader.Read() == true && iDReader.Read() == true && roleReader.Read() == true);
                }

                connectionFirstName.Close();
                connectionLastName.Close();
                connectionUserName.Close();
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
    }
    public class Seats : ObservableCollection<string>
    {
        Dictionary<String, String> seatIDs = new Dictionary<String, String>();

        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
        
        object[] seats = new object[10];
        object[] iDs = new object[10];
        
        OdbcConnection connectionSeat;
        OdbcConnection connectionID;

        public Seats()
        {
            connectionID = new OdbcConnection(cxnString);

            using (connectionSeat = new OdbcConnection(cxnString))
            {
                OdbcCommand commandSeat = new OdbcCommand("SELECT Name FROM [REVINT].[dbo].[ED_Seats] ORDER BY Id", connectionSeat);
                OdbcCommand commandID = new OdbcCommand("SELECT Id FROM [REVINT].[dbo].[ED_Seats] ORDER BY Id", connectionID);
                
                connectionSeat.Open();
                connectionID.Open();

                OdbcDataReader seatReader = commandSeat.ExecuteReader();
                OdbcDataReader iDReader = commandID.ExecuteReader();

                if(seatReader.Read() == true && iDReader.Read() == true)
                {
                    do {
                        int numCols = seatReader.GetValues(seats);
                        numCols = iDReader.GetValues(iDs);

                        for (int i = 0; i < numCols; i++)
                        {
                            seatIDs.Add(seats[i].ToString(), iDs[i].ToString());
                            Add(seats[i].ToString());
                        }
                    } while (seatReader.Read() == true && iDReader.Read() == true);
                }

                connectionSeat.Close();
                connectionID.Close();
            }
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

        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        object[] titles = new object[10];
        object[] iDs = new object[10];

        OdbcConnection connectionTitle;
        OdbcConnection connectionID;

        public Roles()
        {
            connectionID = new OdbcConnection(cxnString);

            using (connectionTitle = new OdbcConnection(cxnString))
            {
                OdbcCommand commandTitle = new OdbcCommand("SELECT Title FROM [REVINT].[dbo].[ED_Roles] ORDER BY Id", connectionTitle);
                OdbcCommand commandID = new OdbcCommand("SELECT Id FROM [REVINT].[dbo].[ED_Roles] ORDER BY Id", connectionID);

                connectionTitle.Open();
                connectionID.Open();

                OdbcDataReader titleReader = commandTitle.ExecuteReader();
                OdbcDataReader iDReader = commandID.ExecuteReader();

                if (titleReader.Read() == true && iDReader.Read() == true)
                {
                    do
                    {
                        int numCols = titleReader.GetValues(titles);
                        numCols = iDReader.GetValues(iDs);

                        for (int i = 0; i < numCols; i++)
                        {
                            titleIDs.Add(titles[i].ToString(), iDs[i].ToString());
                            Add(titles[i].ToString());
                        }
                    } while (titleReader.Read() == true && iDReader.Read() == true);
                }

                connectionTitle.Close();
                connectionID.Close();
            }
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
}
