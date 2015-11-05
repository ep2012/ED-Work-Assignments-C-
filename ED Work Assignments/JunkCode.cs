using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    class JunkCode
    {
    }
    /*
           // Configure open file dialog box
           Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

           dlg.DefaultExt = "*.xlsx;*.xlsm:.xls"; // Default file extension
           dlg.Filter = "Excel Worksheets|*.xlsx"; // Filter files by extension 

           // Show open file dialog box
           Nullable<bool> result = dlg.ShowDialog();

           // Process open file dialog box results 
           if (result == true)
           {
               /*
               String fileName = dlg.FileName;

               string[] columns = new String[4];

               //Opens the Worksheet you want to access 
               Microsoft.Office.Interop.Excel.Application xlsApp = new Microsoft.Office.Interop.Excel.Application();
               Microsoft.Office.Interop.Excel.Workbook xlsWkBk = xlsApp.Workbooks.Open(fileName);
               Microsoft.Office.Interop.Excel.Worksheet xlsWkSt = xlsWkBk.ActiveSheet;

               //Name of the Worksheet techniqually needs a $ at the end 
               String Worksheet = xlsWkSt.Name + "$";

               //Access the first row and get names of each column 
               for (int i = 0; i < 4; i++)
               {
                   columns[i] = xlsWkSt.Cells[1, (i + 1)].Value2;
               }
               */
    // Open document 
    /*
        String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

        Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
        Microsoft.Office.Interop.Excel.Workbook wb = excelApp.Workbooks.Open(dlg.FileName.ToString());

        Microsoft.Office.Interop.Excel.Worksheet excelSheet = wb.ActiveSheet;
        Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)excelSheet.Cells;

        Microsoft.Office.Interop.Excel.Range last = excelSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);

        int lastUsedRow = last.Row;

        Mouse.OverrideCursor = Cursors.Wait;

        int notSuccessful = 0;
        List<int> notSuccessfulRows = new List<int>();

        int row;

        Users users = new Users();

        Seats seats = new Seats();

        for (row = 2; row <= lastUsedRow; row++)
        {
            try
            {
                using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                {
                    //open OdbcConnection object
                    dbConnection.Open();

                    OdbcCommand cmd = new OdbcCommand();

                    cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_newWorkAssignment(?, ?, ?, ?)}";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = dbConnection;

                    cmd.Parameters.Add("@employee", OdbcType.Int).Value = users.getID(((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 1]).Value);
                    cmd.Parameters.Add("@seat", OdbcType.Int).Value = seats.getID(((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 2]).Value);
                    cmd.Parameters.Add("@start", OdbcType.DateTime).Value = ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 3]).Value;
                    cmd.Parameters.Add("@end", OdbcType.DateTime).Value = ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, 4]).Value;

                    cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }

            }
            catch (System.Data.Odbc.OdbcException)
            {
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)excelSheet.Rows[1];

                firstRow.EntireRow.Font.Bold = true;
                firstRow.Cells.Interior.ColorIndex = 36;
                notSuccessful++;
                notSuccessfulRows.Add(row);
            }
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                OdbcCommand cmd = new OdbcCommand();

                cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateChangeTracker(?, ?)}";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = dbConnection;

                cmd.Parameters.Add("@username", OdbcType.NVarChar, 100).Value = Environment.UserName;
                cmd.Parameters.Add("@notes", OdbcType.NVarChar, 4000).Value = "Uploaded file: " + dlg.FileName;

                cmd.ExecuteNonQuery();

                dbConnection.Close();
            }
        }

        Mouse.OverrideCursor = null;

        if (notSuccessful > 0)
        {
            var dialogResult = MessageBox.Show("Completed inserting records into database. Could not insert " + notSuccessful.ToString() + " records.\nView the row numbers for these records?", "Finished inserting records", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                var dialogResult2 = MessageBox.Show(ArrayToString(notSuccessfulRows.ToArray()) + "\n\nCopy to Clipboard?", "Unsuccessful row numbers", MessageBoxButton.YesNo);
                if (dialogResult2 == MessageBoxResult.Yes)
                {
                    Clipboard.SetText(ArrayToString(notSuccessfulRows.ToArray()));
                }
            }
        }

        wb.Close();
        setWindows();
    }
    */
    /*
     * private void btnDeleteAssignment_Click(object sender, RoutedEventArgs e)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";
            var dialogResult = MessageBox.Show("Are you sure you would like to delete this work assignment?", "Deleting Assignment", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
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

                using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
                {
                    //open OdbcConnection object
                    dbConnection.Open();

                    OdbcCommand cmd = new OdbcCommand();

                    cmd.CommandText = "{CALL [REVINT].[HEALTHCARE\\eliprice].ed_updateChangeTracker(?, ?)}";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = dbConnection;

                    cmd.Parameters.Add("@username", OdbcType.NVarChar, 100).Value = Environment.UserName;
                    cmd.Parameters.Add("@notes", OdbcType.NVarChar, 4000).Value = "Deleted record:\nEmployee: " + previousRecordDetail;

                    cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }

                if (mainWindow.ShowActivated)
                {
                    mainWindow.update();
                }
                this.Close();
            }

        }
     */
    /*
        public bool isEmployeeFreeAndWorking(int employee, DateTime start, DateTime end)
        {
            //adjust to make free and working
            List<object> checkerList = new List<object>();
            String sqlString = "SELECT [REVINT].[dbo].[ED_Shifts].[Id] FROM [REVINT].[dbo].[ED_Shifts] JOIN [REVINT].[dbo].[ED_Employees] ON [REVINT].[dbo].[ED_Shifts].[Employee] = [REVINT].[dbo].[ED_Employees].[Id] WHERE [REVINT].[dbo].[ED_Employees].[Id] = " + employee + " AND [REVINT].[dbo].[ED_Shifts].[StartShift] < '" + start + "' AND [REVINT].[dbo].[ED_Shifts].[EndShift] > '" + end + "';";
            new idMaker(sqlString, checkerList);
            if (checkerList.Count == 0)
                return false;
            else
                return true;
        }
         */
    /*
                if (start == end)
                {
                    return;
                }
                //First add all hours to supervisor schedule before scheduling
                schedule.scheduleSupervisor(employee, start, end);
                //break up and schedule
                DateTime tempStart = start;
                if (tempStart.Subtract(end).TotalHours > 3)
                {
                    while (tempStart.Subtract(end).TotalHours > 3)
                    {
                        DateTime tempEnd = tempStart.AddHours(3);

                        schedule.scheduleClocking(employee, tempStart, tempEnd);
                        tempStart = tempEnd;
                    }
                    schedule.scheduleClocking(employee, tempStart, end);
                }
                else
                {
                    schedule.scheduleClocking(employee, start, end);
                }
                 */
    /*
     * 
        public List<OffClocking> whoCanWorkOnlyEnd(DateTime start, DateTime end, String day)
        {
            return whoWorksOnlyEnd(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }
        public List<OffClocking> whoCanWorkSupervisorOnlyEnd(DateTime start, DateTime end, String day)
        {
            return whoWorksSupervisorOnlyEnd(start, end, day).Except(whoHasVacation(start, end, 0)).ToList();
        }
     * 
     * 
        public List<OffClocking> whoWorksOnlyEnd(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"time as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "time as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 2 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
        public List<OffClocking> whoWorksSupervisorOnlyEnd(DateTime start, DateTime end, String day)
        {
            List<OffClocking> checkerList = new List<OffClocking>();
            String sqlString = @"SELECT A.[Id], cast(A." + day + @"time as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE cast(A." + day + "time as time) BETWEEN  cast('" + start + "' as time) AND cast('" + end + "' as time) AND A.Role = 3 ORDER BY NEWID();";
            new idMaker(sqlString, checkerList);
            return checkerList;
        }
     */
    /*
                foreach (OffClocking clockings in employeesThatCanWorkLastPart)
                {
                    DateTime start = DateTime.Parse(startTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(start, endTime, Convert.ToInt32(clockings.id));
                    schedule.scheduleSupervisor(start, endTime, Convert.ToInt32(clockings.id));
                }
                */
    /*
                foreach (OffClocking clockings in employeesThatCanWorkLastPart)
                {
                    DateTime start = DateTime.Parse(startTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(start, endTime, Convert.ToInt32(clockings.id));
                }
                */
    /*public class SupervisorScheduleCreator
    {
        Schedule schedule = new Schedule();
        Random random = new Random();

        public SupervisorScheduleCreator(DateTime day, String dayStr)
        {
            DateTime goaldate = day.AddDays(1);
            DateTime startTime = day;
            DateTime endTime = startTime.AddMinutes(30 * random.Next(4, 9));

            while (endTime < goaldate)
            {
                List<object> employeesThatCanWorkSupervisor = new List<object>();
                //List<OffClocking> employeesThatCanWorkFirstPart = new List<OffClocking>();

                //get employees that can work
                if ((startTime - day).TotalHours < 24)
                {
                    employeesThatCanWorkSupervisor = schedule.whoCanWorkSupervisor(startTime, endTime, dayStr);
                    //employeesThatCanWorkFirstPart = schedule.whoCanWorkSupervisorOnlyStart(startTime, endTime, dayStr);
                }
                else
                {
                    employeesThatCanWorkSupervisor = schedule.whoCanWorkSupervisorDay2(startTime, endTime, dayStr);
                    //employeesThatCanWorkFirstPart = schedule.whoCanWorkSupervisorDay2OnlyStart(startTime, endTime, dayStr);
                }

                //schedule employees
                foreach (object employee in employeesThatCanWorkSupervisor)
                {
                    schedule.scheduleInBestWorkstation(startTime, endTime, Convert.ToInt32(employee));
                    schedule.scheduleSupervisor(startTime, endTime, Convert.ToInt32(employee));
                }

                /*
                foreach (OffClocking clockings in employeesThatCanWorkFirstPart)
                {
                    DateTime end = DateTime.Parse(endTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(startTime, end, Convert.ToInt32(clockings.id));
                    schedule.scheduleSupervisor(startTime, end, Convert.ToInt32(clockings.id));
                }
                */

                //increment times
                /*startTime = startTime.AddMinutes(30);
                endTime = startTime.AddMinutes(30 * random.Next(4, 8));
            }
        }
    }
    */
    /*
public class Schedule
{
    public TempScheduler tempScheduler = new TempScheduler();
    Users users = new Users();
    Random random = new Random();
    public Schedule()
    {

    }

    public void scheduleInBestWorkstation(DateTime start, DateTime end, int employee)
    {
        if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
        {
            scheduleEmployee(employee, start, end, 1);
        }
        else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
        {
            scheduleEmployee(employee, start, end, 5);
        }
        else if (isStationOpen(start, end, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
        {
            scheduleEmployee(employee, start, end, 6);
        }

        //else if !POD 3/4 filled, fill it
        else if (isStationOpen(start, end, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
        {
            scheduleEmployee(employee, start, end, 7);
        }
        else if (isStationOpen(start, end, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
        {
            scheduleEmployee(employee, start, end, 3);
        }
        else if (isStationOpen(start, end, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
        {
            scheduleEmployee(employee, start, end, 6);
        }
        else if (isStationOpen(start, end, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
        {
            scheduleEmployee(employee, start, end, 7);
        }
        else if (isStationOpen(start, end, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
        {
            scheduleEmployee(employee, start, end, 3);
        }
        else if (isStationOpen(start, end, 4, 2) && !didEmployeeWorkAtStationLast(start, 4, employee))
        {
            scheduleEmployee(employee, start, end, 4);
        }
        //else if !iPad filled, fill it
        else if (isStationOpen(start, end, 2, 1))
        {
            scheduleEmployee(employee, start, end, 2);
        }
        //else if (isStationOpenAtBeginning(start, end, out endTime, 2, 1))
        //{
        //    scheduleEmployee(employee, start, endTime, 2);
        //}
        else
        {
            //System.Windows.MessageBox.Show("Overstaffed " + start);
        }
    }
    public void putHoleInHighestPriorityStation(DateTime time)
    {
        int station = getHighestOpenStation(time);
        if (station != -1)
        {
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            getOpenShiftRange(time, station, out start, out end);
            tempScheduler.insertBlank(start, end, station);
        }
    }
    private void getOpenShiftRange(DateTime time, int station, out DateTime start, out DateTime end)
    {
        List<object> checkerList = new List<object>();
        List<object> checkerList2 = new List<object>();
        String sqlString = @"SELECT A.[StartShift] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A WHERE A.[StartShift] > '" + time + "' AND A.Seat = " + station + " ORDER BY A.[StartShift];";
        String sqlString2 = @"SELECT A.[EndShift] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A WHERE A.[EndShift] < '" + time + "' AND A.Seat = " + station + " ORDER BY A.[EndShift] DESC;";

        new idMaker(sqlString, checkerList);
        new idMaker(sqlString2, checkerList2);

        if (checkerList2.Count > 0)
            start = DateTime.Parse(checkerList2[0].ToString());
        else
            start = time.AddMinutes(-121);

        if (checkerList.Count > 0)
            end = DateTime.Parse(checkerList[0].ToString());
        else
            end = time.AddMinutes(119);
    }
    private int getHighestOpenStation(DateTime time)
    {
        if (isStationOpen(time, time, 1, 1))
        {
            return 1;
        }
        else if (isStationOpen(time, time, 5, 1))
        {
            return 5;
        }
        else if (isStationOpen(time, time, 6, 2))
        {
            return 6;
        }
        else if (isStationOpen(time, time, 7, 2))
        {
            return 7;
        }
        else if (isStationOpen(time, time, 3, 2))
        {
            return 3;
        }
        else if (isStationOpen(time, time, 4, 2))
        {
            return 4;
        }
        else if (isStationOpen(time, time, 2, 1))
        {
            return 2;
        }
        return -1;
    }
    public void scheduleInBestWorkstation2(DateTime start, DateTime end, int employee)
    {

        if (isStationOpen(start, end, 6, 1) && !didEmployeeWorkAtStationLast(start, 6, employee))
        {
            scheduleEmployee(employee, start, end, 6);
        }
        else if (isStationOpen(start, end, 7, 1) && !didEmployeeWorkAtStationLast(start, 7, employee))
        {
            scheduleEmployee(employee, start, end, 7);
        }
        else if (isStationOpen(start, end, 3, 1) && !didEmployeeWorkAtStationLast(start, 3, employee))
        {
            scheduleEmployee(employee, start, end, 3);
        }
        else if (isStationOpen(start, end, 6, 2) && !didEmployeeWorkAtStationLast(start, 6, employee))
        {
            scheduleEmployee(employee, start, end, 6);
        }
        else if (isStationOpen(start, end, 7, 2) && !didEmployeeWorkAtStationLast(start, 7, employee))
        {
            scheduleEmployee(employee, start, end, 7);
        }
        else if (isStationOpen(start, end, 3, 2) && !didEmployeeWorkAtStationLast(start, 3, employee))
        {
            scheduleEmployee(employee, start, end, 3);
        }
        else if (isStationOpen(start, end, 4, 2))
        {
            scheduleEmployee(employee, start, end, 4);
        }
        //else if !iPad filled, fill it
        else if (isStationOpen(start, end, 2, 1))
        {
            scheduleEmployee(employee, start, end, 2);
        }
        //else if (isStationOpenAtBeginning(start, end, out endTime, 2, 1))
        //{
        //    scheduleEmployee(employee, start, endTime, 2);
        //}
        else
        {
            scheduleEmployee(employee, start, start.AddMinutes(30), 10);
        }
    }
    public void scheduleCheckInCheckOut(DateTime start, DateTime end, int employee)
    {
        if (isStationOpen(start, end, 1, 1) && !didEmployeeWorkAtStationLast(start, 1, employee))
        {
            scheduleEmployee(employee, start, end, 1);
        }
        else if (isStationOpen(start, end, 5, 1) && !didEmployeeWorkAtStationLast(start, 5, employee))
        {
            scheduleEmployee(employee, start, end, 5);
        }
            
    }
    private int GenerateDigit(int num)
    {
        return random.Next(num);
    }

    public bool isStationOpen(DateTime start, DateTime end, int station, int num)
    {
        List<object> checkerList = new List<object>();
        //String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";
        String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + ";";

        new idMaker(sqlString, checkerList);
        return checkerList.Count < num;
    }
    private bool didEmployeeWorkAtStationLast(DateTime start, int station, int employee)
    {
        List<object> checkerList = new List<object>();
        String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE A.[EndShift] = '" + start + "' AND A.Seat = " + station + " AND A.[Employee] = " + employee + ";";

        new idMaker(sqlString, checkerList);
        return checkerList.Count > 0;
    }

    private bool isStationOpenAtBeginning(DateTime start, DateTime end, out DateTime endTime, int station, int num)
    {
        List<object> checkerList = new List<object>();
        List<object> returnList = new List<object>();
        endTime = new DateTime();

        String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE (((A.[StartShift] <= '" + start + "') AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + " GROUP BY B.[Id];";
        String returnString = @"SELECT A.[StartShift] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) AND A.Seat = " + station + " ORDER BY A.[EndShift];";

        new idMaker(sqlString, checkerList);
        new idMaker(returnString, returnList);
        if (returnList.Count > 0)
            endTime = DateTime.Parse(returnList[0].ToString());
        return checkerList.Count < num;
    }

    public void scheduleEmployee(int employee, DateTime start, DateTime end, int station)
    {
        tempScheduler.insert(employee, start, end, station);
    }

    private List<object> whoHasVacation(DateTime start, DateTime end)
    {
        List<object> checkerList = new List<object>();
        String sqlString = @"SELECT A.[EmployeeId] FROM [REVINT].[HEALTHCARE\eliprice].[ED_TimeOff] A WHERE A.[StartTime] <= '" + start + "' AND A.[EndTime] >= '" + end + "';";
        new idMaker(sqlString, checkerList);
        return checkerList;
    }

    public List<object> whoCanWork(DateTime start, DateTime end, String day)
    {
        return whoWorks(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start, end)).ToList();
    }
    public List<object> whoCanWorkDay2(DateTime start, DateTime end, String day)
    {
        return whoWorksDay2(start, end, day).Except(whoAlreadyWorks(start, end)).Except(whoHasVacation(start, end)).ToList();
    }


    public List<OffClocking> whoCanWorkOnlyStart(DateTime start, DateTime end, String day)
    {
        List<OffClocking> returnList = whoWorksOnlyStart(start, end, day);

        foreach (object employee in (whoAlreadyWorks(start, end).Union(whoHasVacation(start, end))).ToList())
        {
            foreach (OffClocking clocking in returnList.ToList())
            {
                if (clocking.id.ToString() == employee.ToString())
                {
                    returnList.Remove(clocking);
                }
            }
        }
        unionWithPartShifts(start, end, day, returnList);
        return returnList;
    }


    public List<OffClocking> whoCanWorkDay2OnlyStart(DateTime start, DateTime end, String day)
    {
        List<OffClocking> returnList = whoWorksDay2OnlyStart(start, end, day);

        foreach (object employee in (whoAlreadyWorks(start, end).Union(whoHasVacation(start, end))).ToList())
        {
            foreach (OffClocking clocking in returnList.ToList())
            {
                if (clocking.id.ToString() == employee.ToString())
                {
                    returnList.Remove(clocking);
                }
            }
        }
        unionWithPartShiftsDay2(start, end, day, returnList);
        return returnList;
    }
    public void unionWithPartShifts(DateTime start, DateTime end, String day, List <OffClocking> returnList)
    {
        List<OffClocking> checkerList2 = new List<OffClocking>();
        List<OffClocking> checkerList3 = new List<OffClocking>();

        List<object> peopleThatWork = whoWorks(start, end, day);
            
        String sqlString2 = @"SELECT B.[Id], cast(A.[StartShift] as time) FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND (A.[StartShift] > '" + start + "' AND A.[StartShift] < '" + end + "');";
        String sqlString3 = @"SELECT B.[Id], cast(A.[StartShift] as time) FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND (A.[EndShift] > '" + start + "' AND A.[EndShift] < '" + end + "');";

        new idMaker(sqlString2, checkerList2);
        new idMaker(sqlString3, checkerList3);
        foreach (object employee in peopleThatWork.ToList())
        {
            foreach (OffClocking clocking in checkerList2.ToList())
            {
                if (clocking.id.ToString() == employee.ToString())
                {
                    returnList.Add(clocking);
                }
            }
        }
        foreach (OffClocking clocking in checkerList2.ToList())
        {
            foreach (OffClocking otherclocking in checkerList3.ToList())
            {
                if(clocking.id.ToString() == otherclocking.id.ToString() && clocking.date.ToString() == otherclocking.date.ToString())
                {
                    checkerList3.Remove(otherclocking);
                }
            }
        }

        foreach (OffClocking returnClocking in returnList.ToList())
        {
            foreach (OffClocking clocking in checkerList3.ToList())
            {
                if (returnClocking.id.ToString() == clocking.id.ToString())
                {
                    returnList.Remove(returnClocking);
                }
            }
        }
    }
    public void unionWithPartShiftsDay2(DateTime start, DateTime end, String day, List<OffClocking> returnList)
    {
        List<OffClocking> checkerList2 = new List<OffClocking>();
        List<OffClocking> checkerList3 = new List<OffClocking>();


        List<object> peopleThatWork = whoWorksDay2(start, end, day);

        String sqlString2 = @"SELECT B.[Id], cast(A.[StartShift] as time) FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND (A.[StartShift] > '" + start + "' AND A.[StartShift] < '" + end + "');";
        String sqlString3 = @"SELECT B.[Id], cast(A.[StartShift] as time) FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND (A.[EndShift] > '" + start + "' AND A.[EndShift] < '" + end + "');";

        new idMaker(sqlString2, checkerList2);
        new idMaker(sqlString3, checkerList3);
        foreach (object employee in peopleThatWork.ToList())
        {
            foreach (OffClocking clocking in checkerList2.ToList())
            {
                if (clocking.id.ToString() == employee.ToString())
                {
                    returnList.Add(clocking);
                }
            }
        }
        foreach (OffClocking clocking in checkerList2.ToList())
        {
            foreach (OffClocking otherclocking in checkerList3.ToList())
            {
                if (clocking.id.ToString() == otherclocking.id.ToString() && clocking.date.ToString() == otherclocking.date.ToString())
                {
                    checkerList3.Remove(otherclocking);
                }
            }
        }

        foreach (OffClocking returnClocking in returnList.ToList())
        {
            foreach (OffClocking clocking in checkerList3.ToList())
            {
                if (returnClocking.id.ToString() == clocking.id.ToString())
                {
                    returnList.Remove(returnClocking);
                }
            }
        }
    }
    public List<object> whoWorks(DateTime start, DateTime end, String day)
    {
        List<object> checkerList = new List<object>();
        String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (cast(A." + day + "time as time) <= '" + start.ToString("HH:mm") + "') AND (cast(A." + day + "timeend as time) >= '" + end.ToString("HH:mm") + "' OR (A." + day + "day = 'True')) AND (A.Role = 2 OR A.Role = 3) ORDER BY NEWID();";
        //String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time)) AND ((cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time)) OR (cast(A." + day + "timeend as time) BETWEEN cast('" + end.ToString("HH:mm") + "' as time) AND cast('" + start.ToString("HH:mm") + "' as time)) OR (A." + day + "day = 'True'))) AND (A.Role = 2 OR A.Role = 3) ORDER BY NEWID();";
        String sqlString2 = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time) AND cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time)) OR (cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 2 ORDER BY NEWID();";
        new idMaker(sqlString, checkerList);
        return checkerList;
    }

    public List<object> whoWorksDay2(DateTime start, DateTime end, String day)
    {
        List<object> checkerList = new List<object>();
        String sqlString;
        //String sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (cast(A." + day + "time as time) <= cast('" + start.ToString("HH:mm") + "' as time)) AND (cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time)) AND ((A.Role = 2 OR A.Role = 3)) ORDER BY NEWID();";
        if (DateTime.Parse(end.ToShortDateString())>DateTime.Parse(start.ToShortDateString()))
        {
            sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (cast(A." + day + "time as time) <= '" + start.ToString("HH:mm") + "') AND (cast(A." + day + "timeend as time) >= '" + end.ToString("HH:mm") + "') AND (A.Role = 2 OR A.Role = 3) AND (A." + day + "day = 'True') ORDER BY NEWID();";
        }
        else
        {
            sqlString = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (cast(A." + day + "timeend as time) >= '" + end.ToString("HH:mm") + "') AND (A.Role = 2 OR A.Role = 3) AND (A." + day + "day = 'True') ORDER BY NEWID();";
        }

        String sqlString3 = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time) AND (A." + day + "day = 'True'))) AND (A.Role = 2 OR A.Role = 3) ORDER BY NEWID();";
        String sqlString2 = @"SELECT A.[Id] FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) >= cast('" + end.ToString("HH:mm") + "' as time) AND (A." + day + "day = 'True'))) AND A.Role = 2 ORDER BY NEWID();";
        new idMaker(sqlString, checkerList);
        return checkerList;
    }

    public List<OffClocking> whoWorksOnlyStart(DateTime start, DateTime end, String day)
    {
        List<OffClocking> checkerList = new List<OffClocking>();

        String sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) > '" + start.ToString("HH:mm") + "') AND (cast(A." + day + "timeend as time) < '" + end.ToString("HH:mm") + "')) AND ((A.Role = 2 OR A.Role = 3)) AND NOT (A." + day + "day = 'True') ORDER BY NEWID();";
        new idMaker(sqlString, checkerList);


        return checkerList;
    }

    public void scheduleSupervisors(DateTime date, String day)
    {
        List<SupervisorClocking> dayList = new List<SupervisorClocking>();
        List<SupervisorClocking> nightList = new List<SupervisorClocking>();
        String sqlStringNight = @"SELECT A.[Id], cast(A." + day + @"time as time), cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (A." + day + "day = 'True') AND (A.Role = 3 OR A.Role = 3) AND (A." + day + @"timeend IS NOT NULL) ORDER BY NEWID();";
        String sqlStringDay = @"SELECT A.[Id], cast(A." + day + @"time as time), cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (A.Role = 3) AND (A." + day + @"timeend IS NOT NULL) ORDER BY NEWID();";

        new idMaker(sqlStringDay, dayList);
        new idMaker(sqlStringNight, nightList);

        dayList = dayList.Except(nightList).ToList();

        foreach (SupervisorClocking clocking in dayList)
        {
            scheduleEmployee(Convert.ToInt32(clocking.id), DateTime.Parse(date.ToShortDateString() + " " + clocking.start.ToString()), DateTime.Parse(date.ToShortDateString() + " " + clocking.end.ToString()), 9);
        }
        foreach (SupervisorClocking clocking in nightList)
        {
            scheduleEmployee(Convert.ToInt32(clocking.id), DateTime.Parse(date.ToShortDateString() + " " + clocking.start.ToString()), DateTime.Parse(date.AddDays(1).ToShortDateString() + " " + clocking.end.ToString()), 9);
        }
    }

    public List<OffClocking> whoWorksDay2OnlyStart(DateTime start, DateTime end, String day)
    {
        List<OffClocking> checkerList = new List<OffClocking>();
        String sqlString;
        if (DateTime.Parse(end.ToShortDateString()) > DateTime.Parse(start.ToShortDateString()))
        {
            sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE (((cast(A." + day + "time as time) <= '" + start.ToString("HH:mm") + "') AND (cast(A." + day + "timeend as time) < '" + end.ToString("HH:mm") + "') AND (A." + day + "day = 'True')) OR ((cast(A." + day + "timeend as time) < '" + start.ToString("HH:mm") + "' AND NOT (A." + day + "day = 'True')))) AND ((A.Role = 2 OR A.Role = 3)) ORDER BY NEWID();";
        }
        else
        {
            sqlString = @"SELECT A.[Id], cast(A." + day + @"timeend as time) FROM [REVINT].[HEALTHCARE\eliprice].ED_Employees A WHERE ((cast(A." + day + "timeend as time) > '" + start.ToString("HH:mm") + "') AND (cast(A." + day + "timeend as time) < '" + end.ToString("HH:mm") + "')) AND ((A.Role = 2 OR A.Role = 3)) AND (A." + day + "day = 'True') ORDER BY NEWID();";
        }

        new idMaker(sqlString, checkerList);

        return checkerList;
    }

    public List<object> whoAlreadyWorks(DateTime start, DateTime end)
    {
        List<object> checkerList = new List<object>();
        List<object> checkerList2 = new List<object>();
        String sqlString = @"SELECT B.[Id] FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) GROUP BY B.[Id];";
        String sqlString2 = "SELECT B.[Id] FROM [REVINT].[dbo].[ED_Shifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND ((A.[StartShift] BETWEEN '" + start + "' AND '" + end + "') OR (A.[StartShift] <= '" + start + "' AND (A.[EndShift] > '" + start + "'))) GROUP BY B.[Id];";
        new idMaker(sqlString, checkerList);
        new idMaker(sqlString, checkerList2);

        return (checkerList.Union(checkerList2)).ToList();
    }
    private void clearDay(DateTime day)
    {

    }
}
 */
}
/*
    public class EmployeeScheduleCreator
    {
        Schedule schedule = new Schedule();
        Random random = new Random();

        public EmployeeScheduleCreator(DateTime day, String dayStr)
        {
            DateTime goaldate = day.AddHours(36);
            DateTime startTime = day;
            DateTime endTime;

            schedule.scheduleSupervisors(day, dayStr);

            List<object> employeesThatCanWork = new List<object>();

            List<OffClocking> employeesThatCanWorkFirstPart = new List<OffClocking>();

            while (startTime < goaldate)
            {

                endTime = startTime.AddMinutes(30 * random.Next(10, 12));

                //get employees that can work
                if ((endTime - day).TotalHours < 24)
                {
                    employeesThatCanWork = schedule.whoCanWork(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkOnlyStart(startTime, endTime, dayStr);
                }

                else
                {
                    employeesThatCanWork = schedule.whoCanWorkDay2(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkDay2OnlyStart(startTime, endTime, dayStr);
                }
                
                foreach (object employee in employeesThatCanWork)
                {
                    schedule.scheduleCheckInCheckOut(startTime, endTime, Convert.ToInt32(employee));
                }
                

                foreach (OffClocking clockings in employeesThatCanWorkFirstPart)
                {
                    DateTime end = DateTime.Parse(endTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleCheckInCheckOut(startTime, end, Convert.ToInt32(clockings.id));
                }

                //increment time
                startTime = startTime.AddMinutes(30);
            }

            startTime = day;
            while(startTime < goaldate)
            {
                endTime = startTime.AddMinutes(30 * random.Next(10, 12));

                //get employees that can work
                if ((endTime - day).TotalHours < 24)
                {
                    employeesThatCanWork = schedule.whoCanWork(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkOnlyStart(startTime, endTime, dayStr);

                }
                else
                {
                    employeesThatCanWork = schedule.whoCanWorkDay2(startTime, endTime, dayStr);
                    employeesThatCanWorkFirstPart = schedule.whoCanWorkDay2OnlyStart(startTime, endTime, dayStr);
                }
                
                //schedule employees
                foreach (OffClocking clockings in employeesThatCanWorkFirstPart)
                {
                    DateTime end = DateTime.Parse(endTime.ToShortDateString() + " " + clockings.date.ToString());
                    schedule.scheduleInBestWorkstation(startTime, end, Convert.ToInt32(clockings.id));
                }

                
                foreach (object employee in employeesThatCanWork)
                {
                    schedule.scheduleInBestWorkstation2(startTime,endTime, Convert.ToInt32(employee));
                }
                 
                //increment time
                startTime = startTime.AddMinutes(30);
            }

            String sqlStringFullStaffing = "WITH StaffingReport AS (SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            " LEFT JOIN [REVINT].[dbo].[ED_Shifts] Shifts" +
                            " ON NOT Shifts.Seat = 9 AND CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing" +
                            " UNION ALL" +
                            " SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                            ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                            " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                            @" LEFT JOIN [REVINT].[healthcare\eliprice].[ED_ScheduleMakerShifts] Shifts" +
                            " ON NOT Shifts.Seat = 9 AND CONVERT(datetime, CONCAT('" + day.ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift" +
                            " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing)"+ 
                            " SELECT [Time Slot], [Minimum Staffing Requirement], SUM ([Amount Staffed]) AS [Amount Staffed]" +
                            "  FROM StaffingReport WHERE [Amount Staffed] >= [Minimum Staffing Requirement] GROUP BY [Time Slot], [Minimum Staffing Requirement] ORDER BY [Time Slot]";

            List<object> fullStaffing = new List<object>();
            new idMaker(sqlStringFullStaffing, fullStaffing, "");

            List<object> minStaffingSlots = new List<object>();
            String sqlString = "SELECT TimeSlot FROM [REVINT].[dbo].[ED_Staffing]";

            new idMaker(sqlString, minStaffingSlots, "");

            List<object> minStaffingList = minStaffingSlots.Except(fullStaffing).ToList();

            while (minStaffingList.Count > 0)
            {
                foreach (object date in minStaffingList)
                {
                    schedule.putHoleInHighestPriorityStation(DateTime.Parse(day.ToShortDateString() + " " + date.ToString()));
                }

                minStaffingSlots = new List<object>();
                new idMaker(sqlString, minStaffingSlots, "");

                fullStaffing = new List<object>();
                new idMaker(sqlStringFullStaffing, fullStaffing, "");

                minStaffingList = minStaffingSlots.Except(fullStaffing).ToList();
            }

        }

    }
     * */
/*
    public void unionWithPartShiftsDay2(DateTime start, DateTime end, String day, List<OffClocking> returnList)
    {
        List<OffClocking> checkerList2 = new List<OffClocking>();
        List<OffClocking> checkerList3 = new List<OffClocking>();


        List<object> peopleThatWork = whoWorksDay2(start, end, day);

        String sqlString2 = @"SELECT B.[Id], cast(A.[StartShift] as time) FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND (A.[StartShift] > '" + start + "' AND A.[StartShift] < '" + end + "');";
        String sqlString3 = @"SELECT B.[Id], cast(A.[StartShift] as time) FROM [REVINT].[HEALTHCARE\eliprice].[ED_ScheduleMakerShifts] A JOIN [REVINT].[dbo].[ED_Employees] B ON A.[Employee] = B.[Id] WHERE NOT(A.Seat = 9) AND (A.[EndShift] > '" + start + "' AND A.[EndShift] < '" + end + "');";

        new idMaker(sqlString2, checkerList2);
        new idMaker(sqlString3, checkerList3);
        foreach (object employee in peopleThatWork.ToList())
        {
            foreach (OffClocking clocking in checkerList2.ToList())
            {
                if (clocking.id.ToString() == employee.ToString())
                {
                    returnList.Add(clocking);
                }
            }
        }
        foreach (OffClocking clocking in checkerList2.ToList())
        {
            foreach (OffClocking otherclocking in checkerList3.ToList())
            {
                if (clocking.id.ToString() == otherclocking.id.ToString() && clocking.date.ToString() == otherclocking.date.ToString())
                {
                    checkerList3.Remove(otherclocking);
                }
            }
        }

        foreach (OffClocking returnClocking in returnList.ToList())
        {
            foreach (OffClocking clocking in checkerList3.ToList())
            {
                if (returnClocking.id.ToString() == clocking.id.ToString())
                {
                    returnList.Remove(returnClocking);
                }
            }
        }
    }
         */