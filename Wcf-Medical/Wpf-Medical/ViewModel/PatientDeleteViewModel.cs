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
    class PatientDeleteViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        #region Commandes
        private ICommand _deleteCommand;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set { _deleteCommand = value; }
        }
        #endregion

        private string _name;
        private string _firstname;
        private DateTime _birthday;
               

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

        public DateTime Login
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
                    OnPropertyChanged("WaitingMessage");
                }
                _waitingMessage = value;
            }
        }

        private bool _isdeleting;

        public bool Isdeleting
        {
            get { return _isdeleting; }
            set { _isdeleting = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientDeleteViewModel(Page lkView)
        {
            _linkedView = lkView;

            _deleteCommand = new RelayCommand(param => Delete(), param => IsDeleting());
            
            ///
            // ICI IL FAUT AJOUTE RUN NAVIGATION MESSENGER POUR GERER LE PATIENT SELECTIONNE
            ///

            ServicePatient.Patient selectedpatient = null;//NavigationMessenger.GetInstance().TransitCreatedUser;

            _name = selectedpatient.Name;
            _firstname = selectedpatient.Firstname;
            _birthday = selectedpatient.Birthday;
        }

        private bool IsDeleting()
        {
            return !_isdeleting;
        }

        /// <summary>
        /// Le clic sur le bouton permet de supprimer le patient
        /// </summary>
        private void Delete()
        {
            BackgroundWorker worker = new BackgroundWorker();
            
            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                ServicePatient.ServicePatientClient servicePatient = new ServicePatient.ServicePatientClient();

                Debug.WriteLine("DEBUT");
                _isdeleting = true;

                BackgroundWorker bg = s as BackgroundWorker;
                e.Result = servicePatient.DeletePatient(1);
            });

            // TODO penser a mettre un comportement en fonction des differents cas notamment en cas de fail
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object s, RunWorkerCompletedEventArgs e) =>
            {
                Debug.WriteLine("FIN");
                _isdeleting = false;
                WaitingMessage = "";

                if (e.Cancelled)
                {
                    Debug.WriteLine("CANCELLED");
                    WaitingMessage = "L'opération a été annulée.";
                }
                if (e.Error != null)
                {
                    Debug.WriteLine("ERROR");
                    WaitingMessage = "Erreur lors de la suppression : " + e.Error.Message;
                }
                bool? res = e.Result as bool?;

                if (res == null)
                {
                    Debug.WriteLine("ERREUR COTE SERVEUR");
                    WaitingMessage = "Erreur côté serveur lors de la suppression. Veuillez recommencer";
                }
                if (res == true)
                {
                    WaitingMessage = "Suppression réussie";

                    View.PatientBrowserView window = new View.PatientBrowserView();
                    ViewModel.PatientBrowserViewModel vm = new PatientBrowserViewModel(window);
                    window.DataContext = vm;

                    _ns = NavigationService.GetNavigationService(_linkedView);
                    _ns.Navigate(window);
                    WaitingMessage = "";
                }
                else
                {
                    Debug.WriteLine("ECHEC DE LA SUPPRESSION");
                    WaitingMessage = "La suppression a échoué. Veuillez recommencer.";
                }
            });

            worker.RunWorkerAsync();
            WaitingMessage = "Suppression du patient";

            
        }
    }
}
