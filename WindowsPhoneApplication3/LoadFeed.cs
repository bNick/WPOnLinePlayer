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
using Microsoft.Phone.Shell;

namespace WindowsPhoneApplication3
{
    public delegate void LoadFeedEvent();
    
    public class LoadFeed
    {
        public event LoadFeedEvent Loaded;
        public SyndicationFeed feed { get; set; }
        private Page _sender { get; set; }
        private Uri _uri { get; set; }

        public LoadFeed(Page sender, Uri uri)
        {
            _uri = uri;
            _sender = sender;
        }

        public void Run()
        {
            SystemTray.SetProgressIndicator(_sender, progressIndicator);

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(_uri);
        }

        
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
                if (Loaded != null) Loaded();
            }
        }
    }
}
