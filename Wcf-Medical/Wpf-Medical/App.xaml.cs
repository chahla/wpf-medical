using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Wpf_Medical
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// permet de lancer la 1er fenêtre
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Remplacer par votre fenêtre
            View.FirstView window = new View.FirstView();
            ViewModel.FirstViewModel vm = new ViewModel.FirstViewModel();
            window.DataContext = vm;
            window.Show();
        }
    }
}
