using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf_Medical
{
    /// <summary>
    /// Afin de pouvoir transmettre de l'information entre les pages
    /// nous avons un singleton qui contiendra les informations entre 
    /// deux pages
    /// 
    /// Un souci technique peut se poser quant au choix du type des _transit*
    /// En effet, entre stocker des types simples (string) ou exposes 
    /// par le webservice le choix la difference semble moindre
    /// </summary>
    class NavigationMessenger
    {
        private static NavigationMessenger _instance = null;
        private static readonly object _lock = new object();

        /// <summary>
        /// Cet attribut est utile entre la page de creation de compte 
        /// et celle de validation car on a besion d'afficher les donnees
        /// </summary>
        private ServiceUser.User _transitUser;

        public ServiceUser.User TransitUser
        {
            get { return _transitUser; }
            set { _transitUser = value; }
        }

        private NavigationMessenger()
        {
            _transitUser = new ServiceUser.User();
        }

        public static NavigationMessenger GetInstance()
        {
            lock ((_lock))
            {
                if (_instance == null)
                {
                    _instance = new NavigationMessenger();
                }
                return _instance;
            }
        }
    }
}
