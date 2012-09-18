﻿using System;
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

//Added by bNick
using Microsoft.Phone.Shell;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;

namespace WindowsPhoneApplication3
{
    public partial class Page2 : PhoneApplicationPage
    {
        private ProgressIndicator progressIndicator = new ProgressIndicator()
        {
            IsVisible = true,
            IsIndeterminate = true,
            Text = "Загружаем список программ"
        };
        
        public Page2()
        {
            InitializeComponent();

            this.PageTitle.Text = (Application.Current as App).podcastItem.Title.Text;
 
            loadFeed();
        }

        private void loadFeed()
        {

            SystemTray.SetProgressIndicator(this, progressIndicator);

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new System.Uri((Application.Current as App).podcastItem.Links[0].Uri, "/rss/rss.xml"));
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
    }
}