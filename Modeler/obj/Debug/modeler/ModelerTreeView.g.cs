﻿#pragma checksum "..\..\..\modeler\ModelerTreeView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EC7C0C1FBA94D0CF2A9A27EA5F42B82F"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.17929
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace Modeler.modeler {
    
    
    /// <summary>
    /// ModelerTreeView
    /// </summary>
    public partial class ModelerTreeView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\modeler\ModelerTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView MyTree;
        
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
            System.Uri resourceLocater = new System.Uri("/Modeler;component/modeler/modelertreeview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\modeler\ModelerTreeView.xaml"
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
            
            #line 9 "..\..\..\modeler\ModelerTreeView.xaml"
            ((System.Windows.Controls.ContextMenu)(target)).Opened += new System.Windows.RoutedEventHandler(this.TreeMenu_OnOpened);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 10 "..\..\..\modeler\ModelerTreeView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemRename_OnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 11 "..\..\..\modeler\ModelerTreeView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemDelete_OnClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 12 "..\..\..\modeler\ModelerTreeView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemAdd_OnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 13 "..\..\..\modeler\ModelerTreeView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuGenerateCode_OnClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.MyTree = ((System.Windows.Controls.TreeView)(target));
            
            #line 17 "..\..\..\modeler\ModelerTreeView.xaml"
            this.MyTree.AddHandler(System.Windows.Controls.TreeViewItem.ExpandedEvent, new System.Windows.RoutedEventHandler(this.MyTree_OnExpanded));
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\modeler\ModelerTreeView.xaml"
            this.MyTree.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.MyTree_OnMouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

