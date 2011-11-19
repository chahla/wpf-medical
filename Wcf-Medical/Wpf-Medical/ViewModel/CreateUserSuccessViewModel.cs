using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Wpf_Medical.ViewModel
{
    class CreateUserSuccessViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        #region Commandes
        private ICommand _loginCommand;

        public ICommand LoginCommand
        {
            get { return _loginCommand; }
            set { _loginCommand = value; }
        }
        #endregion

        private string _name;
        private string _firstname;
        private string _login;
        private string _password;
        private string _role;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    OnPropertyChanged("Name");
                }
                _name = value;
            }
        }

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (_firstname != value)
                {
                    OnPropertyChanged("Firstname");
                }
                _firstname = value;
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    OnPropertyChanged("Login");
                }
                _login = value;
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    OnPropertyChanged("Password");
                }
                _password = value;
            }
        }

        public string Role
        {
            get { return _role; }
            set
            {
                if (_role != value)
                {
                    OnPropertyChanged("Role");
                }
                _role = value;
            }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public CreateUserSuccessViewModel(Page lkView)
        {
            _linkedView = lkView;

            _loginCommand = new RelayCommand(param => ToFirstLogin(), param => true);

            ServiceUser.User createdUser = NavigationMessenger.GetInstance().TransitUser;

            _name = createdUser.Name;
            _firstname = createdUser.Firstname;
            _login = createdUser.Login;
            _password = createdUser.Pwd;
            _role = createdUser.Role;
        }

        /// <summary>
        /// Le clic sur le bouton permet de se connecter pour la premiere fois
        /// </summary>
        private void ToFirstLogin()
        {
            View.LoginView window = new View.LoginView();
            ViewModel.LoginViewModel vm = new LoginViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }
    }
}
