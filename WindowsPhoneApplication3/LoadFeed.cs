using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Phone.Shell;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;

namespace WindowsPhoneApplication3
{
    public delegate void LoadFeedEvent();
    
    public class LoadFeed
    {
        public event LoadFeedEvent Loaded;
        
        public LoadFeed(Page sender, Uri uri, ListBox e)
        {
            _sender = sender;
            _uri = uri;
            _e = e;
        }

        //public LoadFeed(Page sender, Uri uri, Page1 e)
        //{
        //    _sender = sender;
        //    _uri = uri;
        //    _e = e;
        //}

        public void Run()
        {
            SystemTray.SetProgressIndicator(_sender, progressIndicator);

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(_uri);
        }

        public SyndicationFeed feed { get; set; }

        private Page _sender { get; set; }
        private Uri _uri { get; set; }
        private ListBox _e { get; set; }

        private ProgressIndicator progressIndicator = new ProgressIndicator()
        {
            IsVisible = true,
            IsIndeterminate = true,
            Text = "Загружаем список программ"
        };

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
                // _sender.State["feed"] = e.Result;
                progressIndicator.IsVisible = false;
                UpdateFeedList(e.Result);
            }
        }

        public void UpdateFeedList(string feedXML)
        {
            // Load the feed into a SyndicationFeed instance
            StringReader stringReader = new StringReader(feedXML);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            feed = SyndicationFeed.Load(xmlReader);
            if (feed != null)
            {
                //_e.ItemsSource = feed.Items;

                if (Loaded != null) Loaded();
            }
        }
        //public SyndicationFeed Feed { get; set; }
        //public void WorkDone() 
        //{
        //    feed = _e.ItemsSource;
        //}
    }
}
