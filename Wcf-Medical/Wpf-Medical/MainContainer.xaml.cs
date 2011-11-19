using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace Wpf_Medical
{
    /// <summary>
    /// Logique d'interaction pour MainContainer.xaml
    /// </summary>
    public partial class MainContainer : NavigationWindow
    {
        public MainContainer()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            View.LoginView window = new View.LoginView();
            ViewModel.LoginViewModel vm = new ViewModel.LoginViewModel(window);
            window.DataContext = vm;

            this.Navigate(window);
        }
    }
}
