using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Wpf_Medical.ViewModel
{
    class PatientPictureViewModel : BaseViewModel
    {
        private Page _linkedView = null;
        private NavigationService _ns = null;

        #region Commandes
        public ICommand ClickCommand { get; set; }
        #endregion

        /// <summary>
        /// constructeur
        /// </summary>
        public PatientPictureViewModel(Page lkView, byte[][] listImages)
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
