﻿#pragma checksum "..\..\IndividualSchedule.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "372CD9D73630ADBADB099BF0A7CADF8A"
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


namespace ED_Work_Assignments {
    
    
    /// <summary>
    /// IndividualSchedule
    /// </summary>
    public partial class IndividualSchedule : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\IndividualSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border bdrEmployeeSchedule;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\IndividualSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblSchedule;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\IndividualSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtStart;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\IndividualSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtEnd;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\IndividualSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgSchedule;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\IndividualSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblEmployeeSchedule;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\IndividualSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboEmployee;
        
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
            System.Uri resourceLocater = new System.Uri("/ED Work Assignments;component/individualschedule.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\IndividualSchedule.xaml"
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
            this.bdrEmployeeSchedule = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.lblSchedule = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.dtStart = ((System.Windows.Controls.DatePicker)(target));
            
            #line 9 "..\..\IndividualSchedule.xaml"
            this.dtStart.CalendarClosed += new System.Windows.RoutedEventHandler(this.CalendarClosed);
            
            #line default
            #line hidden
            return;
            case 4:
            this.dtEnd = ((System.Windows.Controls.DatePicker)(target));
            
            #line 10 "..\..\IndividualSchedule.xaml"
            this.dtEnd.CalendarClosed += new System.Windows.RoutedEventHandler(this.CalendarClosed);
            
            #line default
            #line hidden
            return;
            case 5:
            this.dgSchedule = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            this.lblEmployeeSchedule = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.cboEmployee = ((System.Windows.Controls.ComboBox)(target));
            
            #line 15 "..\..\IndividualSchedule.xaml"
            this.cboEmployee.DropDownClosed += new System.EventHandler(this.cboEmployee_DropDownClosed);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

