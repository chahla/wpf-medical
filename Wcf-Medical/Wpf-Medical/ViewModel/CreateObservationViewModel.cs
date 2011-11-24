using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Wpf_Medical.ViewModel
{
    class CreateObservationViewModel : BaseViewModel
    {
        private Page _linkedView = null;
        private NavigationService _ns = null;

        private string _weight;
        private string _bloodPressure;
        private string _comment;
        private string _prescriptionInput;
        private string[] _arrayPrescription;

        public string Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public string BloodPressure
        {
            get { return _bloodPressure; }
            set { _bloodPressure = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string PrescriptionInput
        {
            get { return _prescriptionInput; }
            set { _prescriptionInput = value; }
        }

        public string[] ArrayPrescription
        {
            get { return _arrayPrescription; }
            set { _arrayPrescription = value; }
        }

        public CreateObservationViewModel(Page lkView)
        {
            _linkedView = lkView;
        }
    }
}
