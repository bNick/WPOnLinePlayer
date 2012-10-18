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

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WindowsPhoneApplication3
{
    public class FavPodcastsItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int count;
        public int Count {
            get { return count; }
            set 
            {
                count = value;
                NotifyPropertyChanged("Count");
            }
        }

        private string title;
        public string Title 
        {
            get { return title; } 
            set 
            {
                title = value;
                NotifyPropertyChanged("Title");
            } 
        }

        private string description;
        public string Description 
        {
            get { return description; }
            set 
            {
                description = value;
                NotifyPropertyChanged("Description");
            } 
        }

        private string uri;
        public string Uri 
        {
            get { return uri; }
            set
            {
                uri = value;
                NotifyPropertyChanged("Uri");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class FavPodcasts : ObservableCollection<FavPodcastsItem>
    {
        public void AddFavItem(FavPodcastsItem favPodcastsItem)
        {
            if (this.Items.Count == 0)
            {
                this.Add(favPodcastsItem);
            }
            else
            {
                FavPodcastsItem item = getFromList(favPodcastsItem.Uri);
                if (item == null)
                {
                    this.Add(favPodcastsItem);
                }
                else
                {
                    item.Count = item.Count + 1;
                }
            }
        }

        private FavPodcastsItem getFromList(string id)
        {
            foreach (FavPodcastsItem j in this.Items)
            {
                if (j.Uri == id)
                { return j; }
            }
            return null;
        }
    }
}

