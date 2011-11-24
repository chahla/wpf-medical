using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Wpf_Medical.ViewModel
{
    class PatientGraphViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        private List<KeyValuePair<string, double>> _data;

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

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientGraphViewModel(Page lkView)
        {
            _linkedView = lkView;

            _data = new List<KeyValuePair<string, double>>();
            _data = ServiceLiveCallback.Templist.Take<KeyValuePair<string, double>>(ServiceLiveCallback.Templist.Count).ToList();
        }
    }
}
