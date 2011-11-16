﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Wpf_Medical.ViewModel
{
    class PictureManager2ViewModel : BaseViewModel
    {
        #region Commandes
        public ICommand ClickCommand { get; set; }
        #endregion

        /// <summary>
        /// constructeur
        /// </summary>
        public PictureManager2ViewModel()
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
