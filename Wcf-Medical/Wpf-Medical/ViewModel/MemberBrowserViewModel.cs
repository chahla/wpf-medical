using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Wpf_Medical.ViewModel
{
    class MemberBrowserViewModel : BaseViewModel
    {
        private Page _linkedView;
        private NavigationService _ns;

        #region Commandes
        public ICommand ClickCommand { get; set; }
        #endregion

        /// <summary>
        /// constructeur
        /// </summary>
        public MemberBrowserViewModel(Page lkView)
        {
            _linkedView = lkView;

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
