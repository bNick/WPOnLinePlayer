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
using System.Xml.Serialization;

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
            MessageBox.Show("Button Add work");
            PodcastItem podcast1 = new PodcastItem { Name = "My Added PodCast #1" };
            PodcastItem podcast2 = new PodcastItem { Name = "My Added PodCast #2" };
            
            var podcastitems = new List<PodcastItem>();
            podcastitems.Add(podcast1);
            podcastitems.Add(podcast2);

            //MyList.Items.Clear();
            //MyList.ItemsSource = podcastitems;
            MyList.DataContext = podcastitems;
            MessageBox.Show(MyList.Items.Count.ToString());
        }

        public class RSS {
            private string _version = "2.0";
            public List<Channel> channel;

            [XmlAttribute]
            public string version {
                get { return _version; }
                set { _version=value; }
            }
        }

        public class Channel { 
            
        }
    }
}
