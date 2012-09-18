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
using System.Xml.Linq;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;
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
        public Page1()
        {
            InitializeComponent();
  
            loadFeed();
        }



        // Click handler which runs when the 'Load Feed' or 'Refresh Feed' button is clicked. 
        private void loadFeed()
        {
            
            SystemTray.SetProgressIndicator(this, progressIndicator);


            // WebClient is used instead of HttpWebRequest in this code sample because 
            // the implementation is simpler and easier to use, and we do not need to use 
            // advanced functionality that HttpWebRequest provides, such as the ability to send headers.
            WebClient webClient = new WebClient();

            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel. 
            //webClient.DownloadStringAsync(new System.Uri("http://windowsteamblog.com/windows_phone/b/windowsphone/rss.aspx"));
            webClient.DownloadStringAsync(new System.Uri("http://podfm.ru/rss/programs/rss.xml"));
        }

        // Event handler which runs after the feed is fully downloaded.
        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show(e.Error.Message);
                });
            }
            else
            {
                

                // Save the feed into the State property in case the application is tombstoned. 
                this.State["feed"] = e.Result;
                progressIndicator.IsVisible = false;
                UpdateFeedList(e.Result);
                
            }
        }

        // This method sets up the feed and binds it to our ListBox. 
        private void UpdateFeedList(string feedXML)
        {
            // Load the feed into a SyndicationFeed instance
            StringReader stringReader = new StringReader(feedXML);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Bind the list of SyndicationItems to our ListBox
                LBAll.ItemsSource = feed.Items;
            });
        }

        private void LBAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SyndicationItem i = (SyndicationItem)LBAll.SelectedItem;
            (Application.Current as App).podcastItem = (SyndicationItem)LBAll.SelectedItem;
            NavigationService.Navigate(new Uri("/page2.xaml", UriKind.Relative));
        }
    }
}