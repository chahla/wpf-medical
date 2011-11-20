using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;

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

        private void ClickImage()
        {

        }
    }
}
