using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;

namespace Wpf_Medical.ViewModel
{
    class CreateUserViewModel : BaseViewModel
    {
        private Page _linkedView;

        #region Commandes
        private ICommand _clickCommand;

        public ICommand ClickCommand
        {
            get { return _clickCommand; }
            set { _clickCommand = value; }
        }
        #endregion

        /// <summary>
        /// constructeur
        /// </summary>
        public CreateUserViewModel(Page lkView)
        {
            _linkedView = lkView;

            _clickCommand = new RelayCommand(param => Click(), param => true);
        }

        /// <summary>
        /// réponse à la commande click
        /// </summary>
        private void Click()
        {

        }
    }
}
