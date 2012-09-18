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
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
//using System.Runtime.Serialization;


namespace WindowsPhoneApplication3
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public class PodcastItem {
            public string Name {get; set;}
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            //MessageBox.Show("Button Add work");
            //PodcastItem podcast1 = new PodcastItem { Name = "My Added PodCast #1" };
            //PodcastItem podcast2 = new PodcastItem { Name = "My Added PodCast #2" };
            
            //var podcastitems = new List<PodcastItem>();
            //podcastitems.Add(podcast1);
            //podcastitems.Add(podcast2);

            //MyList.DataContext = podcastitems;


            //var tmp = LoadRSS("radio-t.xml");
            //var tmp = LoadRSS("rss_rpod.xml");
            var tmp = LoadRSS("podfm.xml");
            MessageBox.Show(tmp.Count.ToString());
            MyList.DataContext = tmp;
        }

        private List<RSS> LoadRSS(string path)
        {
            XDocument xdoc = XDocument.Load(path);
            var feeds = from feed in xdoc.Descendants("item")
                        select new RSS
                        {
                            Name = feed.Element("title").Value,
                            PubDate = feed.Element("pubDate").Value
                        };
            return feeds.ToList();
        }
    }

    public class RSS
    { 
        public string Name { get; set; }
        public string PubDate { get; set; }
    }
}
