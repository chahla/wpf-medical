﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.IO;

namespace Wpf_Medical.ViewModel
{
    class PatientPictureViewModel : BaseViewModel
    {
        private Page _linkedView = null;
        private NavigationService _ns = null;

        #region Commandes
        private ICommand _navigateToBrowserPatientCommand;
        private ICommand _graphCommand;

        public ICommand NavigateToBrowserPatientCommand
        {
            get { return _navigateToBrowserPatientCommand; }
            set { _navigateToBrowserPatientCommand = value; }
        }

        public ICommand GraphCommand
        {
            get { return _graphCommand; }
            set { _graphCommand = value; }
        }
        #endregion

        private byte[][] _listImages;

        public byte[][] ListImages
        {
            get { return _listImages; }
            set { _listImages = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientPictureViewModel(Page lkView, byte[][] byteArrayImages)
        {
            _linkedView = lkView;

            _navigateToBrowserPatientCommand = new RelayCommand(param => NavigateToBrowserPatient(), param => true);
            _graphCommand = new RelayCommand(param => NavigateToGraph(), param => true);

            _listImages = byteArrayImages;
        }

        /// <summary>
        /// réponse à la commande click
        /// </summary>
        private void NavigateToBrowserPatient()
        {
            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.GoBack();
        }


        private void NavigateToGraph()
        {
            View.PatientGraphView window = new View.PatientGraphView();
            ViewModel.PatientGraphViewModel vm = new PatientGraphViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }
    }
}
