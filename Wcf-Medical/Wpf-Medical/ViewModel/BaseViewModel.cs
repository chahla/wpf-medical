using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace Wpf_Medical.ViewModel
{
    /// <summary>
    /// classe de base pour les view Model avec les méthodes communes
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Constructor

        protected BaseViewModel()
        {
        }

        #endregion // Constructor

        #region DisplayName

        /// <summary>
        /// Retourne le nom de l'objet
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        #endregion

        #region Debugging Aides

        /// <summary>
        /// Utilise la reflexion pour vérifier que l'élément binder existe bien
        /// Ne s'active qu'à la compilation par debug
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {

            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>

        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Event déclancher quand une propriété est changée
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Déclenche l'évenement
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changé</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
