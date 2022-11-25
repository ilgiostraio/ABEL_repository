﻿#pragma checksum "..\..\..\ECSController.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D591B332B51C47451EED44EEFB3AFB1505A6A061D701615B432977E93892EC3C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Expression.Controls;
using Microsoft.Expression.Media;
using Microsoft.Expression.Shapes;
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


namespace Act.Lib.ControllersLibrary {
    
    
    /// <summary>
    /// ECSController
    /// </summary>
    public partial class ECSController : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\ECSController.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border border;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\ECSController.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ECSCanvas;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\ECSController.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Expression.Controls.LineArrow Yaxis;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\ECSController.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Expression.Controls.LineArrow Xaxis;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\ECSController.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Expression.Shapes.RegularPolygon Position_Star;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\ECSController.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label CurrentECSLabel;
        
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
            System.Uri resourceLocater = new System.Uri("/Act.Lib.ControllersLibrary;component/ecscontroller.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ECSController.xaml"
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
            
            #line 7 "..\..\..\ECSController.xaml"
            ((Act.Lib.ControllersLibrary.ECSController)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.UserControl_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.border = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.ECSCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 23 "..\..\..\ECSController.xaml"
            this.ECSCanvas.MouseMove += new System.Windows.Input.MouseEventHandler(this.MouseOnCanvas);
            
            #line default
            #line hidden
            
            #line 23 "..\..\..\ECSController.xaml"
            this.ECSCanvas.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.MouseClickOnCanvas);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Yaxis = ((Microsoft.Expression.Controls.LineArrow)(target));
            return;
            case 5:
            this.Xaxis = ((Microsoft.Expression.Controls.LineArrow)(target));
            return;
            case 6:
            this.Position_Star = ((Microsoft.Expression.Shapes.RegularPolygon)(target));
            return;
            case 7:
            this.CurrentECSLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

