using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Input;

namespace Wpf_Medical.ViewModel
{
    class PatientGraphViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        #region Commandes
        private ICommand _navigateToBrowserPatientCommand;

        public ICommand NavigateToBrowserPatientCommand
        {
            get { return _navigateToBrowserPatientCommand; }
            set { _navigateToBrowserPatientCommand = value; }
        }
        #endregion

        private List<KeyValuePair<string, double>> _data;
        private List<KeyValuePair<string, double>> _dataheart;

        public List<KeyValuePair<string, double>> Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged("Data");
                }
            }
        }

        public List<KeyValuePair<string, double>> Dataheart
        {
            get { return _dataheart; }
            set
            {
                if (_dataheart != value)
                {
                    _dataheart = value;
                    OnPropertyChanged("Dataheart");
                }
            }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientGraphViewModel(Page lkView)
        {
            _linkedView = lkView;

            _navigateToBrowserPatientCommand = new RelayCommand(param => NavigateToBrowserPatient(), param => true);

            _data = new List<KeyValuePair<string, double>>();
            _dataheart = new List<KeyValuePair<string, double>>();
            _data = ServiceLiveCallback.Templist.Take<KeyValuePair<string, double>>(ServiceLiveCallback.Templist.Count).ToList();            
            _dataheart = ServiceLiveCallback.Heartlist.Take<KeyValuePair<string, double>>(ServiceLiveCallback.Heartlist.Count).ToList();
        }

        private void NavigateToBrowserPatient()
        {
            View.PatientBrowserView window = new View.PatientBrowserView();
            ViewModel.PatientBrowserViewModel vm = new PatientBrowserViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }
    }
}
