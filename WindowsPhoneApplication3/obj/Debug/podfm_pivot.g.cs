﻿#pragma checksum "C:\Users\nick\Documents\GitHub\WPOnLinePlayer\WindowsPhoneApplication3\podfm_pivot.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0A3AF9C96615C9CB3B5C3E945B318160"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.269
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WindowsPhoneApplication3 {
    
    
    public partial class podfm_pivote : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ListBox LBFav;
        
        internal System.Windows.Controls.ListBox LBRubriki;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton appbar_button1;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/WindowsPhoneApplication3;component/podfm_pivot.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LBFav = ((System.Windows.Controls.ListBox)(this.FindName("LBFav")));
            this.LBRubriki = ((System.Windows.Controls.ListBox)(this.FindName("LBRubriki")));
            this.appbar_button1 = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("appbar_button1")));
        }
    }
}

