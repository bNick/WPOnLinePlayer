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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace WindowsPhoneApplication3
{
    public delegate void LoadFeedEvent();
    
    public class LoadFeed
    {
        public event LoadFeedEvent Loaded;
        public SyndicationFeed feed { get; set; }
        public Categories cat { get; set; }
        public ObservableCollection<ItemViewModel> Items { get; set; }

        private Page _sender { get; set; }
        private string _uri { get; set; }
        

        public LoadFeed(Page sender, string uri)
        {
            _uri = uri;
            _sender = sender;
        }

        public LoadFeed(Page sender)
        {
            //_uri = uri;
            _sender = sender;
        }

        public void Programs()
        {
            const string uri = "http://podfm.ru/rss/programs/rss.xml";

            Download(uri);
        }

        private void Download(string uri)
        {
            SystemTray.SetProgressIndicator(_sender, progressIndicator);

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new System.Uri(uri));
        }

        public void Run()
        {
            Download(_uri);
            
            //SystemTray.SetProgressIndicator(_sender, progressIndicator);

            //WebClient webClient = new WebClient();
            //webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            //webClient.DownloadStringAsync(_uri);
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
            try
            {
                InitItems();
            }
            catch { }
            if (feed != null)
            {
                if (Loaded != null) Loaded();
            }
        }

        private void InitItems()
        {
            //From feed select and count category
            cat = new Categories();

            cat.CreateCategories(feed);
        }

        internal System.Collections.Generic.List<string> Favorites()
        {
            List<string> podcastTitle = new List<string>();
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                if (storage.FileExists("Favorite.xml"))
                {
                    using (IsolatedStorageFileStream stream = storage.OpenFile("Favorite.xml", FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(List<string>));
                        podcastTitle = xml.Deserialize(stream) as List<string>;
                        stream.Close();
                    }
                }
            return podcastTitle;
        }
    }
}
