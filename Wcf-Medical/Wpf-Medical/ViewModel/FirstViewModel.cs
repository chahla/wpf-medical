using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace Wpf_Medical.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Commandes
        public ICommand ClickCommand { get; set; }
        #endregion

        /// <summary>
        /// constructeur
        /// </summary>
        public LoginViewModel()
        {
            ClickCommand = new RelayCommand(param => Click(), param => true);
        }

        /// <summary>
        /// réponse à la commande click
        /// </summary>
        private void Click()
        {

        }
    }
}
