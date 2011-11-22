using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Diagnostics;

namespace Wpf_Medical.ViewModel
{
    class PatientBrowserViewModel : BaseViewModel
    {
        private Page _linkedView = null;
        private NavigationService _ns = null;

        private List<ServicePatient.Patient> _listPatient;

        #region Commandes
        public ICommand ClickCommand { get; set; }
        private ICommand _addPatientCommand;
        #endregion

        public List<ServicePatient.Patient> ListPatient
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

            ClickCommand = new RelayCommand(param => Click(), param => true);
            _addPatientCommand = new RelayCommand(param => ClickAddPatient(), param => IsAllowed());

            _listPatient = new List<ServicePatient.Patient>();

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
                    Debug.WriteLine("OK");
                }
            });

            worker.RunWorkerAsync();

        }

        private bool IsAllowed()
        {
            return true;
        }

        private void ClickAddPatient()
        {
            View.PaientAddView window = new View.PaientAddView();
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
        private void Click()
        {

        }
    }
}
