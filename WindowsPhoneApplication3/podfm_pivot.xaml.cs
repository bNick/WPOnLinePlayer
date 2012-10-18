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

using System.Collections.ObjectModel;

namespace WindowsPhoneApplication3
{
    public partial class podfm_pivote : PhoneApplicationPage
    {
        private LoadFeed Loader;

        public podfm_pivote()
        {
            InitializeComponent();
            
            //Loader = new LoadFeed(this, "http://podfm.ru/rss/programs/rss.xml");
            Loader = new LoadFeed(this);
            UpdateFav(Loader.Favorites());
            Loader.Programs();
            Loader.Loaded += new LoadFeedEvent(UpdateUI);
        }

        private void UpdateFav(List<FavPodcats> podcastTitle)
        {
            LBFav.ItemsSource = podcastTitle;
        }
        
        public void UpdateUI()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                LBRubriki.ItemsSource = Loader.cat;
            });
        }
                
        private void LBRubriki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (LBRubriki.SelectedIndex == 0)
            //{
            //    (Application.Current as App).CategoryItem = (Items)LBRubriki.SelectedItem;
            //    NavigationService.Navigate(new Uri("/podfmpodcastlist.xaml", UriKind.Relative));
            //}
            (Application.Current as App).CategoryItem = (Items)LBRubriki.SelectedItem;
            NavigationService.Navigate(new Uri("/podfmpodcastlist.xaml", UriKind.Relative));
        }
    }
}