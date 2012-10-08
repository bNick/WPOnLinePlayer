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
using System.ServiceModel.Syndication;
using Microsoft.Phone.Shell;

namespace WindowsPhoneApplication3
{
    public partial class Page1 : PhoneApplicationPage
    {

        private ProgressIndicator progressIndicator = new ProgressIndicator()
        {
            IsVisible = true,
            IsIndeterminate = true,
            Text = "Загружаем список программ"
        };

        private LoadFeed Loader;

        public Page1()
        {
            InitializeComponent();

            Loader = new LoadFeed(this, new System.Uri("http://podfm.ru/rss/programs/rss.xml"));
            Loader.Run();
            Loader.Loaded += new LoadFeedEvent(UpdateUI);
        }

        public void UpdateUI()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Bind the list of SyndicationItems to our ListBox
                SyndicationFeed feed = Loader.feed;
                LBAll.ItemsSource = feed.Items.Where(x => x.Categories[0].Name == (Application.Current as App).CategoryItem.Category);
            });
        }

        private void LBAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SyndicationItem i = (SyndicationItem)LBAll.SelectedItem;
            (Application.Current as App).podcastItem = (SyndicationItem)LBAll.SelectedItem;
            NavigationService.Navigate(new Uri("/Episodes.xaml", UriKind.Relative));
        }
    }
}