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


        public ObservableCollection<ServicePatient.Patient> ListPatient
        {
            get { return _listPatient; }
            set { _listPatient = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientBrowserViewModel(Page lkView)
        {
            _linkedView = lkView;

            _imageCommand = new RelayCommand(param => ImageAccess(param), param => true);

            _createObservationCommand = new RelayCommand(param => CreateObservationClick(), param => true);


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

        /// <summary>
        /// réponse à la commande click
        /// </summary>
        private void CreateObservationClick()
        {

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
