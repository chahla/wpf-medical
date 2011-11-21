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
        #endregion

        public List<ServicePatient.Patient> ListPatient
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

            ClickCommand = new RelayCommand(param => Click(), param => true);

            _listPatient = new List<ServicePatient.Patient>();

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                Debug.WriteLine("DEBUT");
                ServicePatient.ServicePatientClient patientService = new ServicePatient.ServicePatientClient();
                //meme en attendant  5 min
                //patientService.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);
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

        /// <summary>
        /// réponse à la commande click
        /// </summary>
        private void Click()
        {

        }
    }
}
