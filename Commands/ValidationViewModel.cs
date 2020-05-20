using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.MVVM.Commands
{
    class ValidationViewModel : IDataErrorInfo
    {
        // Eingabevalidierung (Benutzer)
        // Binding an TextBlock, der un-/sichtbar geschaltet werden kann (HasErrors)
        public string Error
        {
            get
            {
                string joinedErrors = null;

                // läuft durch das Dictionary Errors durch und fügt Einträge zu einem Error zusammen
                foreach (string key in Errors.Keys)
                {
                    if (!String.IsNullOrEmpty(joinedErrors))
                    {
                        joinedErrors += "\n";
                    }

                    joinedErrors += Errors[key];
                }

                return joinedErrors;
            }
        }

        // speichert property und errormessage
        public Dictionary<string, string> Errors { get; }

        // gets errormessage für das übergebene Property
        public string this[string columnName] => ValidateProperty(columnName);

        // überprüft, ob es einen Error gibt -> bool
        public bool HasErrors
        {
            get { return !String.IsNullOrEmpty(Error); }
        }

        // Methode überprüft, ob das Property im Errors Dictionary vorhanden ist
        // Wenn ja: gets propertyName und gibts zurück
        // Wenn nein: wird null zurückgegeben
        private string ValidateProperty(string propertyName)
        {
            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        public ValidationViewModel()
        {
            Errors = new Dictionary<string, string>();
        }
    }
}
