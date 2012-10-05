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

using System.Collections.ObjectModel;

namespace WindowsPhoneApplication3
{
    public partial class podfm_pivote : PhoneApplicationPage
    {
        private LoadFeed Loader;
        //public ObservableCollection<ItemViewModel> Items { get; private set; }

        //public podfm_pivote()
        //{
        //    InitializeComponent();
        //    DataContext = App.ViewModel;
        //    this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        //}
        public podfm_pivote()
        {
            InitializeComponent();
            //this.Tag = "podfm_pivote";

            //this.Items = new ObservableCollection<ItemViewModel>();

            Loader = new LoadFeed(this, new System.Uri("http://podfm.ru/rss/programs/rss.xml"));
            Loader.Run();
            Loader.Loaded += new LoadFeedEvent(UpdateUI);
        }

        public void UpdateUI()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Bind the list of SyndicationItems to our ListBox
                //SyndicationFeed feed = Loader.feed;

                //this.Items.Add(new ItemViewModel() { Category = "ВСЕ" });
                //this.Items.Add(new ItemViewModel() { Category = "аудиокниги" });
                //this.Items.Add(new ItemViewModel() { Category = "аудиогиды" });
                //this.Items.Add(new ItemViewModel() { Category = "политика" });
                //this.Items.Add(new ItemViewModel() { Category = "бизнес" });
                //this.Items.Add(new ItemViewModel() { Category = "технологии" });
                //this.Items.Add(new ItemViewModel() { Category = "автомобили" });
                //this.Items.Add(new ItemViewModel() { Category = "карьера" });
                //this.Items.Add(new ItemViewModel() { Category = "наука" });
                //this.Items.Add(new ItemViewModel() { Category = "интернет" });
                //this.Items.Add(new ItemViewModel() { Category = "кино" });
                //this.Items.Add(new ItemViewModel() { Category = "культура и искуство" });
                //this.Items.Add(new ItemViewModel() { Category = "развлечения" });
                //this.Items.Add(new ItemViewModel() { Category = "путешествия" });
                //this.Items.Add(new ItemViewModel() { Category = "история" });
                //this.Items.Add(new ItemViewModel() { Category = "здоровье" });
                //this.Items.Add(new ItemViewModel() { Category = "новости" });
                //this.Items.Add(new ItemViewModel() { Category = "красота" });
                //this.Items.Add(new ItemViewModel() { Category = "спорт" });
                //this.Items.Add(new ItemViewModel() { Category = "образование" });

                LBRubriki.ItemsSource = Loader.Items;
            });
        }

        //// Загрузка данных для элементов ViewModel
        //private void MainPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (!App.ViewModel.IsDataLoaded)
        //    {
        //        App.ViewModel.LoadData();
        //    }
        //}
        
        private void LBRubriki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBRubriki.SelectedIndex == 0)
            {
                NavigationService.Navigate(new Uri("/podfmpodcastlist.xaml", UriKind.Relative));
            }
        }
    }
}