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
            //this.Items = new ObservableCollection<ItemViewModel>();
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

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                this.Items = new ObservableCollection<ItemViewModel>();

                this.Items.Add(new ItemViewModel() { Category = "ВСЕ" });
                this.Items.Add(new ItemViewModel() { Category = "аудиокниги" });
                this.Items.Add(new ItemViewModel() { Category = "аудиогиды" });
                this.Items.Add(new ItemViewModel() { Category = "политика" });
                this.Items.Add(new ItemViewModel() { Category = "бизнес" });
                this.Items.Add(new ItemViewModel() { Category = "технологии" });
                this.Items.Add(new ItemViewModel() { Category = "автомобили" });
                this.Items.Add(new ItemViewModel() { Category = "карьера" });
                this.Items.Add(new ItemViewModel() { Category = "наука" });
                this.Items.Add(new ItemViewModel() { Category = "интернет" });
                this.Items.Add(new ItemViewModel() { Category = "кино" });
                this.Items.Add(new ItemViewModel() { Category = "культура и искуство" });
                this.Items.Add(new ItemViewModel() { Category = "развлечения" });
                this.Items.Add(new ItemViewModel() { Category = "путешествия" });
                this.Items.Add(new ItemViewModel() { Category = "история" });
                this.Items.Add(new ItemViewModel() { Category = "здоровье" });
                this.Items.Add(new ItemViewModel() { Category = "новости" });
                this.Items.Add(new ItemViewModel() { Category = "красота" });
                this.Items.Add(new ItemViewModel() { Category = "спорт" });
                this.Items.Add(new ItemViewModel() { Category = "образование" });

                return this.Items;
            }
            //private set
            //{
                
            //}
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
