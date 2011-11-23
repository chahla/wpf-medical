using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;
using System.ComponentModel;

namespace Wpf_Medical.ViewModel
{
    class CreateUserViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        #region Commandes
        private ICommand _createCommand;
        #endregion

        private string _name;
        private string _firstname;
        private string _login;
        private string _password;
        private string _confirmPassword;
        private string _role;
        private List<string> _availableRoleList;

        private string _waitingMessage;

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; }
        }

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
                if (_password!= value)
                {
                    OnPropertyChanged("Password");
                }
                _password = value;
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword != value)
                {
                    OnPropertyChanged("ConfirmPassword");
                }
                _confirmPassword = value;
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

        public List<string> AvailableRoleList
        {
            get { return _availableRoleList; }
            set { _availableRoleList = value; }
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

        // Si vrai, empêche le clic sur le bouton
        private bool _iscreatingaccount;

        public bool Iscreatingaccount
        {
            get { return _iscreatingaccount; }
            set { _iscreatingaccount = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public CreateUserViewModel(Page lkView)
        {
            _linkedView = lkView;

            _createCommand = new RelayCommand(param => CreateAccount(), param => IsValidForm());

            _name = "";
            _firstname = "";
            _login = "";
            _password = "";
            _confirmPassword = "";

            _availableRoleList = new List<string>();
            _availableRoleList.Add("Chirurgien");
            _availableRoleList.Add("Infirmière");
            _availableRoleList.Add("Medecin");
            _availableRoleList.Add("Radiologue");

            _role = "Chirurgien";

            _iscreatingaccount = false;
            _waitingMessage = "";
        }

        /// <summary>
        /// La validation du formulaire
        /// A completer par la suite avec d'autres regles de validation plus robustes
        /// On verifie que les champs du mot de passe soient egaux
        /// La lambda expression vise a chercher si le choix de la combobox fait 
        /// partie de la liste possible
        /// </summary>
        private bool IsValidForm()
        {
            // TODO formulaire champ password non visible
            return (
                _name.Length != 0 && 
                _firstname.Length != 0 && 
                _login.Length != 0 && 
                _password.Length != 0 && 
                _confirmPassword.Length != 0 && 
                _password == _confirmPassword && 
                (_availableRoleList.Find(lambda => lambda == _role).Any()) &&
                !_iscreatingaccount
            );
        }

        /// <summary>
        /// La creation de compte va appeller un webservice dans un 
        /// BackgroundWorker pour laisser l'UI disponible
        /// </summary>
        private void CreateAccount()
        {
            BackgroundWorker worker = new BackgroundWorker();

            ServiceUser.User newUser = new ServiceUser.User();

            newUser.Name = _name;
            newUser.Firstname = _firstname;
            newUser.Connected = false;
            newUser.Picture = null;
            newUser.Login = _login;
            newUser.Pwd = _password;
            newUser.Role = _role;

            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                ServiceUser.ServiceUserClient serviceUser = new ServiceUser.ServiceUserClient();

                Debug.WriteLine("DEBUT");
                _iscreatingaccount = true;

                BackgroundWorker bg = s as BackgroundWorker;
                e.Result = serviceUser.AddUser(newUser);
            });

            // TODO penser a mettre un comportement en fonction des differents cas notamment en cas de fail
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object s, RunWorkerCompletedEventArgs e) =>
            {
                Debug.WriteLine("FIN");
                _iscreatingaccount = false;
                WaitingMessage = "";

                if (e.Cancelled) {
                    Debug.WriteLine("CANCELLED");
                    WaitingMessage = "L'opération a été annulée.";
                }
                if (e.Error != null) {
                    Debug.WriteLine("ERROR");
                    WaitingMessage = "Erreur lors de la création : " + e.Error.Message;
                }
                bool? res = e.Result as bool?;

                if (res == null)
                {
                    Debug.WriteLine("ERREUR COTE SERVEUR");
                    WaitingMessage = "Erreur côté serveur lors de la création. Veuillez recommencer";
                }
                if (res == true)
                {
                    /// Juste avant de creer la page de confirmation il faut 
                    /// enregister les informations dans le NavigationMessenger
                    NavigationMessenger.GetInstance().TransitCreatedUser = newUser;
                    WaitingMessage = "Création réussie";

                    View.CreateUserSuccessView window = new View.CreateUserSuccessView();
                    ViewModel.CreateUserSuccessViewModel vm = new CreateUserSuccessViewModel(window);
                    window.DataContext = vm;

                    _ns = NavigationService.GetNavigationService(_linkedView);
                    _ns.Navigate(window);
                    WaitingMessage = "";
                }
                else {
                    Debug.WriteLine("ECHEC DE L'INSCRIPTION");
                    WaitingMessage = "L'inscription a échoué. Veuillez recommencer.";
                }
            });

            worker.RunWorkerAsync();
            WaitingMessage = "Création du compte";
        }
    }
}
