﻿#pragma checksum "..\..\..\Dialogs\EditDictDialog.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A84C01E0EB77F794D64767936E84703AE037B0A3F470DFF7345BFFC8F77FF1AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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
using TTS.Dialogs;


namespace TTS.Dialogs {
    
    
    /// <summary>
    /// EditDictDialog
    /// </summary>
    public partial class EditDictDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\Dialogs\EditDictDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel mainDictContent;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\Dialogs\EditDictDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox fromBox;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\Dialogs\EditDictDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox toBox;
        
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
            System.Uri resourceLocater = new System.Uri("/TTS;component/dialogs/editdictdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Dialogs\EditDictDialog.xaml"
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
            this.mainDictContent = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.fromBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 95 "..\..\..\Dialogs\EditDictDialog.xaml"
            this.fromBox.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.SetActiveBoxHandler);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 100 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SpeakFromHandler);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 119 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddStarCharHandler);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 124 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddLeftCornerBracketCharHandler);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 134 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddRightCornerBracketCharHandler);
            
            #line default
            #line hidden
            return;
            case 7:
            this.toBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 149 "..\..\..\Dialogs\EditDictDialog.xaml"
            this.toBox.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.SetActiveBoxHandler);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 154 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SpeakToHandler);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 170 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NewHandler);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 179 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddHandler);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 185 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.InsertHandler);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 195 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ReplaceHandler);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 201 "..\..\..\Dialogs\EditDictDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveHandler);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

