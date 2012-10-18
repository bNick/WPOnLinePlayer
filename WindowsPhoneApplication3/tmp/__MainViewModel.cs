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
    

    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<_ItemViewModel>();
        }

        public ObservableCollection<_ItemViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {

            this.Items.Add(new _ItemViewModel() { Category = "ВСЕ" });
            this.Items.Add(new _ItemViewModel() { Category = "аудиокниги" });
            this.Items.Add(new _ItemViewModel() { Category = "аудиогиды" });
            this.Items.Add(new _ItemViewModel() { Category = "политика" });
            this.Items.Add(new _ItemViewModel() { Category = "бизнес" });
            this.Items.Add(new _ItemViewModel() { Category = "технологии" });
            this.Items.Add(new _ItemViewModel() { Category = "автомобили" });
            this.Items.Add(new _ItemViewModel() { Category = "карьера" });
            this.Items.Add(new _ItemViewModel() { Category = "наука" });
            this.Items.Add(new _ItemViewModel() { Category = "интернет" });
            this.Items.Add(new _ItemViewModel() { Category = "кино" });
            this.Items.Add(new _ItemViewModel() { Category = "культура и искуство" });
            this.Items.Add(new _ItemViewModel() { Category = "развлечения" });
            this.Items.Add(new _ItemViewModel() { Category = "путешествия" });
            this.Items.Add(new _ItemViewModel() { Category = "история" });
            this.Items.Add(new _ItemViewModel() { Category = "здоровье" });
            this.Items.Add(new _ItemViewModel() { Category = "новости" });
            this.Items.Add(new _ItemViewModel() { Category = "красота" });
            this.Items.Add(new _ItemViewModel() { Category = "спорт" });
            this.Items.Add(new _ItemViewModel() { Category = "образование" });

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
