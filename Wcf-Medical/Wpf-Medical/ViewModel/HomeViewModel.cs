﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;

namespace Wpf_Medical.ViewModel
{
    class HomeViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        #region Commandes
        private ICommand manageUserCommand;
        private ICommand managePatientCommand;
        private ICommand manageObservationCommand;
        private ICommand manageImageCommand;
        private ICommand _createCommand;        
        #endregion

        public ICommand ManageUserCommand
        {
            get { return manageUserCommand; }
            set { manageUserCommand = value; }
        }

        public ICommand ManagePatientCommand
        {
            get { return managePatientCommand; }
            set { managePatientCommand = value; }
        }

        public ICommand ManageObservationCommand
        {
            get { return manageObservationCommand; }
            set { manageObservationCommand = value; }
        }

        public ICommand ManageImageCommand
        {
            get { return manageImageCommand; }
            set { manageImageCommand = value; }
        }

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; }
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public HomeViewModel(Page lkView)
        {
            _linkedView = lkView;

            manageUserCommand = new RelayCommand(param => ClickUser(), param => true);
            managePatientCommand = new RelayCommand(param => ClickPatient(), param => true);
            manageObservationCommand = new RelayCommand(param => ClickObservation(), param => true);
            manageImageCommand = new RelayCommand(param => ClickImage(), param => true);
            _createCommand = new RelayCommand(param => ClickCreate(), param => true);

            BackgroundWorker worker = new BackgroundWorker();
            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(new ServiceLiveCallback());

            ServiceLive.ServiceLiveClient liveService = new ServiceLive.ServiceLiveClient(context);

            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) => 
            {
                liveService.Subscribe();
            });
            worker.RunWorkerAsync();
        }

        private void ClickUser()
        {
            View.MemberBrowserView window = new View.MemberBrowserView();
            ViewModel.MemberBrowserViewModel vm = new MemberBrowserViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        private void ClickPatient()
        {
            View.PatientBrowserView window = new View.PatientBrowserView();
            ViewModel.PatientBrowserViewModel vm = new PatientBrowserViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        private void ClickObservation()
        {
            View.ObservationTableView window = new View.ObservationTableView();
            ViewModel.ObservationTableViewModel vm = new ObservationTableViewModel(window);
            window.DataContext = vm;

            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        private void ClickCreate()
        {
            View.CreateUserView window = new View.CreateUserView();
            ViewModel.CreateUserViewModel vm = new CreateUserViewModel(window);
            window.DataContext = vm;

            /// Afin de pouvoir naviguer entre les pages mais que les ViewModel ne savent pas 
            /// du tout qui elles sont liees, on garde une trace de la page liee UNIQUEMENT 
            /// pour avoir acces a son navigation service
            _ns = NavigationService.GetNavigationService(_linkedView);
            _ns.Navigate(window);
        }

        private void ClickImage()
        {

        }
    }
}
