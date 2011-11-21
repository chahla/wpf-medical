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

namespace Wpf_Medical.ViewModel
{
    class MemberBrowserViewModel : BaseViewModel
    {
        private Page _linkedView = null;
        private NavigationService _ns = null;

        #region Commandes
        public ICommand ClickCommand { get; set; }
        #endregion

        ObservableCollection<ServiceUser.User> _listUser = null;

        public ObservableCollection<ServiceUser.User> ListUser
        {
            get { return _listUser; }
            set
            {
                _listUser = value;

            }
        }



        /// <summary>
        /// constructeur
        /// </summary>
        public MemberBrowserViewModel(Page lkView)
        {
            _linkedView = lkView;

            ClickCommand = new RelayCommand(param => Click(), param => true);

            _listUser = new ObservableCollection<ServiceUser.User>();

            /// Lors de la creation de la fenetre nous chargons la liste des utilisateurs
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler((object s, DoWorkEventArgs e) =>
            {
                Debug.WriteLine("DEBUT");
                ServiceUser.ServiceUserClient serviceUser = new ServiceUser.ServiceUserClient();
                e.Result = serviceUser.GetListUser();
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
                ServiceUser.User[] res = e.Result as ServiceUser.User[];
                if (res != null)
                {
                    foreach (ServiceUser.User item in res)
                    {
                        _listUser.Add(item);
                    }
                }
                else 
                {
                    Debug.WriteLine("LISTE DES USERS NON RECUPERABLES");
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
