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
}
