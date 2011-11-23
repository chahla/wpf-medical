using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Input;
using System.ComponentModel;
using System.Diagnostics;

namespace Wpf_Medical.ViewModel
{
    class PatientAddViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        #region Commandes
        private ICommand _createCommand;
        #endregion

        private string _name;
        private string _firstname;
        private DateTime _birthday;

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; }
        }

        private string _waitingMessage;

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

        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                if (_birthday != value)
                {
                    OnPropertyChanged("Birthday");
                }
                _birthday = value;
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

        // Si vrai, empêche le clic sur le bouton
        private bool _iscreatingpatient;

        public bool Iscreatingpatient
        {
            get { return _iscreatingpatient; }
            set { _iscreatingpatient = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientAddViewModel(Page lkView)
        {
            _linkedView = lkView;

            _createCommand = new RelayCommand(param => CreatePatient(), param => IsValidForm());

            _name = "";
            _firstname = "";
            _birthday = new DateTime();

            _iscreatingpatient = false;
            _waitingMessage = "";
        }

        private bool IsValidForm()
        {
            return (
                _name.Length != 0 &&
                _firstname.Length != 0 &&
                _birthday != null &&
                !_iscreatingpatient
            );
        }

        private void CreatePatient()
        {
            BackgroundWorker worker = new BackgroundWorker();

            ServicePatient.Patient newPatient = new ServicePatient.Patient();

            newPatient.Name = _name;
            newPatient.Firstname = _firstname;
            newPatient.Birthday = _birthday;

            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                ServicePatient.ServicePatientClient servicePatient = new ServicePatient.ServicePatientClient();

                Debug.WriteLine("DEBUT");
                _iscreatingpatient = true;

                BackgroundWorker bg = s as BackgroundWorker;
                e.Result = servicePatient.AddPatient(newPatient);
            });

            // TODO penser a mettre un comportement en fonction des differents cas notamment en cas de fail
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object s, RunWorkerCompletedEventArgs e) =>
            {
                Debug.WriteLine("FIN");
                _iscreatingpatient = false;
                WaitingMessage = "";

                if (e.Cancelled)
                {
                    Debug.WriteLine("CANCELLED");
                    WaitingMessage = "L'opération a été annulée.";
                }
                if (e.Error != null)
                {
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
                    //NavigationMessenger.GetInstance().TransitCreatedUser = newPatient;
                    WaitingMessage = "Création réussie";

                    View.PatientBrowserView window = new View.PatientBrowserView();
                    ViewModel.PatientBrowserViewModel vm = new PatientBrowserViewModel(window);
                    window.DataContext = vm;

                    _ns = NavigationService.GetNavigationService(_linkedView);
                    _ns.Navigate(window);
                    WaitingMessage = "";
                }
                else
                {
                    Debug.WriteLine("ECHEC DE LA CREATION");
                    WaitingMessage = "La création a échoué. Veuillez recommencer.";
                }
            });

            worker.RunWorkerAsync();
            WaitingMessage = "Création du patient";
        }
    }
}
