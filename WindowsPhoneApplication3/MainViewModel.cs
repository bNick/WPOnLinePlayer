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

//Added by Belopolov L. Nikolay
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace WindowsPhoneApplication3
{
    public class FavPodcats
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {

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

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


}
