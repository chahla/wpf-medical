using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Wpf_Medical.ViewModel
{
    class CreateObservationViewModel : BaseViewModel
    {
        private Page _linkedView = null;
        private NavigationService _ns = null;

        private int _idPatient;

        private string _weight = "";
        private string _bloodPressure = "";
        private string _comment = "";
        private string _prescriptionInput = "";
        private ObservableCollection<string> _arrayPrescription;
        private string _selectedEntry = null;
        private ObservableCollection<BitmapImage> _listDisplayedImages;
        private BitmapImage _selectedImage = null;

        private string _waitingMessage = "";

        public string WaitingMessage
        {
            get { return _waitingMessage; }
            set
            {
                if (_waitingMessage != value)
                {
                    OnPropertyChanged("WaitingMessage");
                    _waitingMessage = value;
                }
            }
        }

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
            set
            {
                if (_prescriptionInput != value)
                {
                    OnPropertyChanged("PrescriptionInput");
                }
                _prescriptionInput = value;
            }
        }

        public ObservableCollection<string> ArrayPrescription
        {
            get { return _arrayPrescription; }
            set
            {
                if (_arrayPrescription != value)
                {
                    OnPropertyChanged("ArrayPrescription");
                }
                _arrayPrescription = value;
            }
        }

        public ObservableCollection<BitmapImage> ListDisplayedImages
        {
            get { return _listDisplayedImages; }
            set { _listDisplayedImages = value; }
        }

        public string SelectedEntry
        {
            get { return _selectedEntry; }
            set { _selectedEntry = value; }
        }

        public BitmapImage SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; }
        }

        private ICommand _addToListCommand;
        private ICommand _removeToListCommand;
        private ICommand _imageCommand;
        private ICommand _navigateToHomeCommand;
        private ICommand _removeImageCommand;
        private ICommand _validCommand;

        public ICommand AddToListCommand
        {
            get { return _addToListCommand; }
            set { _addToListCommand = value; }
        }

        public ICommand RemoveToListCommand
        {
            get { return _removeToListCommand; }
            set { _removeToListCommand = value; }
        }

        public ICommand ImageCommand
        {
            get { return _imageCommand; }
            set { _imageCommand = value; }
        }

        public ICommand NavigateToHomeCommand
        {
            get { return _navigateToHomeCommand; }
            set { _navigateToHomeCommand = value; }
        }

        public ICommand RemoveImageCommand
        {
            get { return _removeImageCommand; }
            set { _removeImageCommand = value; }
        }

        public ICommand ValidCommand
        {
            get { return _validCommand; }
            set { _validCommand = value; }
        }

        public CreateObservationViewModel(Page lkView, int id)
        {
            _linkedView = lkView;

            _idPatient = id;

            _addToListCommand = new RelayCommand(param => AddToList(), param => IsPrescriptionInputEmpty());
            _removeToListCommand = new RelayCommand(param => RemoveToList(), param => IsSelectedEntry());
            _imageCommand = new RelayCommand(param => SelectImage(), param => true);
            _navigateToHomeCommand = new RelayCommand(param => NavigateToHome(), param => true);
            _removeImageCommand = new RelayCommand(param => RemoveImage(), param => IsSelectedImage());
            _validCommand = new RelayCommand(param => CreateObservation(), param => IsFormValid());

            _arrayPrescription = new ObservableCollection<string>();
            _listDisplayedImages = new ObservableCollection<BitmapImage>();
        }

        private bool IsPrescriptionInputEmpty()
        {
            return _prescriptionInput.Length != 0;
        }

        private bool IsSelectedEntry()
        {
            return _selectedEntry != null;
        }

        private bool IsSelectedImage()
        {
            return _selectedImage != null;
        }

        private bool IsFormValid()
        {
            return ((_weight.Length != 0) && (_bloodPressure.Length != 0));
        }

        private void AddToList()
        {
            _arrayPrescription.Add(_prescriptionInput);
            Debug.WriteLine(_arrayPrescription);
        }

        private void RemoveToList()
        {
            if (_selectedEntry != null) {
                ArrayPrescription.Remove(_selectedEntry);
            }
        }

        private void RemoveImage()
        {
            if (_selectedImage != null) {
                _listDisplayedImages.Remove(_selectedImage);
            }
        }

        private void SelectImage()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Images Files (*.jpg)|*.jpg";
            ///
            // Peut-etre informer l'utilisateur que les images ne doivent pas depasser 128*128
            ///

            openFileDialog1.FileOk += new CancelEventHandler((object s, CancelEventArgs e) =>
            {
                string path = openFileDialog1.FileName;
                BitmapImage bitimg = new BitmapImage(new Uri(path));

                _listDisplayedImages.Add(bitimg);
            });
            openFileDialog1.ShowDialog();
        }

        private void NavigateToHome()
        {
            View.PatientBrowserView window = new View.PatientBrowserView();
            ViewModel.PatientBrowserViewModel vm = new ViewModel.PatientBrowserViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        private void CreateObservation()
        {
            ServiceObservation.Observation newObs = new ServiceObservation.Observation();

            int tmp = 0;
            if (Int32.TryParse(_weight, out tmp)) {
                newObs.Weight = tmp;
            }

            if (Int32.TryParse(_bloodPressure, out tmp)) {
                newObs.BloodPressure = tmp;
            }

            if (_comment != null) {
                newObs.Comment = _comment;
            }

            if (_arrayPrescription != null) {
                newObs.Prescription = _arrayPrescription.ToArray();
            }

            if (_listDisplayedImages != null && _listDisplayedImages.Count != 0)
            {
                /// Le nombre d'images voulues utile pour creer le tableau statique
                int finalSize = _listDisplayedImages.Count;

                /// Le tableau d'images final
                byte[][] finalArrayImages = new byte[finalSize][];

                ///Pour convertir nos images en byte[]
                ByteArrayConverter cv = new ByteArrayConverter();

                for (int i = 0; i < finalSize; i++)
                {
                    byte[] convertedImg = (byte[])cv.ConvertBack(_listDisplayedImages.ElementAt(i), null, null, null);
                    finalArrayImages[i] = convertedImg;
                }
                newObs.Pictures = finalArrayImages;

                newObs.Date = DateTime.Now;
            }

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                ServiceObservation.ServiceObservationClient observService = new ServiceObservation.ServiceObservationClient();
                e.Result = observService.AddObservation(_idPatient, newObs);
            });

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object s, RunWorkerCompletedEventArgs e) =>
            {
                WaitingMessage = "";
                if (e.Cancelled)
                {
                    WaitingMessage = "L'opération a été annulée.";
                }
                if (e.Error != null)
                {
                    WaitingMessage = "Erreur lors de la création : " + e.Error.Message;
                }
                bool? resWebService = e.Result as bool?;
                if (resWebService.HasValue && resWebService.Value)
                {
                    View.PatientBrowserView window = new View.PatientBrowserView();
                    ViewModel.PatientBrowserViewModel vm = new PatientBrowserViewModel(window);
                    window.DataContext = vm;

                    _ns = NavigationService.GetNavigationService(_linkedView);
                    _ns.Navigate(window);
                }
                else {
                    WaitingMessage = "Erreur côté serveur lors de la création. Veuillez recommencer";
                }

            });

            worker.RunWorkerAsync();
            WaitingMessage = "Ajout de l'observation en cours";
        }
    }
}
