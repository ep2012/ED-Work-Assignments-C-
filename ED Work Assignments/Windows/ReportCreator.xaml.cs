using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ED_Work_Assignments
{
    /// <summary>
    /// Interaction logic for ReportCreator.xaml
    /// </summary>
    public partial class ReportCreator : Window
    {
        bool totalHours;
        bool minStaffing;
        public ReportCreator()
        {
            InitializeComponent();

            totalHours = false;
            minStaffing = false;

            Users listData1 = new Users();
            Binding binding1 = new Binding();

            binding1.Source = listData1;
            cboEmployee.SetBinding(ListBox.ItemsSourceProperty, binding1);

            Seats listData2 = new Seats();
            Binding binding2 = new Binding();

            binding2.Source = listData2;
            cboSeat.SetBinding(ListBox.ItemsSourceProperty, binding2);

            Roles listData3 = new Roles();
            Binding binding3 = new Binding();

            binding3.Source = listData3;
            cboRole.SetBinding(ListBox.ItemsSourceProperty, binding3);

            btnExportToExcel.Visibility = Visibility.Hidden;
        }

        private void rdoTotalHours_Click(object sender, RoutedEventArgs e)
        {
            rdoMinStaffing.IsChecked = false;
            minStaffing = false;

            if (!totalHours)
            {
                rdoTotalHours.IsChecked = true;
                totalHours = true;
            }
            else
            {
                rdoTotalHours.IsChecked = false;
                totalHours = false;
            }
        }

        private void rdoMinStaffing_Click(object sender, RoutedEventArgs e)
        {
            rdoTotalHours.IsChecked = false;
            totalHours = false;

            if (!minStaffing)
            {
                rdoMinStaffing.IsChecked = true;
                minStaffing = true;
            }
            else
            {
                rdoMinStaffing.IsChecked = false;
                minStaffing = false;
            }
        }

        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            String sqlString;

            if (!minStaffing)
            {
                sqlString = "SELECT CONCAT([REVINT].[HEALTHCARE\\eliprice].[ED_Employees].[FirstName], ' ' , [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].[LastName]) AS [Employee], ";

                if (totalHours)
                {
                    sqlString += "[REVINT].[dbo].[ED_Seats].Name AS [Seat], SUM(DATEDIFF(MI, [REVINT].[dbo].[ED_Shifts].[StartShift], [REVINT].[dbo].[ED_Shifts].[EndShift])/60.0) AS [Hours Worked] ";
                }
                else
                {
                    sqlString += "[REVINT].[dbo].[ED_Roles].[Title] AS [Role], [REVINT].[dbo].[ED_Seats].Name AS [Seat], [REVINT].[dbo].[ED_Shifts].[StartShift] AS [Start Date], [REVINT].[dbo].[ED_Shifts].[EndShift] AS [End Date], DATEDIFF(MI, [REVINT].[dbo].[ED_Shifts].[StartShift], [REVINT].[dbo].[ED_Shifts].[EndShift])/60.0 AS [Hours Worked] ";
                }

                sqlString += "FROM [REVINT].[dbo].[ED_Shifts] ";
                sqlString += "JOIN [REVINT].[dbo].[ED_Seats] ON [REVINT].[dbo].[ED_Seats].[Id] = [REVINT].[dbo].[ED_Shifts].Seat ";
                sqlString += "JOIN [REVINT].[HEALTHCARE\\eliprice].[ED_Employees] ON [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].[Id] = [REVINT].[dbo].[ED_Shifts].Employee ";
                sqlString += "JOIN [REVINT].[dbo].[ED_Roles] ON [REVINT].[dbo].[ED_Roles].[Id] = [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].[Role] ";

                if (cboEmployee.Text != "" || cboSeat.Text != "" || cboRole.Text != "" || dtTPEnd.Text != "" || dtTPStart.Text != "" || tmDayEnd.Value.ToString() != "" || tmDayStart.Value.ToString() != "")
                {
                    sqlString += "WHERE ";
                    bool first = true;

                    if (cboEmployee.Text != "")
                    {
                        String[] firstLast = cboEmployee.Text.ToString().Split(null);
                        sqlString += "([REVINT].[HEALTHCARE\\eliprice].[ED_Employees].[FirstName] = '" + firstLast[0] + "' AND [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].LastName = '" + firstLast[1] + "') ";
                        first = false;
                    }
                    if (cboSeat.Text != "")
                    {
                        if (!first)
                        {
                            sqlString += " AND ";
                        }
                        sqlString += "([REVINT].[dbo].[ED_Seats].[Name] = '" + cboSeat.Text + "') ";
                        first = false;
                    }
                    if (cboRole.Text != "")
                    {
                        if (!first)
                        {
                            sqlString += " AND ";
                        }
                        sqlString += "([REVINT].[dbo].[ED_Roles].[Title] = '" + cboRole.Text + "') ";
                        first = false;
                    }
                    if (dtTPStart.Text != "")
                    {
                        if (!first)
                        {
                            sqlString += " AND ";
                        }
                        sqlString += "([REVINT].[dbo].[ED_Shifts].[StartShift] >= '" + dtTPStart.Text + "') ";
                        first = false;
                    }
                    if (dtTPEnd.Text != "")
                    {
                        if (!first)
                        {
                            sqlString += " AND ";
                        }
                        sqlString += "([REVINT].[dbo].[ED_Shifts].[EndShift] <= '" + dtTPEnd.Text + "') ";
                        first = false;
                    }
                    if (tmDayStart.Value.ToString() != "")
                    {
                        if (!first)
                        {
                            sqlString += " AND ";
                        }
                        sqlString += "(CAST([REVINT].[dbo].[ED_Shifts].[StartShift] AS time) >= '" + tmDayStart.Value.ToString() + "') ";
                        first = false;
                    }
                    if (tmDayEnd.Value.ToString() != "")
                    {
                        if (!first)
                        {
                            sqlString += " AND ";
                        }
                        sqlString += "(CAST([REVINT].[dbo].[ED_Shifts].[EndShift] AS time) <= '" + tmDayEnd.Value.ToString() + "') ";
                        first = false;
                    }
                }

                if (totalHours)
                {
                    sqlString += "GROUP BY [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].LastName, [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].FirstName, [REVINT].[dbo].[ED_Seats].Name " +
                        "ORDER BY [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].LastName, [REVINT].[HEALTHCARE\\eliprice].[ED_Employees].FirstName";
                }
                else
                {
                    sqlString += "ORDER BY [REVINT].[dbo].[ED_Shifts].[StartShift], [REVINT].[dbo].[ED_Shifts].[EndShift]";
                }
            }
            else
            {
                if (dtTPStart.Text != null)
                {
                    sqlString = "SELECT Staffing.TimeSlot AS [Time Slot], Staffing.MinStaffing AS [Minimum Staffing Requirement]" +
                        ", COUNT(Shifts.Id) AS [Amount Staffed]" +
                        ", CASE WHEN COUNT(Shifts.Id) < Staffing.MinStaffing THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                        " FROM [REVINT].[dbo].[ED_Staffing] Staffing" +
                        " LEFT JOIN [REVINT].[dbo].[ED_Shifts] Shifts" +
                        " ON CONVERT(datetime, CONCAT('" + DateTime.Parse(dtTPStart.Text).ToShortDateString() + " ', Staffing.TimeSlot)) BETWEEN Shifts.StartShift AND Shifts.EndShift AND Shifts.Employee IS NOT NULL" +
                        " GROUP BY Staffing.Id, Staffing.TimeSlot, Staffing.MinStaffing";
                    //" [REVINT].[dbo].[ED_Staffing].TimeSlot BETWEEN CAST([REVINT].[dbo].[ED_Shifts].StartShift AS time) AND CAST([REVINT].[dbo].[ED_Shifts].EndShift AS time)";
                }
                else
                {
                    sqlString = "";
                }
                /*
                if (dtTPEnd.Text != "" && dtTPStart.Text != "")
                {
                    sqlString += "AND [REVINT].[dbo].[ED_Shifts].StartShift >'" + dtTPStart.Text + "'AND [REVINT].[dbo].[ED_Shifts].EndShift <'" + dtTPEnd.Text + "'";
                }*/
                /*
                sqlString = "WITH StaffingReport AS (SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement]" +
                    ", COUNT([REVINT].[dbo].[ED_Shifts].Id) AS [Amount Staffed]" +
                    "FROM [REVINT].[dbo].[ED_Staffing] " +
                    "LEFT JOIN [REVINT].[dbo].[ED_Shifts] " +
                    "ON [REVINT].[dbo].[ED_Staffing].TimeSlot BETWEEN CAST([REVINT].[dbo].[ED_Shifts].StartShift AS time) AND CAST([REVINT].[dbo].[ED_Shifts].EndShift AS time)";
                if (dtTPEnd.Text != "" && dtTPStart.Text != "")
                {
                    sqlString += "AND [REVINT].[dbo].[ED_Shifts].StartShift >'" + dtTPStart.Text + "'AND [REVINT].[dbo].[ED_Shifts].EndShift <'" + dtTPEnd.Text + "'";
                }
                sqlString += " AND NOT [REVINT].[dbo].[ED_Shifts].[Employee] IS NULL GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing";
                sqlString += " UNION ALL";
                sqlString += " SELECT [REVINT].[dbo].[ED_Staffing].TimeSlot AS [Time Slot], [REVINT].[dbo].[ED_Staffing].MinStaffing AS [Minimum Staffing Requirement], COUNT([REVINT].[dbo].[ED_Shifts].Id) AS [Amount Staffed]";
                sqlString += " FROM [REVINT].[dbo].[ED_Staffing] LEFT JOIN [REVINT].[dbo].[ED_Shifts] ON [REVINT].[dbo].[ED_Staffing].TimeSlot = '00:00:00' ";
                sqlString += " AND (CAST([REVINT].[dbo].[ED_Shifts].EndShift AS date) > CAST([REVINT].[dbo].[ED_Shifts].StartShift AS date))";
                if (dtTPEnd.Text != "" && dtTPStart.Text != "")
                {
                   sqlString += " AND [REVINT].[dbo].[ED_Shifts].StartShift >'" + dtTPStart.Text + "'AND [REVINT].[dbo].[ED_Shifts].StartShift <'" + dtTPEnd.Text + "'";
                }
                sqlString += " AND NOT [REVINT].[dbo].[ED_Shifts].[Employee] IS NULL GROUP BY [REVINT].[dbo].[ED_Staffing].Id, [REVINT].[dbo].[ED_Staffing].TimeSlot, [REVINT].[dbo].[ED_Staffing].MinStaffing)";
                sqlString += " SELECT [Time Slot], [Minimum Staffing Requirement], SUM ([Amount Staffed]) AS [Amount Staffed]"+
                    ", CASE WHEN SUM([Amount Staffed]) < [Minimum Staffing Requirement] THEN 'Understaffed' ELSE 'Sufficiently Staffed' END AS [Staffing Status]" +
                    "  FROM StaffingReport GROUP BY [Time Slot], [Minimum Staffing Requirement] ORDER BY [Time Slot]";
                */
                //sqlString = sqlString;
            }
            setWindows(sqlString);
            btnExportToExcel.Visibility = Visibility.Visible;

        }
        private void setWindows(String sqlString)
        {
            String cxnString = "Driver={SQL Server};Server=HC-sql7;Database=REVINT;Trusted_Connection=yes;";

            //create an OdbcConnection object and connect it to the data source.
            using (OdbcConnection dbConnection = new OdbcConnection(cxnString))
            {
                //open OdbcConnection object
                dbConnection.Open();

                //Create adapter from connection and sql to obtain desired data
                OdbcDataAdapter dadapter = new OdbcDataAdapter(sqlString, dbConnection);

                //Create a table and fill it with the data from the adapter
                DataTable dtable = new DataTable();
                dadapter.Fill(dtable);

                //set the contents of the gui grid table to the data table created
                this.dtaReport.ItemsSource = dtable.DefaultView;
                this.dtaReport.CanUserAddRows = false;

                //Close connection
                dbConnection.Close();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cboEmployee.SelectedIndex = -1;
            cboRole.SelectedIndex = -1;
            cboSeat.SelectedIndex = -1;
            dtTPEnd.Text = "";
            dtTPStart.Text = "";
            tmDayEnd.Value = null;
            tmDayStart.Value = null;
            minStaffing = false;
            totalHours = false;
            rdoMinStaffing.IsChecked = false;
            rdoTotalHours.IsChecked = false;

            dtaReport.ItemsSource = null;
            btnExportToExcel.Visibility = Visibility.Hidden;
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application app = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                workbook = app.Workbooks.Add();
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)app.ActiveSheet;

                // Put Column Header into excel work sheet
                for (int i = 0; i < dtaReport.Columns.Count; i++)
                {
                    worksheet.Range["A1"].Offset[0, i].Value = dtaReport.Columns[i].Header;
                }

                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[1];
                firstRow.Cells.Interior.ColorIndex = 36;

                worksheet.Application.ActiveWindow.SplitRow = 1;
                worksheet.Application.ActiveWindow.FreezePanes = true;
                firstRow.EntireRow.Font.Bold = true;

                BorderAround(firstRow, 0);

                for (int rowIndex = 0; rowIndex < dtaReport.Items.Count; rowIndex++)
                    for (int columnIndex = 0; columnIndex < dtaReport.Columns.Count; columnIndex++)
                    {
                        worksheet.Range["A2"].Offset[rowIndex, columnIndex].Value =
                          (dtaReport.Items[rowIndex] as DataRowView).Row.ItemArray[columnIndex].ToString();
                    }
                worksheet.Columns.AutoFit();
                app.Visible = true;
            }
            catch (Exception)
            {
                Console.Write("Error");
            }
        }

        private void BorderAround(Microsoft.Office.Interop.Excel.Range range, int color)
        {
            Microsoft.Office.Interop.Excel.Borders borders = range.Borders;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            borders.Color = color;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            borders = null;
        }
    }
}
