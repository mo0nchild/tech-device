﻿#pragma checksum "..\..\..\..\Views\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7E5332DB07006B3A314D0F508962CF5C8D395939"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using TechDevice.Wattmeter.Components.WattmeterGauge2;
using TechDevice.Wattmeter.Views;


namespace TechDevice.Wattmeter.Views {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TechDevice.Wattmeter.Views.MainWindow @this;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox power;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\..\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border stateBlock;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\..\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TechDevice.Wattmeter.Components.WattmeterGauge2.WattmeterGauge2 wattmeterGauge;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TechDevice.Wattmeter;component/views/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.3.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.@this = ((TechDevice.Wattmeter.Views.MainWindow)(target));
            return;
            case 2:
            
            #line 17 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ConnectionButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 18 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ConnectWiresButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 57 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonAmperage_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 60 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonAmperage_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.power = ((System.Windows.Controls.CheckBox)(target));
            
            #line 69 "..\..\..\..\Views\MainWindow.xaml"
            this.power.Checked += new System.Windows.RoutedEventHandler(this.Power_Checked);
            
            #line default
            #line hidden
            
            #line 69 "..\..\..\..\Views\MainWindow.xaml"
            this.power.Unchecked += new System.Windows.RoutedEventHandler(this.Power_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 72 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonVoltage_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 75 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonVoltage_Checked);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 78 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonVoltage_Checked);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 81 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonVoltage_Checked);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 84 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonVoltage_Checked);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 87 "..\..\..\..\Views\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonVoltage_Checked);
            
            #line default
            #line hidden
            return;
            case 13:
            this.stateBlock = ((System.Windows.Controls.Border)(target));
            return;
            case 14:
            this.wattmeterGauge = ((TechDevice.Wattmeter.Components.WattmeterGauge2.WattmeterGauge2)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

