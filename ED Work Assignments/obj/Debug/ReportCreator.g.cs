﻿#pragma checksum "..\..\ReportCreator.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7CBC8BD4ABE6C6AFC2A351AF5EE5568D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.Toolkit;


namespace ED_Work_Assignments {
    
    
    /// <summary>
    /// ReportCreator
    /// </summary>
    public partial class ReportCreator : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboEmployee;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboSeat;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtTPStart;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtTPEnd;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.TimePicker tmDayStart;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.TimePicker tmDayEnd;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtaReport;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboSpecialTag;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdoTotalHours;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\ReportCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGenerateReport;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ED Work Assignments;component/reportcreator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ReportCreator.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cboEmployee = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.cboSeat = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.dtTPStart = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.dtTPEnd = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            this.tmDayStart = ((Xceed.Wpf.Toolkit.TimePicker)(target));
            return;
            case 6:
            this.tmDayEnd = ((Xceed.Wpf.Toolkit.TimePicker)(target));
            return;
            case 7:
            this.dtaReport = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.cboSpecialTag = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.rdoTotalHours = ((System.Windows.Controls.RadioButton)(target));
            
            #line 29 "..\..\ReportCreator.xaml"
            this.rdoTotalHours.Click += new System.Windows.RoutedEventHandler(this.rdoTotalHours_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnGenerateReport = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\ReportCreator.xaml"
            this.btnGenerateReport.Click += new System.Windows.RoutedEventHandler(this.btnGenerateReport_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

