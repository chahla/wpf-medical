using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows;

namespace Wpf_Medical.ViewModel
{
    class PatientBrowserViewModel : BaseViewModel
    {
        private Page _linkedView = null;
        private NavigationService _ns = null;

        /// <summary>
        ///  La liste des patients obtenue via le webservice
        /// </summary>
        private ObservableCollection<ServicePatient.Patient> _listPatient;

        /// <summary>
        /// Le patient actuellement selectionne dans la liste
        /// </summary>
        private ServicePatient.Patient _selectedPatient = null;

        public ServicePatient.Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                if (_selectedPatient != value)
                {
                    OnPropertyChanged("SelectedPatient");
                }
                _selectedPatient = value;
            }
        }

        /// <summary>
        /// Afin d'activer ou non les boutons de creation/suppression on recupere 
        /// le booleen de droits de l'utilisateur actuel dans le systeme de session 
        /// represente par la classe singleton NavigationService
        /// Au cas ou une faille apparaitrait les boutons sont desactives de base
        /// </summary>
        private Visibility _isAvailableRW = Visibility.Hidden;

        public Visibility IsAvailableRW
        {
            get { return _isAvailableRW; }
            set
            {
                if (_isAvailableRW != value)
                {
                    OnPropertyChanged("IsAvailableRW");
                }
                _isAvailableRW = value; }
        }

        private ICommand _createObservationCommand;
        private ICommand _imageCommand;
        private ICommand _navigateToHomeCommand;

        public ICommand NavigateToHomeCommand
        {
            get { return _navigateToHomeCommand; }
            set { _navigateToHomeCommand = value; }
        }


        public ICommand ImageCommand
        {
            get { return _imageCommand; }
            set { _imageCommand = value; }
        }

        public ICommand CreateObservationCommand
        {
            get { return _createObservationCommand; }
            set { _createObservationCommand = value; }
        }

        private ICommand _addPatientCommand;
        private ICommand _deletePatientCommand;

        public ICommand DeletePatientCommand
        {
            get { return _deletePatientCommand; }
            set { _deletePatientCommand = value; }
        }


        public ObservableCollection<ServicePatient.Patient> ListPatient
        {
            get { return _listPatient; }
            set { _listPatient = value; }
        }

        public ICommand AddPatientCommand
        {
            get { return _addPatientCommand; }
            set { _addPatientCommand = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientBrowserViewModel(Page lkView)
        {
            _linkedView = lkView;

            _imageCommand = new RelayCommand(param => ImageAccess(param), param => true);

            _createObservationCommand = new RelayCommand(param => NavigateToCreateObservation(param), param => IsButtonAvailable());
            _addPatientCommand = new RelayCommand(param => ClickAddPatient(), param => true);
            _navigateToHomeCommand = new RelayCommand(param => NavigateToHome(), param => true);
            _deletePatientCommand = new RelayCommand(param => NavigateToDeletePatient(param), param => IsButtonAvailable());

            /// Definit si les bouton de creation/suppression est disponible ou non
            if (NavigationMessenger.GetInstance().IsRWAccount) {
                IsAvailableRW = Visibility.Visible;
            }
            else {
                IsAvailableRW = Visibility.Hidden;
            }
            

            _listPatient = new ObservableCollection<ServicePatient.Patient>();

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                Debug.WriteLine("DEBUT");
                ServicePatient.ServicePatientClient patientService = new ServicePatient.ServicePatientClient();
                e.Result = patientService.GetListPatient();
            });

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object s, RunWorkerCompletedEventArgs e) =>
            {
                Debug.WriteLine("FIN");
                if (e.Cancelled)
                {
                    Debug.WriteLine("CANCEL");
                }
                if (e.Error != null)
                {
                    Debug.WriteLine("ERREUR");
                }
                if (e.Result == null)
                {
                    Debug.WriteLine("NULL");
                }
                else
                {
                    ServicePatient.Patient[] res = e.Result as ServicePatient.Patient[];
                    if (res != null)
                    {
                        foreach (ServicePatient.Patient item in res)
                        {
                            _listPatient.Add(item);
                        }
                    }
                    else {
                        Debug.WriteLine("LISTE DES PATIENTS NON RECUPERABLE");
                    }
                }
            });

            worker.RunWorkerAsync();

        }

        private void ClickAddPatient()
        {
            View.PatientAddView window = new View.PatientAddView();
            ViewModel.PatientAddViewModel vm = new PatientAddViewModel(window);
            window.DataContext = vm;

            /// Afin de pouvoir naviguer entre les pages mais que les ViewModel ne savent pas 
            /// du tout qui elles sont liees, on garde une trace de la page liee UNIQUEMENT 
            /// pour avoir acces a son navigation service
            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        /// <summary>
        /// réponse à la commande click
        /// </summary>
        private void NavigateToCreateObservation(object obj)
        {
            int? idSelectedPatient = obj as int?;
            if (idSelectedPatient.HasValue) {
                View.CreateObservationView window = new View.CreateObservationView();
                ViewModel.CreateObservationViewModel vm = new CreateObservationViewModel(window, idSelectedPatient.Value);
                window.DataContext = vm;

                _ns = NavigationService.GetNavigationService(_linkedView);
                _ns.Navigate(window);
            }
        }

        /// <summary>
        /// Predicat indiquant si le bouton de suppression est active ou non
        /// </summary>
        /// <returns></returns>
        private bool IsButtonAvailable()
        {
            return (SelectedPatient != null);
        }

        /// <summary>
        /// Se charge de naviguer vers la page de suppression
        /// </summary>
        /// <param name="obj">l'id du patient</param>
        private void NavigateToDeletePatient(object obj)
        {
            int? idSelectedPatient = obj as int?;
            if (idSelectedPatient.HasValue)
            {
                View.PatientDeleteView window = new View.PatientDeleteView();
                ViewModel.PatientDeleteViewModel vm = new PatientDeleteViewModel(window, idSelectedPatient.Value);
                window.DataContext = vm;

                _ns = NavigationService.GetNavigationService(_linkedView);
                _ns.Navigate(window);
            }
        }

        private void NavigateToHome()
        {
            View.HomeView window = new View.HomeView();
            ViewModel.HomeViewModel vm = new HomeViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        private void ImageAccess(object obj)
        {
            byte[][] currentObservationImages = obj as byte[][];

            View.PatientPictureView window = new View.PatientPictureView();
            ViewModel.PatientPictureViewModel vm = new PatientPictureViewModel(window, currentObservationImages);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

    }
}
