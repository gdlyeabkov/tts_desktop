﻿#pragma checksum "..\..\..\Dialogs\TranslateDialog.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B13D170790F7AD7891787A5088BC29F0866FA102AB1D61C3BAE760FB8CE9AE67"
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
    /// TranslateDialog
    /// </summary>
    public partial class TranslateDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\Dialogs\TranslateDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inputBox;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Dialogs\TranslateDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox fromLangSelector;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\Dialogs\TranslateDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox toLangSelector;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\..\Dialogs\TranslateDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox outputBox;
        
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
            System.Uri resourceLocater = new System.Uri("/TTS;component/dialogs/translatedialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Dialogs\TranslateDialog.xaml"
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
            this.inputBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            
            #line 32 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.InsertFromBufferHandler);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 44 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.InsertFromDocHandler);
            
            #line default
            #line hidden
            return;
            case 4:
            this.fromLangSelector = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            
            #line 82 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ToggleLangsHandler);
            
            #line default
            #line hidden
            return;
            case 6:
            this.toLangSelector = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            
            #line 120 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TranslateHandler);
            
            #line default
            #line hidden
            return;
            case 8:
            this.outputBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            
            #line 139 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CopyOutputHandler);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 151 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenAsNewDocHandler);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 174 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenSettingsHandler);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 181 "..\..\..\Dialogs\TranslateDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelHandler);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

