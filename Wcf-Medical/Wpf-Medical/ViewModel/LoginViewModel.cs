using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

namespace Wpf_Medical.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Systeme de combinaison MVVM/Navigation copyright Laurent Garcin
        private Page _linkedView;
        private NavigationService _ns;
        #endregion

        #region Commandes
        private ICommand _connectCommand;
        private ICommand _createCommand;
        #endregion

        #region Binding Champs Formulaire
        private string _login;
        private string _password;

        /// Pendant l'appel au webservice, un message indique a l'utilisateur ce qui se passe
        /// a savoir VERIFICATION DE COMPTE
        private string _waitingMessage;

        #endregion

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
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public string WaitingMessage
        {
            get { return _waitingMessage; }
            set { _waitingMessage = value; }
        }

        public ICommand ConnectCommand
        {
            get { return _connectCommand; }
            set { _connectCommand = value; }
        }

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; }
        }
        

        /// <summary>
        /// constructeur
        /// </summary>
        public LoginViewModel(Page lkView)
        {
            _linkedView = lkView;

            _createCommand = new RelayCommand(param => ClickCreate(), param => true);
            _connectCommand = new RelayCommand(param => ClickConnect(), param => IsFormValid());

            _login = "";
            _password = "";
        }

        /// <summary>
        /// L'action effectuee suite a un clic sur la creation de compte
        /// </summary>
        private void ClickCreate()
        {
            View.CreateUserView window = new View.CreateUserView();
            ViewModel.CreateUserViewModel vm = new CreateUserViewModel(window);
            window.DataContext = vm;

            /// Afin de pouvoir naviguer entre les pages mais que les ViewModel ne savent pas 
            /// du tout qui elles sont liees, on garde une trace de la page liee UNIQUEMENT 
            /// pour avoir acces a son navigation service
            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        /// <summary>
        /// cette methode verifie que le formulaire soit valide afin d'activer le bouton de connection
        /// </summary>
        /// <returns></returns>
        public bool IsFormValid()
        {
            return ((_login.Length > 0) && (_password.Length > 0));
        }

        public void BeginWaitingSequence()
        {
            _waitingMessage = "Vérification du compte";
        }

        public void EndWaitingSequence()
        {
            _waitingMessage = "";
        }

        /// <summary>
        /// le clic de connection au compte
        /// </summary>
        private void ClickConnect()
        {
            ServiceUser.ServiceUserClient userService = new ServiceUser.ServiceUserClient();
            BeginWaitingSequence();
            if (userService.Connect(_login, _password))
            {
                Debug.WriteLine("REGISTERED");
            }
            else {
                Debug.WriteLine("NOT REGISTERED");
            }
            EndWaitingSequence();
        }
    }
}
