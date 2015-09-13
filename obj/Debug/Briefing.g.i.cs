﻿#pragma checksum "..\..\Briefing.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0FE2982A28D7FC2FAA9684841FB61FD4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Nihon_Mission_Maker;
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


namespace Nihon_Mission_Maker {
    
    
    /// <summary>
    /// Briefing
    /// </summary>
    public partial class Briefing : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 42 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SideSelectionBox;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem briefSideBlue;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem briefSideInd;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem briefSideRed;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem briefSideCiv;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox creditsTextBox;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox administrationTextBox;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox missionTextBox;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox situationTextBox;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\Briefing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button briefingSaveButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Nihon Mission Maker;component/briefing.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Briefing.xaml"
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
            this.SideSelectionBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 42 "..\..\Briefing.xaml"
            this.SideSelectionBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SideSelectionBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.briefSideBlue = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 3:
            this.briefSideInd = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 4:
            this.briefSideRed = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 5:
            this.briefSideCiv = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 6:
            this.creditsTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.administrationTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.missionTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.situationTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.briefingSaveButton = ((System.Windows.Controls.Button)(target));
            
            #line 84 "..\..\Briefing.xaml"
            this.briefingSaveButton.Click += new System.Windows.RoutedEventHandler(this.briefingSaveButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

