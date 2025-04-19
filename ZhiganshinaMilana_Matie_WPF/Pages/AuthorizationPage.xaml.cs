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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public static List<Employee> employees {  get; set; }
        public AuthorizationPage()
        {
            InitializeComponent();
            employees = new List<Employee>(DbConnection.matieEntities.Employee.ToList());

            this.DataContext = this;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTb.Text.Trim();
            string password = PasswordTb.Password.Trim();

            employees = new List<Employee>(DbConnection.matieEntities.Employee.ToList());
            Employee currentUser = employees.FirstOrDefault(i => i.Login == login && i.Password == password);
            if (currentUser != null)
            {
                NavigationService.Navigate(new HomePage());
            }
            else
                MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
