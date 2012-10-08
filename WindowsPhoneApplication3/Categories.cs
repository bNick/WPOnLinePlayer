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

using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;

namespace WindowsPhoneApplication3
{
    public class Items : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                NotifyPropertyChanged("Count");
            }
        }

        private string name;
        public string Category
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
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

    public class Categories : ObservableCollection<Items>
    {
        public void CreateCategories(SyndicationFeed feed)
        {
            int counter = 0;
            foreach (var i in feed.Items)
            {
                string name = i.Categories[0].Name;
                Items cat = getFromList(name);
                if (cat == null)
                {
                    cat = new Items { Category = i.Categories[0].Name, Count = 1 };
                    this.Add(cat);
                }
                else
                {
                    cat.Count = cat.Count + 1;
                }
                counter++;
            }
            this.Insert(0, new Items {Category = "ВСЕ", Count = counter });
        }

        private Items getFromList(string name)
        {
            foreach (Items j in this)
            {
                if (j.Category == name)
                { return j; }
            }
            return null;
        }

    }
}
