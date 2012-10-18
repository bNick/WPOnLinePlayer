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

//Added by bNick
using Microsoft.Phone.Shell;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;
using Microsoft.Phone.BackgroundAudio;
//using System.IO;
using System.IO.IsolatedStorage;
//using System.Xml;
using System.Xml.Serialization;

namespace WindowsPhoneApplication3
{
    public partial class episodes : PhoneApplicationPage
    {
        private LoadFeed Loader;
        public episodes()
        {
            InitializeComponent();

            this.PageTitle.Text = (Application.Current as App).podcastItem.Title.Text;
 
            Loader = new LoadFeed(this, String.Concat((Application.Current as App).podcastItem.Links[0].Uri, "/rss/rss.xml"));
            Loader.Run();
            Loader.Loaded += new LoadFeedEvent(UpdateUI);
        }

        public void UpdateUI()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Bind the list of SyndicationItems to our ListBox
                SyndicationFeed feed = Loader.feed;
                LBAll.ItemsSource = feed.Items;
            });
        }

        private void ImgPlay_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
            {
                BackgroundAudioPlayer.Instance.Pause();
            }
            else
            {
                SyndicationItem podcast = (SyndicationItem)LBAll.SelectedItem;
                List<string> track = new List<string>();
                track.Add(podcast.Links[1].Uri.ToString());
                track.Add(podcast.Title.Text);
                track.Add("Windows Phone Radio");
                track.Add("Windows Phone Radio Podcast");

                SerializeToIsolatedStorage<List<string>>(track, "podcast.link");
                SaveToFavorite((Application.Current as App).podcastItem);
                BackgroundAudioPlayer.Instance.Play();
            }
        }

        private void SaveToFavorite(SyndicationItem podcastTitle)
        {

            List<FavPodcats> titles = new List<FavPodcats>();
            FavPodcats favPodcastsItem = new FavPodcats();
            favPodcastsItem.Title = podcastTitle.Title.Text;
            favPodcastsItem.Description = podcastTitle.Summary.Text;
            favPodcastsItem.Count = 1;
            
            //SerializeToIsolatedStorage<SyndicationItem>(PodcastFeed, "favorite.link");
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists("Favorite.xml"))
                {
                    using (IsolatedStorageFileStream stream = store.OpenFile("Favorite.xml", FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(List<FavPodcats>));
                        titles = xml.Deserialize(stream) as List<FavPodcats>;
                        stream.Close();
                    }
                    
                    titles.Add(favPodcastsItem);
                }
                else
                {
                    titles.Add(favPodcastsItem);

                }
                using (var fileStream = store.CreateFile("Favorite.xml"))
                using (var writer = new StreamWriter(fileStream))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(List<FavPodcats>));
                    ser.Serialize(writer, titles);
                    writer.Close();
                }
            }
        }

        private static void SerializeToIsolatedStorage<T>(T obj, string filename)
        {
            if ((obj == null) || string.IsNullOrEmpty(filename))
            {
                return;
            }

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = store.CreateFile(filename))
            using (var writer = XmlWriter.Create(stream))
            {
                new XmlSerializer(obj.GetType()).Serialize(writer, obj);
            }
        }
    }
}