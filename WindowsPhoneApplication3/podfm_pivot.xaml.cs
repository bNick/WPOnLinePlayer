using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace WindowsPhoneApplication3
{
    public partial class podfm_pivote : PhoneApplicationPage
    {
        public podfm_pivote()
        {
            InitializeComponent();
        }

        private void LBRubriki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBRubriki.SelectedIndex == 0)
            {
                LoadPodFMPodcastList();
            }
        }

        private void LoadPodFMPodcastList()
        {
            //MessageBox.Show("Loaded!!!");
            NavigationService.Navigate(new Uri("/podfmpodcastlist.xaml", UriKind.Relative));
        }
    }

    
}