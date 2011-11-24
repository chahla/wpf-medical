using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;
using System.ComponentModel;

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
            set
            {
                if (_waitingMessage != value)
                {
                    _waitingMessage = value;
                    OnPropertyChanged("WaitingMessage");
                }
            }
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
        
		// Si vrai, empêche le clic sur les boutons
		private bool _ischecking;

        public bool Ischecking
        {
            get { return _ischecking; }
            set { _ischecking = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public LoginViewModel(Page lkView)
        {
            _linkedView = lkView;

            _createCommand = new RelayCommand(param => ClickCreate(), param => IscheckingAccount());
            _connectCommand = new RelayCommand(param => ClickConnect(), param => IsFormValid());

            _login = "";
            _password = "";
            _waitingMessage = "";
            _ischecking = false;
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
            return ((_login.Length > 0) && (_password.Length > 0) && !_ischecking);
        }

        /// <summary>
        /// cette methode permet de desactiver le bouton de creation pendant la verification
        /// </summary>
        /// <returns></returns>
        public bool IscheckingAccount()
        {
            return (!_ischecking);
        }

        public void BeginWaitingSequence()
        {
            WaitingMessage = "Vérification du compte";
        }

        public void EndWaitingSequence()
        {
            WaitingMessage = "";
        }

        /// <summary>
        /// le clic de connection au compte
        /// </summary>
        private void ClickConnect()
        {
            /// Le bgw servant a verifier si le compte existe
            BackgroundWorker worker = new BackgroundWorker();

            /// le bgw servant a determiner le role
            BackgroundWorker workerFetchRole = new BackgroundWorker();
            
            ServiceUser.ServiceUserClient userService = new ServiceUser.ServiceUserClient();
            
            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) => 
            {
                _ischecking = true;
                e.Result = userService.Connect(_login, _password);
            });

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object s, RunWorkerCompletedEventArgs e) =>
            {
                EndWaitingSequence();
                _ischecking = false;
                // TODO voir le CreateUserViewModel pour l'implementation erreur
                if (e.Cancelled)
                {
                    WaitingMessage = "L'opération a été annulée.";
                }
                if (e.Error != null)
                {
                    WaitingMessage = "Erreur lors de l'authentification : " + e.Error.Message;
                }
                bool? res = e.Result as bool?;
                if (res == null)
                {
                    WaitingMessage = "Erreur côté serveur lors de l'authentification. Veuillez recommencer";
                }
                if (res == true)
                {
                    /// Le webservice nous indique que le compte est valide mais on ne connait
                    /// pas encore le role de l'utilisateur On doit donc faire appel a un 
                    /// autre webservice
                    workerFetchRole.RunWorkerAsync();

                    View.HomeView window = new View.HomeView();
                    ViewModel.HomeViewModel vm = new HomeViewModel(window);
                    window.DataContext = vm;

                    _ns = NavigationService.GetNavigationService(_linkedView);
                    _ns.Navigate(window);
                }
                else {
                    Debug.WriteLine("NON ENREGISTRE");
                    WaitingMessage = "Erreur de login ou mot de passe.";
                }
            });

            workerFetchRole.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                e.Result = userService.GetRole(_login);
            });

            workerFetchRole.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object s, RunWorkerCompletedEventArgs e) =>
            {
                if (e.Cancelled)
                {
                    WaitingMessage = "L'opération a été annulée.";
                }
                if (e.Error != null)
                {
                    WaitingMessage = "Erreur lors de l'authentification : " + e.Error.Message;
                }
                string res = e.Result as string;
                if (res == null)
                {
                    WaitingMessage = "Erreur côté serveur lors de l'authentification. Veuillez recommencer";
                }
                if ((res == "Chirurgien") || (res == "Medecin") || (res == "Radiologue")) {
                    NavigationMessenger.GetInstance().IsRWAccount = true;
                }
                else {                    
                    NavigationMessenger.GetInstance().IsRWAccount = false;
                }
            });

            worker.RunWorkerAsync();

            BeginWaitingSequence();
        }
    }
}