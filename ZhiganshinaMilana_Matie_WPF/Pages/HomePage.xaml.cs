using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZhiganshinaMilana_Matie_WPF.DB;

namespace ZhiganshinaMilana_Matie_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public static List<Service> services {  get; set; }
        public static List<Master> masters {  get; set; }
        public static List<ServiceMaster> serviceMasters { get; set; }

        private int _pageSize = 3;
        private int _pageNumber = 1;
        private int _pageCount = 0;
        public HomePage()
        {
            InitializeComponent();
            Refresh();
            UpdateNavigation();

            serviceMasters = new List<ServiceMaster>(DbConnection.matieEntities.ServiceMaster.ToList());
            services = new List<Service>(DbConnection.matieEntities.Service.ToList());
            ServiceLV.ItemsSource = services;

            this.DataContext = this;
        }

        private void Refresh()
        {
            var filterProduct = DbConnection.matieEntities.Service.ToList();

            if(NameTb.Text.Length > 0)
            {
                filterProduct = filterProduct.Where(i => i.Name.ToLower().StartsWith(NameTb.Text.Trim().ToLower())).ToList();
            }


            _pageCount = (int)Math.Ceiling((double)filterProduct.Count / _pageSize);
            filterProduct = filterProduct.Skip((_pageNumber - 1) * _pageSize).Take(_pageSize).ToList();
            UpdateNavigation();

            ServiceLV.ItemsSource = filterProduct;
        }

        private void UpdateNavigation()
        {
            NavSp.Children.Clear();

            if (_pageCount > 1)
            {
                Button button1 = new Button
                {
                    Content = "<",
                    IsHitTestVisible = _pageNumber > 1,
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderBrush = new SolidColorBrush(Colors.Transparent),
                };
                button1.Click += PageBtn_Click;
                NavSp.Children.Add(button1);

                for (int i = 1; i <= _pageCount; i++)
                {
                    TextBlock textBlock = new TextBlock()
                    {
                        Text = i.ToString(),
                        TextDecorations = i == _pageNumber ? TextDecorations.Underline : null 
                    };

                    Button button2 = new Button
                    {
                        Content = textBlock,
                        IsHitTestVisible = i != _pageNumber,
                        Background = new SolidColorBrush(Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Colors.Transparent)
                    };
                    button2.Click += PageBtn_Click;
                    button2.Tag = i; 
                    NavSp.Children.Add(button2);
                }

                Button button3 = new Button
                {
                    Content = ">",
                    IsHitTestVisible = _pageNumber < _pageCount,
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderBrush = new SolidColorBrush(Colors.Transparent)
                };
                button3.Click += PageBtn_Click;
                NavSp.Children.Add(button3);
            }
        }
        private void PageBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Content.ToString())
            {
                case "<":
                    _pageNumber--;
                    break;
                case ">":
                    _pageNumber++;
                    break;
                default:
                    _pageNumber = int.Parse(((TextBlock)button.Content).Text);
                    break;
            }
            Refresh();
        }
        private void MinimizedBt_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.WindowState = WindowState.Minimized;
        }

        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void NameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());

        }
    }
}
