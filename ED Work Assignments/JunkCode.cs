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
}
